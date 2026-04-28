using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmDocumentVerificationHistoryService
{
    Task<CrmDocumentVerificationHistoryDto> CreateAsync(CreateCrmDocumentVerificationHistoryRecord record, CancellationToken cancellationToken = default);
    Task<CrmDocumentVerificationHistoryDto> UpdateAsync(UpdateCrmDocumentVerificationHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmDocumentVerificationHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmDocumentVerificationHistoryDto> DocumentVerificationHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmDocumentVerificationHistoryDto>> DocumentVerificationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmDocumentVerificationHistoryDto>> DocumentVerificationHistoriesByDocumentIdAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmDocumentVerificationHistoryDto>> DocumentVerificationHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
