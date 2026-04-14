using Domain.Entities.Entities.System;
using Domain.Contracts.Core.Authentication;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Core.Authentication;

public class TokenBlacklistRepository : RepositoryBase<TokenBlacklist>, ITokenBlacklistRepository
{
	public TokenBlacklistRepository(CrmContext context) : base(context) { }

	public async Task<bool> IsBlacklistedAsync(string tokenHash, CancellationToken ct = default)
	{
		return await ExistsAsync(
			tb => tb.TokenHash == tokenHash && tb.ExpiryDate > DateTime.UtcNow,
			cancellationToken: ct);
	}

	public async Task AddAsync(TokenBlacklist token, CancellationToken ct = default)
	{
		await CreateAsync(token, ct);
	}

	public async Task RemoveExpiredAsync(CancellationToken ct = default)
	{
		await ExecuteDeleteAsync(
			tb => tb.ExpiryDate <= DateTime.UtcNow,
			ct);
	}
}
