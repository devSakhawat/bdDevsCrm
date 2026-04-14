using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Application.Services.Caching;

public interface IHybridCacheService
{
    Task<T?> OrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CacheProfile profile = CacheProfile.Default);
    Task<T?> Async<T>(string key);
    Task SetAsync<T>(string key, T value, TimeSpan? expiry = null);
    Task RemoveAsync(string key);
    Task RemoveByPrefixAsync(string prefix);
}

public enum CacheProfile
{
    Default,
    Static,      // 24 hours - Countries, Currencies, System Settings
    User,        // 4 hours - User profile, Permissions
    Dynamic,     // 15 minutes - Dashboard stats, Recent activities
    Session      // 30 minutes - User session data
}

public class HybridCacheService : IHybridCacheService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IDistributedCache _distributedCache;
    private readonly ILogger<HybridCacheService> _logger;
    private readonly IConfiguration _configuration;
    private readonly bool _enableL1Cache;
    private readonly bool _enableDistributedCache;
  private object _configurationGet;

  public HybridCacheService(
        IMemoryCache memoryCache,
        IDistributedCache distributedCache,
        ILogger<HybridCacheService> logger,
        IConfiguration configuration)
    {
        _memoryCache = memoryCache;
        _distributedCache = distributedCache;
        _logger = logger;
        _configuration = configuration;
        _enableL1Cache = configuration.GetValue<bool>("CacheSettings:EnableL1Cache", true);
        _enableDistributedCache = configuration.GetValue<bool>("CacheSettings:EnableDistributedCache", false);
    }

    public async Task<T?> OrSetAsync<T>(string key, Func<Task<T>> factory, TimeSpan? expiry = null, CacheProfile profile = CacheProfile.Default)
    {
        var cacheKey = CacheKey(key);
        var expiryTime = expiry ?? ExpiryForProfile(profile);

        // L1: Try memory cache first (fastest)
        if (_enableL1Cache && _memoryCache.TryGetValue<T>(cacheKey, out var memoryValue))
        {
            _logger.LogDebug("Cache HIT (L1 Memory): {Key}", cacheKey);
            return memoryValue;
        }

        // L2: Try distributed cache (Redis)
        if (_enableDistributedCache)
        {
            try
            {
                var distributedValue = await _distributedCache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(distributedValue))
                {
                    _logger.LogDebug("Cache HIT (L2 Redis): {Key}", cacheKey);
                    var value = JsonSerializer.Deserialize<T>(distributedValue);

                    // Store in L1 cache for faster subsequent access
                    if (_enableL1Cache && value != null)
                    {
                        var l1Expiry = TimeSpan.FromMinutes(5); // L1 cache expires faster
                        _memoryCache.Set(cacheKey, value, l1Expiry);
                    }

                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading from distributed cache for key: {Key}", cacheKey);
            }
        }

        // L3:  from source (database/API)
        _logger.LogDebug("Cache MISS: {Key} - Fetching from source", cacheKey);
        var data = await factory();

        if (data != null)
        {
            await SetAsync(cacheKey, data, expiryTime);
        }

        return data;
    }

    public async Task<T?> Async<T>(string key)
    {
        var cacheKey = CacheKey(key);

        // Try L1 first
        if (_enableL1Cache && _memoryCache.TryGetValue<T>(cacheKey, out var memoryValue))
        {
            return memoryValue;
        }

        // Try L2
        if (_enableDistributedCache)
        {
            try
            {
                var distributedValue = await _distributedCache.GetStringAsync(cacheKey);
                if (!string.IsNullOrEmpty(distributedValue))
                {
                    var value = JsonSerializer.Deserialize<T>(distributedValue);

                    if (_enableL1Cache && value != null)
                    {
                        _memoryCache.Set(cacheKey, value, TimeSpan.FromMinutes(5));
                    }

                    return value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading from distributed cache for key: {Key}", cacheKey);
            }
        }

        return default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? expiry = null)
    {
        var cacheKey = CacheKey(key);
        var expiryTime = expiry ?? TimeSpan.FromHours(1);

        // Set in L1 (Memory)
        if (_enableL1Cache)
        {
            var l1Expiry = expiryTime > TimeSpan.FromMinutes(5)
                ? TimeSpan.FromMinutes(5)
                : expiryTime;

            _memoryCache.Set(cacheKey, value, l1Expiry);
        }

        // Set in L2 (Redis)
        if (_enableDistributedCache)
        {
            try
            {
                var serialized = JsonSerializer.Serialize(value);
                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expiryTime
                };
                await _distributedCache.SetStringAsync(cacheKey, serialized, options);
                _logger.LogDebug("Cache SET: {Key} (Expiry: {Expiry})", cacheKey, expiryTime);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error writing to distributed cache for key: {Key}", cacheKey);
            }
        }
    }

    public async Task RemoveAsync(string key)
    {
        var cacheKey = CacheKey(key);

        // Remove from L1
        if (_enableL1Cache)
        {
            _memoryCache.Remove(cacheKey);
        }

        // Remove from L2
        if (_enableDistributedCache)
        {
            try
            {
                await _distributedCache.RemoveAsync(cacheKey);
                _logger.LogDebug("Cache REMOVED: {Key}", cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing from distributed cache for key: {Key}", cacheKey);
            }
        }
    }

    public async Task RemoveByPrefixAsync(string prefix)
    {
        // Note: Pattern-based removal requires Redis SCAN or key tracking
        _logger.LogWarning("Pattern-based cache removal requested for prefix: {Prefix}. Implement using Redis SCAN if needed.", prefix);

        // For now, just log the request
        await Task.CompletedTask;
    }

    private string CacheKey(string key)
    {
        var instanceName = _configuration["Redis:InstanceName"] ?? "bdDevCRM:";
        return $"{instanceName}{key}";
    }

    private TimeSpan ExpiryForProfile(CacheProfile profile)
    {
        return profile switch
        {
            CacheProfile.Static => TimeSpan.FromHours(_configuration.GetValue<int>("CacheSettings:CacheProfiles:Static:ExpirationHours", 24)),
            CacheProfile.User => TimeSpan.FromHours(_configuration.GetValue<int>("CacheSettings:CacheProfiles:User:ExpirationHours", 4)),
            CacheProfile.Dynamic => TimeSpan.FromMinutes(_configuration.GetValue<int>("CacheSettings:CacheProfiles:Dynamic:ExpirationMinutes", 15)),
            CacheProfile.Session => TimeSpan.FromMinutes(_configuration.GetValue<int>("CacheSettings:CacheProfiles:Session:ExpirationMinutes", 30)),
            _ => TimeSpan.FromMinutes(_configuration.GetValue<int>("CacheSettings:DefaultExpirationMinutes", 60))
        };
    }
}
