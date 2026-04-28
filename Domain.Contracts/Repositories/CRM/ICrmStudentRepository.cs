using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentRepository : IRepositoryBase<CrmStudent>
{
    Task<IEnumerable<CrmStudent>> CrmStudentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudent?> CrmStudentAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudent>> CrmStudentsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudent>> CrmStudentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
