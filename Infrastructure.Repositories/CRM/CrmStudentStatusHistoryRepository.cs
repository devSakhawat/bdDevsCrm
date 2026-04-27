using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmStudentStatusHistoryRepository : RepositoryBase<CrmStudentStatusHistory>, ICrmStudentStatusHistoryRepository
{
    public CrmStudentStatusHistoryRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmStudentStatusHistory>> StudentStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.StudentStatusHistoryId, trackChanges, cancellationToken);

    public async Task<CrmStudentStatusHistory?> CrmStudentStatusHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.StudentStatusHistoryId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmStudentStatusHistory>> StudentStatusHistoriesByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.StudentId == studentId, x => x.StudentStatusHistoryId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmStudentStatusHistory>> StudentStatusHistoriesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.StudentStatusHistoryId), trackChanges, cancellationToken);
}
