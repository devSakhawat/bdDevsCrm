using Domain.Contracts.Services.Core.Infrastructure;
﻿using Application.Services.Core.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using System.Collections;
using System.Reflection;

namespace Application.Services.Core.Infrastructure;

/// <summary>
/// Concrete implementation of cache management service.
/// Provides centralized cache operations for the application.
/// </summary>
public sealed class CacheManagementService : ICacheManagementService
{
	private readonly IMemoryCache _memoryCache;

	public CacheManagementService(IMemoryCache memoryCache)
	{
		_memoryCache = memoryCache;
	}

	public void ClearCacheEntry(string key)
	{
		if (string.IsNullOrWhiteSpace(key))
			return;

		_memoryCache.Remove(key);
	}

	public void ClearAllCache()
	{
		var memCache = _memoryCache as MemoryCache;
		if (memCache == null)
			return;

		var coherentState = typeof(MemoryCache).GetProperty("CoherentState",
			BindingFlags.NonPublic | BindingFlags.Instance);

		var coherentStateValue = coherentState?.GetValue(memCache);
		if (coherentStateValue == null)
			return;

		var entriesCollection = coherentStateValue.GetType()
			.GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);

		var cacheItems = entriesCollection?.GetValue(coherentStateValue) as IDictionary;
		if (cacheItems == null)
			return;

		foreach (var key in cacheItems.Keys.Cast<object>().ToList())
		{
			_memoryCache.Remove(key);
		}
	}

	public void ClearUserCache(int userId)
	{
		if (userId <= 0)
			return;

		var cacheKey = $"User_{userId}";
		_memoryCache.Remove(cacheKey);
	}
}

