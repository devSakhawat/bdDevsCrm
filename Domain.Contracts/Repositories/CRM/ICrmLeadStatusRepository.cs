using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmLeadStatusRepository : IRepositoryBase<CrmLeadStatus>
{
    /// <summary>
    /// Retrieves all CrmLeadStatus records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmLeadStatus>> CrmLeadStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmLeadStatus record by ID asynchronously.
    /// </summary>
    Task<CrmLeadStatus?> CrmLeadStatusAsync(int crmleadstatusid, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmLeadStatus records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmLeadStatus>> CrmLeadStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
