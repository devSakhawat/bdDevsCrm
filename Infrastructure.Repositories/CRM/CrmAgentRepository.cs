using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmAgent data access operations.</summary>
public class CrmAgentRepository : RepositoryBase<CrmAgent>, ICrmAgentRepository
{
    public CrmAgentRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmAgent records asynchronously.</summary>
    public async Task<IEnumerable<CrmAgent>> CrmAgentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.AgentId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmAgent record by ID asynchronously.</summary>
    public async Task<CrmAgent?> CrmAgentAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.AgentId.Equals(agentId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmAgent records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmAgent>> CrmAgentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.AgentId), trackChanges, cancellationToken);
}
