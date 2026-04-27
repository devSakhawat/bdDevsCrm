using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmStudentDocumentChecklistService
{
    Task<CrmStudentDocumentChecklistDto> CreateAsync(CreateCrmStudentDocumentChecklistRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentDocumentChecklistDto> UpdateAsync(UpdateCrmStudentDocumentChecklistRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmStudentDocumentChecklistRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentDocumentChecklistDto> StudentDocumentChecklistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDocumentChecklistDto>> StudentDocumentChecklistsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDocumentChecklistDto>> StudentDocumentChecklistsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmStudentDocumentChecklistDto>> StudentDocumentChecklistsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
