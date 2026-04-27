using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmOfficeRepository : IRepositoryBase<CrmOffice>
{
    /// <summary>
    /// Retrieves all CrmOffice records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmOffice>> CrmOfficesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmOffice record by ID asynchronously.
    /// </summary>
    Task<CrmOffice?> CrmOfficeAsync(int crmofficeid, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmOffice records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmOffice>> CrmOfficesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
