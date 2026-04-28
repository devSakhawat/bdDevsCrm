using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmStudentDocumentChecklistRepository : IRepositoryBase<CrmStudentDocumentChecklist>
{
    Task<IEnumerable<CrmStudentDocumentChecklist>> StudentDocumentChecklistsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentDocumentChecklist?> StudentDocumentChecklistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDocumentChecklist>> StudentDocumentChecklistsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
}
