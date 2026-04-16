using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class AppsTokenInfoRepository : RepositoryBase<AppsTokenInfo>, IAppsTokenInfoRepository
{
    public AppsTokenInfoRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<AppsTokenInfo?> AppsTokenInfoAsync(int appsTokenInfoId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.AppsTokenInfoId == appsTokenInfoId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<AppsTokenInfo>> AppsTokenInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
