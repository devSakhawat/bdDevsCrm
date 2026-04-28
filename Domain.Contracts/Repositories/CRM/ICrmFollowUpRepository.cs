using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmFollowUpRepository : IRepositoryBase<CrmFollowUp>
{
    Task<IEnumerable<CrmFollowUp>> CrmFollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmFollowUp?> CrmFollowUpAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUp>> CrmFollowUpsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUp>> CrmFollowUpsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
