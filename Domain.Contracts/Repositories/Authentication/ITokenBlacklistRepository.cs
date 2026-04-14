using Domain.Entities.Entities.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Contracts.Repositories.Authentication;

public interface ITokenBlacklistRepository : IRepositoryBase<TokenBlacklist>
{
  Task<bool> IsBlacklistedAsync(
      string tokenHash,
      CancellationToken ct = default);

  Task AddAsync(
      TokenBlacklist token,
      CancellationToken ct = default);

  Task RemoveExpiredAsync(
      CancellationToken ct = default);
}
