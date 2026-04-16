using Domain.Contracts.DMS;
using Domain.Entities.Entities.DMS;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;

namespace Infrastructure.Repositories.DMS;

internal sealed class DmsFileUpdateHistoryRepository : RepositoryBase<DmsFileUpdateHistory>, IDmsFileUpdateHistoryRepository
{
    public DmsFileUpdateHistoryRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<DmsFileUpdateHistory?> FileUpdateHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(f => f.Id == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DmsFileUpdateHistory>> FileUpdateHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<DmsFileUpdateHistory>> FileUpdateHistoriesByEntityAsync(string entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByWhereAsync(f => f.EntityId == entityId, trackChanges, cancellationToken);
    }
}
