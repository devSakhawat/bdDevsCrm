using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentStatusHistoryRepository : IRepositoryBase<CrmStudentStatusHistory>
{
    Task<IEnumerable<CrmStudentStatusHistory>> StudentStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentStatusHistory?> CrmStudentStatusHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentStatusHistory>> StudentStatusHistoriesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentStatusHistory>> StudentStatusHistoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
