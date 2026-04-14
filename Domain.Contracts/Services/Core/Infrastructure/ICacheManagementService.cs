namespace Domain.Contracts.Services.Core.Infrastructure;

/// <summary>
/// Service for managing in-memory cache operations.
/// Provides centralized cache management for user sessions and application data.
/// </summary>
public interface ICacheManagementService
{
	/// <summary>
	/// Clears a specific cache entry by key.
	/// </summary>
	/// <param name="key">Cache key to remove</param>
	void ClearCacheEntry(string key);

	/// <summary>
	/// Clears all cache entries from memory.
	/// WARNING: Use sparingly - clears entire application cache.
	/// </summary>
	void ClearAllCache();

	/// <summary>
	/// Clears user-specific cache entry.
	/// </summary>
	/// <param name="userId">User ID to clear cache for</param>
	void ClearUserCache(int userId);
}