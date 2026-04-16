using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class MaritalStatusRepository : RepositoryBase<MaritalStatus>, IMaritalStatusRepository
{
    public MaritalStatusRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<MaritalStatus?> MaritalStatusAsync(int maritalStatusId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(m => m.MaritalStatusId == maritalStatusId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<MaritalStatus>> MaritalStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<MaritalStatus>> ActiveMaritalStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(m => m.IsActive == 1, null, trackChanges, false, cancellationToken);
    }
}
