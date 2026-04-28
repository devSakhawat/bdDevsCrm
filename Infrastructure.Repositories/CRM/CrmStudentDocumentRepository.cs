using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmStudentDocumentRepository : RepositoryBase<CrmStudentDocument>, ICrmStudentDocumentRepository
{
    public CrmStudentDocumentRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmStudentDocument>> StudentDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.StudentDocumentId, trackChanges, cancellationToken);

    public async Task<CrmStudentDocument?> StudentDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.StudentDocumentId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmStudentDocument>> StudentDocumentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.StudentId == studentId, x => x.StudentDocumentId, trackChanges, false, cancellationToken);
}
