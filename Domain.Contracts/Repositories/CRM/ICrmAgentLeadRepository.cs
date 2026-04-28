using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmAgentLeadRepository : IRepositoryBase<CrmAgentLead>
{
    Task<IEnumerable<CrmAgentLead>> CrmAgentLeadsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmAgentLead?> CrmAgentLeadAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmAgentLead?> CrmAgentLeadByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgentLead>> CrmAgentLeadsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgentLead>> CrmAgentLeadsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
