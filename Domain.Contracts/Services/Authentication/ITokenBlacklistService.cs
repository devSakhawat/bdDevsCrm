using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Authentication;

public interface ITokenBlacklistService
{
  // CRUD Records Pattern Methods
  Task<TokenBlacklistDto> CreateAsync(CreateTokenBlacklistRecord record, CancellationToken cancellationToken = default);
  Task<TokenBlacklistDto> UpdateAsync(UpdateTokenBlacklistRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteTokenBlacklistRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<TokenBlacklistDto> TokenBlacklistAsync(Guid tokenId, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<TokenBlacklistDto>> TokenBlacklistsAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<GridEntity<TokenBlacklistDto>> TokenBlacklistsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);

  // Authentication-specific Methods
  /// <summary>
  /// Token blacklist-এ আছে কিনা check করে।
  /// Logout বা manual revoke হলে true return করে।
  /// </summary>
  Task<bool> IsTokenBlacklistedAsync(string token, CancellationToken ct = default);

  /// <summary>
  /// Token blacklist-এ যোগ করো (logout-এ call করো)
  /// </summary>
  Task BlacklistTokenAsync(string token, DateTime expiry, CancellationToken ct = default);

  /// <summary>
  /// Expired blacklisted tokens পরিষ্কার করো
  /// TokenCleanupBackgroundService এটা call করে
  /// </summary>
  Task RemoveExpiredTokensAsync(CancellationToken ct = default);
}
