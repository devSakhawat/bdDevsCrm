using Domain.Entities.Entities.System;
using Domain.Contracts.Core.Authentication;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.Authentication;

public class TokenBlacklistRepository : RepositoryBase<TokenBlacklist>, ITokenBlacklistRepository
{
	public TokenBlacklistRepository(CRMContext context) : base(context) { }

	public async Task<TokenBlacklist> AddToBlacklistAsync(TokenBlacklist token, CancellationToken cancellationToken)
	{
		await CreateAsync(token, cancellationToken);
		return token;
	}

	public async Task<bool> IsTokenBlacklistedAsync(string token, CancellationToken cancellationToken)
	{
		return await ExistsAsync(tb => tb.Token == token && tb.ExpiryDate > DateTime.UtcNow, cancellationToken: cancellationToken);
	}
}
