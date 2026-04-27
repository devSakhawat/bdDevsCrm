using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentStatusRepository : IRepositoryBase<CrmStudentStatus>
{
    /// <summary>
    /// Retrieves all CrmStudentStatus records asynchronously.
    /// </summary>
    Task<IEnumerable<CrmStudentStatus>> CrmStudentStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves a single CrmStudentStatus record by ID asynchronously.
    /// </summary>
    Task<CrmStudentStatus?> CrmStudentStatusAsync(int crmstudentstatusid, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>
    /// Retrieves CrmStudentStatus records by a collection of IDs asynchronously.
    /// </summary>
    Task<IEnumerable<CrmStudentStatus>> CrmStudentStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
