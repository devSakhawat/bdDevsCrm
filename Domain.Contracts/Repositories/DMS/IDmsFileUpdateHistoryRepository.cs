using Domain.Contracts.Repositories.Common;
using Domain.Entities.Entities.DMS;

namespace Domain.Contracts.DMS;

public interface IDmsFileUpdateHistoryRepository : IRepositoryBase<DmsFileUpdateHistory>
{
    Task<DmsFileUpdateHistory?> FileUpdateHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DmsFileUpdateHistory>> FileUpdateHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<DmsFileUpdateHistory>> FileUpdateHistoriesByEntityAsync(string entityId, bool trackChanges, CancellationToken cancellationToken = default);
}
