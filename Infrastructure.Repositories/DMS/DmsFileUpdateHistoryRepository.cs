using Domain.Contracts.DMS;
using Domain.Entities.Entities.DMS;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.DMS;

internal sealed class DmsFileUpdateHistoryRepository : RepositoryBase<DmsFileUpdateHistory>, IDmsFileUpdateHistoryRepository
{
    public DmsFileUpdateHistoryRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<DmsFileUpdateHistory?> FileUpdateHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(f => f.Id == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DmsFileUpdateHistory>> FileUpdateHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DmsFileUpdateHistory>> FileUpdateHistoriesByEntityAsync(string entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(f => f.EntityId == entityId, null, trackChanges, false, cancellationToken);
    }
}
