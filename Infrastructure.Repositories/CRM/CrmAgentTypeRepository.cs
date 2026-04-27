using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmAgentType data access operations.
/// </summary>
public class CrmAgentTypeRepository : RepositoryBase<CrmAgentType>, ICrmAgentTypeRepository
{
    public CrmAgentTypeRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmAgentType records asynchronously.</summary>
    public async Task<IEnumerable<CrmAgentType>> CrmAgentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.AgentTypeId, trackChanges, cancellationToken);
    }

    /// <summary>Retrieves a single CrmAgentType record by ID asynchronously.</summary>
    public async Task<CrmAgentType?> CrmAgentTypeAsync(int crmagenttypeid, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(
            x => x.AgentTypeId.Equals(crmagenttypeid),
            trackChanges,
            cancellationToken);
    }

    /// <summary>Retrieves CrmAgentType records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmAgentType>> CrmAgentTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.AgentTypeId), trackChanges, cancellationToken);
    }
}
