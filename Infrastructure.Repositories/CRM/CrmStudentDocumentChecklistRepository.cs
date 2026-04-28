using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmStudentDocumentChecklistRepository : RepositoryBase<CrmStudentDocumentChecklist>, ICrmStudentDocumentChecklistRepository
{
    public CrmStudentDocumentChecklistRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmStudentDocumentChecklist>> StudentDocumentChecklistsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.StudentDocumentChecklistId, trackChanges, cancellationToken);

    public async Task<CrmStudentDocumentChecklist?> StudentDocumentChecklistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.StudentDocumentChecklistId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmStudentDocumentChecklist>> StudentDocumentChecklistsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.StudentId == studentId, x => x.StudentDocumentChecklistId, trackChanges, false, cancellationToken);
}
