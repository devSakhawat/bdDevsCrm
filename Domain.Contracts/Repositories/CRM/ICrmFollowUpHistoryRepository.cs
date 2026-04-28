using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmFollowUpHistoryRepository : IRepositoryBase<CrmFollowUpHistory>
{
    Task<IEnumerable<CrmFollowUpHistory>> FollowUpHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmFollowUpHistory?> CrmFollowUpHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUpHistory>> FollowUpHistoriesByFollowUpIdAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUpHistory>> FollowUpHistoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
