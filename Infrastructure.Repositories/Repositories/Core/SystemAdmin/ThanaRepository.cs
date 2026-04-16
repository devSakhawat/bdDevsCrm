using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class ThanaRepository : RepositoryBase<Thana>, IThanaRepository
{
    public ThanaRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<Thana?> ThanaAsync(int thanaId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(t => t.ThanaId == thanaId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Thana>> ThanasAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Thana>> ThanasByDistrictIdAsync(int districtId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(t => t.DistrictId == districtId, null, trackChanges, false, cancellationToken);
    }

    public async Task<IEnumerable<Thana>> ActiveThanasAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(t => t.Status == 1, null, trackChanges, false, cancellationToken);
    }
}
