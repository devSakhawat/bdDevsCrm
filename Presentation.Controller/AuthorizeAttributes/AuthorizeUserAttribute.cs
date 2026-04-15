using Presentation.Extensions;
using Domain.Contracts.Services;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Presentation.AuthorizeAttributes;

/// <summary>
/// use IAsyncAuthorizationFilter — to prevent thread block
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeUserAttribute : Attribute, IAsyncAuthorizationFilter
{
  private const string USER_CACHE_PREFIX = "User_";
  private static readonly TimeSpan CACHE_SLIDING_EXPIRATION = TimeSpan.FromHours(5);
  private static readonly TimeSpan CACHE_ABSOLUTE_EXPIRATION = TimeSpan.FromHours(5);

  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    var httpContext = context.HttpContext;
    var env = httpContext.RequestServices.GetService<IHostEnvironment>();

    var serviceManager = httpContext.RequestServices.GetService<IServiceManager>();
    var memoryCache = httpContext.RequestServices.GetService<IMemoryCache>();
    var logger = httpContext.RequestServices.GetService<ILogger<AuthorizeUserAttribute>>();

    if (serviceManager == null || memoryCache == null)
    {
      logger?.LogCritical("Critical services not available");
      context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
      return;
    }

    try
    {
      var userIdClaim = httpContext.User.FindFirst("UserId")?.Value;

      if (string.IsNullOrWhiteSpace(userIdClaim) ||
          !int.TryParse(userIdClaim, out int userId) ||
          userId <= 0)
      {
        context.Result = new UnauthorizedObjectResult(new
        {
          StatusCode = 401,
          Message = "Authentication required. Please log in.",
          ErrorCode = "AUTH_NO_USER_CLAIM"
        });
        return;
      }

      // Cache check (sync — MemoryCache is thread-safe)
      var cacheKey = $"{USER_CACHE_PREFIX}{userId}";
      UsersDto? user = null;

      if (!memoryCache.TryGetValue(cacheKey, out user) || user == null)
      {
        // Async DB call — don't thread block
        user = await serviceManager.Users.UserAsync(userId, trackChanges: false);

        if (user == null)
        {
          context.Result = new UnauthorizedObjectResult(new
          {
            StatusCode = 401,
            Message = "User account not found.",
            ErrorCode = "AUTH_USER_NOT_FOUND"
          });
          return;
        }

        // Cache
        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(CACHE_SLIDING_EXPIRATION)
            .SetAbsoluteExpiration(CACHE_ABSOLUTE_EXPIRATION)
            .SetPriority(CacheItemPriority.High);

        memoryCache.Set(cacheKey, user, cacheOptions);
      }

      user.Password = string.Empty;
      httpContext.SetCurrentUser(user);
      httpContext.SetUserId(userId);
    }
    catch (Exception ex)
    {
      logger?.LogError(ex, "Authorization failed");
      var isDev = env?.IsDevelopment() ?? false;
      context.Result = new UnauthorizedObjectResult(new
      {
        StatusCode = 401,
        Message = isDev ? $"Auth failed: {ex.Message}" : "Authentication failed.",
        ErrorCode = "AUTH_EXCEPTION"
      });
    }
  }

  /// <summary>
  /// Manually clears user cache (useful for logout or user updates)
  /// Call this from your service layer when user data changes
  /// </summary>
  public static void ClearUserCache(IMemoryCache memoryCache, int userId)
  {
    var cacheKey = $"{USER_CACHE_PREFIX}{userId}";
    memoryCache.Remove(cacheKey);
  }
}














//using Presentation.Extensions;
//using Domain.Contracts.Services;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.Extensions.Caching.Memory;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using System.Diagnostics;

//namespace Presentation.AuthorizeAttributes;

///// <summary>
///// Optimized authorization filter with cache-first strategy and DB fallback
///// Ensures user context is available in controllers with minimal performance impact
///// </summary>
//[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
//public class AuthorizeUserAttribute : Attribute, IAuthorizationFilter
//{
//  private const string USER_CACHE_PREFIX = "User_";
//  private static readonly TimeSpan CACHE_SLIDING_EXPIRATION = TimeSpan.FromHours(5);
//  private static readonly TimeSpan CACHE_ABSOLUTE_EXPIRATION = TimeSpan.FromHours(5);

//  public void OnAuthorization(AuthorizationFilterContext context)
//  {
//    var httpContext = context.HttpContext;
//    var env = httpContext.RequestServices.Service<IHostEnvironment>();
//    var stopwatch = Stopwatch.StartNew();

//    //  required services
//    var serviceManager = httpContext.RequestServices.Service<IServiceManager>();
//    var memoryCache = httpContext.RequestServices.Service<IMemoryCache>();
//    var logger = httpContext.RequestServices.Service<ILogger<AuthorizeUserAttribute>>();

//    // Validate service dependencies
//    if (serviceManager == null || memoryCache == null)
//    {
//      logger?.LogCritical("Critical services (IServiceManager or IMemoryCache) are not available");
//      context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
//      return;
//    }

//    try
//    {
//      // Step 1: Extract and validate UserId claim from JWT token
//      var userIdClaim = httpContext.User.FindFirst("UserId")?.Value;

//      if (string.IsNullOrWhiteSpace(userIdClaim))
//      {
//        logger?.LogWarning("Authorization failed: UserId claim not found in token");
//        context.Result = new UnauthorizedObjectResult(new
//        {
//          StatusCode = 401,
//          Message = "Authentication required. Please log in.",
//          ErrorCode = "AUTH_NO_USER_CLAIM"
//        });
//        return;
//      }

//      // Step 2: Parse UserId
//      if (!int.TryParse(userIdClaim, out int userId) || userId <= 0)
//      {
//        logger?.LogWarning("Authorization failed: Invalid UserId format - {UserIdClaim}", userIdClaim);
//        context.Result = new UnauthorizedObjectResult(new
//        {
//          StatusCode = 401,
//          Message = "Invalid user identification. Please log in again.",
//          ErrorCode = "AUTH_INVALID_USER_ID"
//        });
//        return;
//      }

//      // Step 3: Try to get user from cache (fast path)
//      UsersDto? user = TryUserFromCache(memoryCache, userId, logger);

//      // Step 4: If cache miss, load from database (slow path)
//      if (user == null)
//      {
//        logger?.LogInformation("Cache miss for UserId: {UserId}. Loading from database.", userId);

//        user = LoadUserFromDatabase(serviceManager, userId, logger);

//        if (user == null)
//        {
//          logger?.LogWarning("Authorization failed: User not found in database - UserId: {UserId}", userId);
//          context.Result = new UnauthorizedObjectResult(new
//          {
//            StatusCode = 401,
//            Message = "User account not found. Please log in again.",
//            ErrorCode = "AUTH_USER_NOT_FOUND"
//          });
//          return;
//        }

//        // Step 5: Populate cache for future requests
//        CacheUser(memoryCache, userId, user, logger);
//      }

//      // Step 6: Sanitize sensitive data
//      user.Password = string.Empty;

//      // Step 7: Set user context in HttpContext for controllers
//      httpContext.SetCurrentUser(user);
//      httpContext.SetUserId(userId);

//      stopwatch.Stop();
//      logger?.LogDebug("Authorization successful for UserId: {UserId} in {ElapsedMs}ms",
//          userId, stopwatch.ElapsedMilliseconds);
//    }
//    catch (Exception ex)
//    {
//      stopwatch.Stop();
//      logger?.LogError(ex, "Authorization filter exception after {ElapsedMs}ms",
//          stopwatch.ElapsedMilliseconds);

//      //context.Result = new UnauthorizedObjectResult(new
//      //{
//      //  StatusCode = 401,
//      //  Message = "Authentication failed. Please log in again.",
//      //  ErrorCode = "AUTH_EXCEPTION",
//      //  Details = ex.Message // Remove in production
//      //});

//      // Only show details in development
//      var isDev = env?.IsDevelopment() ?? false;
//      context.Result = new UnauthorizedObjectResult(new
//      {
//        StatusCode = 401,
//        Message = isDev ? $"Authentication failed. {ex.Message}" : "Authentication failed. Please log in again.",
//        ErrorCode = "AUTH_EXCEPTION",
//        // Only include error details if development
//        Details = isDev ? ex.ToString() : null
//      });
//    }
//  }

//  /// <summary>
//  /// Attempts to retrieve user from memory cache
//  /// </summary>
//  private static UsersDto? TryUserFromCache(
//      IMemoryCache memoryCache,
//      int userId,
//      ILogger? logger)
//  {
//    try
//    {
//      var cacheKey = $"{USER_CACHE_PREFIX}{userId}";

//      if (memoryCache.TryGetValue(cacheKey, out UsersDto? cachedUser))
//      {
//        logger?.LogDebug("Cache hit for UserId: {UserId}", userId);
//        return cachedUser;
//      }

//      logger?.LogDebug("Cache miss for UserId: {UserId}", userId);
//      return null;
//    }
//    catch (Exception ex)
//    {
//      logger?.LogError(ex, "Error reading from cache for UserId: {UserId}", userId);
//      return null;
//    }
//  }

//  /// <summary>
//  /// Loads user from database
//  /// </summary>
//  private static UsersDto? LoadUserFromDatabase(
//      IServiceManager serviceManager,
//      int userId,
//      ILogger? logger)
//  {
//    try
//    {
//      var user = serviceManager.Users.User(userId, trackChanges: false);

//      if (user != null)
//      {
//        logger?.LogInformation("User loaded from database - UserId: {UserId}, UserName: {UserName}",
//            userId, user.UserName);
//      }

//      return user;
//    }
//    catch (Exception ex)
//    {
//      logger?.LogError(ex, "Error loading user from database - UserId: {UserId}", userId);
//      return null;
//    }
//  }

//  /// <summary>
//  /// Caches user data with configurable expiration
//  /// </summary>
//  private static void CacheUser(
//      IMemoryCache memoryCache,
//      int userId,
//      UsersDto user,
//      ILogger? logger)
//  {
//    try
//    {
//      var cacheKey = $"{USER_CACHE_PREFIX}{userId}";
//      var cacheOptions = new MemoryCacheEntryOptions()
//          .SetSlidingExpiration(CACHE_SLIDING_EXPIRATION)
//          .SetAbsoluteExpiration(CACHE_ABSOLUTE_EXPIRATION)
//          .SetPriority(CacheItemPriority.High)
//          .RegisterPostEvictionCallback((key, value, reason, state) =>
//          {
//            logger?.LogDebug("User cache evicted - Key: {CacheKey}, Reason: {Reason}",
//                      key, reason);
//          });

//      memoryCache.Set(cacheKey, user, cacheOptions);

//      logger?.LogDebug("User cached successfully - UserId: {UserId}", userId);
//    }
//    catch (Exception ex)
//    {
//      logger?.LogError(ex, "Error caching user - UserId: {UserId}", userId);
//      // Non-fatal: Continue without caching
//    }
//  }

//  /// <summary>
//  /// Manually clears user cache (useful for logout or user updates)
//  /// Call this from your service layer when user data changes
//  /// </summary>
//  public static void ClearUserCache(IMemoryCache memoryCache, int userId)
//  {
//    var cacheKey = $"{USER_CACHE_PREFIX}{userId}";
//    memoryCache.Remove(cacheKey);
//  }

//  /// <summary>
//  /// Manually refreshes user cache (useful after user updates)
//  /// </summary>
//  public static void RefreshUserCache(
//      IMemoryCache memoryCache,
//      IServiceManager serviceManager,
//      int userId,
//      ILogger? logger = null)
//  {
//    try
//    {
//      // Clear old cache
//      ClearUserCache(memoryCache, userId);

//      // Load fresh data
//      var user = serviceManager.Users.User(userId, trackChanges: false);

//      if (user != null)
//      {
//        CacheUser(memoryCache, userId, user, logger);
//        logger?.LogInformation("User cache refreshed - UserId: {UserId}", userId);
//      }
//    }
//    catch (Exception ex)
//    {
//      logger?.LogError(ex, "Error refreshing user cache - UserId: {UserId}", userId);
//    }
//  }
//}

//// [AllowAnonymous] use anywhere you want to allow anonymous access.
