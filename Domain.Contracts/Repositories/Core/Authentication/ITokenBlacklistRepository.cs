using Domain.Contracts.Repositories;
using Domain.Entities.Entities;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.Authentication;

public interface ITokenBlacklistRepository : IRepositoryBase<TokenBlacklist>
{
	//Task<TokenBlacklist> AddToBlacklistAsync(TokenBlacklist token, CancellationToken cancellationToken);

	//Task<bool> IsTokenBlacklistedAsync(string token, CancellationToken cancellationToken);

  //Task AddToBlacklistAsync(TokenBlacklist token, bool trackChanges, CancellationToken cancellationToken);
  //Task<bool> IsTokenBlacklistedAsync(string token, CancellationToken cancellationToken);


  Task<bool> IsBlacklistedAsync(
       string tokenHash,
       CancellationToken ct = default);

  Task AddAsync(
      TokenBlacklist token,
      CancellationToken ct = default);

  Task RemoveExpiredAsync(
      CancellationToken ct = default);

}