using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentRepository : IRepositoryBase<CrmStudent>
{
    /// <summary>Retrieves all CrmStudent records asynchronously.</summary>
    Task<IEnumerable<CrmStudent>> CrmStudentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmStudent record by ID asynchronously.</summary>
    Task<CrmStudent?> CrmStudentAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmStudent records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmStudent>> CrmStudentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}