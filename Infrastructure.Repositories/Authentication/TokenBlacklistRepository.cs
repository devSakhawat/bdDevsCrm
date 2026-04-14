using Domain.Contracts.Core.Authentication;
using Domain.Entities.Entities.System;
using Infrastructure.Sql.Context;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Repositories.Authentication;

public class TokenBlacklistRepository
    : RepositoryBase<TokenBlacklist>, ITokenBlacklistRepository
{
  public TokenBlacklistRepository(CrmContext context)
      : base(context) { }

  public async Task<bool> IsBlacklistedAsync(
      string tokenHash,
      CancellationToken ct = default)
  {
    return await ExistsAsync(
        t => t.Token == tokenHash
          && t.ExpiryDate > DateTime.UtcNow,
        ct);
  }

  public async Task AddAsync(
      TokenBlacklist token,
      CancellationToken ct = default)
  {
    await CreateAsync(token, ct);
  }

  public async Task RemoveExpiredAsync(
      CancellationToken ct = default)
  {
    await ExecuteDeleteAsync(
        t => t.ExpiryDate <= DateTime.UtcNow,
        ct);
  }
}
