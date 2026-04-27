using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmAgentTypeRepository : IRepositoryBase<CrmAgentType>
{
    /// <summary>
    /// Retrieves all CrmAgentType records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmAgentType>> CrmAgentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmAgentType record by ID asynchronously.
    /// </summary>
    Task<CrmAgentType?> CrmAgentTypeAsync(int crmagenttypeid, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmAgentType records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmAgentType>> CrmAgentTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
