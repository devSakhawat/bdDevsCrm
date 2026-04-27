using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmAgentRepository : IRepositoryBase<CrmAgent>
{
    /// <summary>Retrieves all CrmAgent records asynchronously.</summary>
    Task<IEnumerable<CrmAgent>> CrmAgentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmAgent record by ID asynchronously.</summary>
    Task<CrmAgent?> CrmAgentAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmAgent records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmAgent>> CrmAgentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
