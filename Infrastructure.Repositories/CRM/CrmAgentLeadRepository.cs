using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmAgentLeadRepository : RepositoryBase<CrmAgentLead>, ICrmAgentLeadRepository
{
    public CrmAgentLeadRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmAgentLead>> CrmAgentLeadsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.AgentLeadId, trackChanges, cancellationToken);

    public async Task<CrmAgentLead?> CrmAgentLeadAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.AgentLeadId == id, trackChanges, cancellationToken);

    public async Task<CrmAgentLead?> CrmAgentLeadByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.LeadId == leadId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmAgentLead>> CrmAgentLeadsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.AgentId == agentId, x => x.AgentLeadId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmAgentLead>> CrmAgentLeadsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.AgentLeadId), trackChanges, cancellationToken);
}
