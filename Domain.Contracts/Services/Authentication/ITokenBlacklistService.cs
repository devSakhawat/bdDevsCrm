using Domain.Entities.Entities.System;

namespace Domain.Contracts.Services.Authentication;

public interface ITokenBlacklistService
{
  //Task<TokenBlacklist> AddToBlacklistAsync(string token, bool trackChanges, CancellationToken cancellationToken = default);
  //Task<bool> IsTokenBlacklisted(string token, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// Token blacklist-এ আছে কিনা check করে।
  /// Logout বা manual revoke হলে true return করে।
  /// </summary>
  Task<bool> IsTokenBlacklistedAsync(
      string token,
      CancellationToken ct = default);

  /// <summary>
  /// Token blacklist-এ যোগ করো (logout-এ call করো)
  /// </summary>
  Task BlacklistTokenAsync(
      string token,
      DateTime expiry,
      CancellationToken ct = default);

  /// <summary>
  /// Expired blacklisted tokens পরিষ্কার করো
  /// TokenCleanupBackgroundService এটা call করে
  /// </summary>
  Task RemoveExpiredTokensAsync(
      CancellationToken ct = default);

}
