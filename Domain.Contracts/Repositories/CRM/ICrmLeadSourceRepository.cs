using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmLeadSourceRepository : IRepositoryBase<CrmLeadSource>
{
    /// <summary>
    /// Retrieves all CrmLeadSource records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmLeadSource>> CrmLeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmLeadSource record by ID asynchronously.
    /// </summary>
    Task<CrmLeadSource?> CrmLeadSourceAsync(int crmleadsourceid, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmLeadSource records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmLeadSource>> CrmLeadSourcesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
