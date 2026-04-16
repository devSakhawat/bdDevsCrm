using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class ThanaRepository : RepositoryBase<Thana>, IThanaRepository
{
    public ThanaRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<Thana?> ThanaAsync(int thanaId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(t => t.ThanaId == thanaId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Thana>> ThanasAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Thana>> ThanasByDistrictIdAsync(int districtId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByWhereAsync(t => t.DistrictId == districtId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<Thana>> ActiveThanasAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByWhereAsync(t => t.Status == 1, trackChanges, cancellationToken);
    }
}
