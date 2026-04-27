using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmFollowUpHistoryRepository : RepositoryBase<CrmFollowUpHistory>, ICrmFollowUpHistoryRepository
{
    public CrmFollowUpHistoryRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmFollowUpHistory>> FollowUpHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.FollowUpHistoryId, trackChanges, cancellationToken);

    public async Task<CrmFollowUpHistory?> CrmFollowUpHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.FollowUpHistoryId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmFollowUpHistory>> FollowUpHistoriesByFollowUpIdAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.FollowUpId == followUpId, x => x.FollowUpHistoryId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmFollowUpHistory>> FollowUpHistoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.FollowUpHistoryId), trackChanges, cancellationToken);
}
