using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmFollowUpRepository : RepositoryBase<CrmFollowUp>, ICrmFollowUpRepository
{
    public CrmFollowUpRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmFollowUp>> CrmFollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.FollowUpDate, trackChanges, cancellationToken);

    public async Task<CrmFollowUp?> CrmFollowUpAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.FollowUpId.Equals(followUpId), trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmFollowUp>> CrmFollowUpsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.LeadId == leadId, x => x.FollowUpDate, trackChanges, true, cancellationToken);

    public async Task<IEnumerable<CrmFollowUp>> CrmFollowUpsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.FollowUpId), trackChanges, cancellationToken);
}
