using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmVisaTypeRepository : IRepositoryBase<CrmVisaType>
{
    /// <summary>
    /// Retrieves all CrmVisaType records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmVisaType>> CrmVisaTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmVisaType record by ID asynchronously.
    /// </summary>
    Task<CrmVisaType?> CrmVisaTypeAsync(int crmvisatypeid, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmVisaType records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmVisaType>> CrmVisaTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
