using Domain.Entities.Entities.System;
using Domain.Entities.Entities.Token;
using Domain.Contracts.Core.Authentication;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.Authentication;

public class RefreshTokenRepository : RepositoryBase<RefreshToken>, IRefreshTokenRepository
{
  public RefreshTokenRepository(CrmContext context) : base(context) { }

  public async Task<IEnumerable<RefreshToken>> ExpiredTokensAsync(CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(rt => rt.ExpiryDate < DateTime.UtcNow || rt.IsRevoked, trackChanges: false, cancellationToken: cancellationToken);
  }


  public async Task RevokeAllTokensByUserIdAsync(int userId, CancellationToken cancellationToken = default)
  {
    var activeTokens = await ActiveTokensByUserIdAsync(userId, trackChanges: true, cancellationToken: cancellationToken);

    if (activeTokens == null || !activeTokens.Any())
      return;

    foreach (var token in activeTokens)
    {
      token.IsRevoked = true;
      token.RevokedDate = DateTime.UtcNow;
    }
  }


  /// <summary>
  ///  all active tokens by user ID
  /// </summary>
  public async Task<IEnumerable<RefreshToken>> ActiveTokensByUserIdAsync(int userId, bool trackChanges, CancellationToken cancellationToken )
  {
    return await ListByConditionAsync(rt => rt.UserId == userId && rt.ExpiryDate > DateTime.UtcNow && !rt.IsRevoked, orderBy: x => x.CreatedDate, trackChanges: trackChanges, cancellationToken: cancellationToken);
  }

}
