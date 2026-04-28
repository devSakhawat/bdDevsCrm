using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmStudentRepository : RepositoryBase<CrmStudent>, ICrmStudentRepository
{
    public CrmStudentRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmStudent>> CrmStudentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.StudentId, trackChanges, cancellationToken);

    public async Task<CrmStudent?> CrmStudentAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.StudentId.Equals(studentId), trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmStudent>> CrmStudentsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.BranchId == branchId, x => x.StudentId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmStudent>> CrmStudentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.StudentId), trackChanges, cancellationToken);
}
