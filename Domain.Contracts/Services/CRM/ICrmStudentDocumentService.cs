using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Microsoft.AspNetCore.Http;

namespace Domain.Contracts.Services.CRM;

public interface ICrmStudentDocumentService
{
    Task<CrmStudentDocumentDto> CreateAsync(CreateCrmStudentDocumentRecord record, CancellationToken cancellationToken = default);
    Task<CrmStudentDocumentDto> UpdateAsync(UpdateCrmStudentDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmStudentDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmStudentDocumentDto> StudentDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDocumentDto>> StudentDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmStudentDocumentDto>> StudentDocumentsByStudentIdAsync(int studentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmStudentDocumentDto>> StudentDocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmStudentDocumentDto> UploadAsync(StudentDocumentUploadRequestDto request, IFormFile file, CancellationToken cancellationToken = default);
    Task<CrmStudentDocumentDto> ChangeStatusAsync(ChangeCrmStudentDocumentStatusRecord record, CancellationToken cancellationToken = default);
    Task<int> EscalateRejectedDocumentsAsync(CancellationToken cancellationToken = default);
}
