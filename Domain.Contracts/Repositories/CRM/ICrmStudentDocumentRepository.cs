using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentDocumentRepository : IRepositoryBase<CrmStudentDocument>
{
    Task<IEnumerable<CrmStudentDocument>> StudentDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentDocument?> StudentDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDocument>> StudentDocumentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
}
