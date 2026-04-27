using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmLeadRepository : IRepositoryBase<CrmLead>
{
    /// <summary>Retrieves all CrmLead records asynchronously.</summary>
    Task<IEnumerable<CrmLead>> CrmLeadsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmLead record by ID asynchronously.</summary>
    Task<CrmLead?> CrmLeadAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmLead records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmLead>> CrmLeadsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
