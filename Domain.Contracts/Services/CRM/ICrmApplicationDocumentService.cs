using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmApplicationDocumentService
{
    Task<CrmApplicationDocumentDto> CreateAsync(CreateCrmApplicationDocumentRecord record, CancellationToken cancellationToken = default);
    Task<CrmApplicationDocumentDto> UpdateAsync(UpdateCrmApplicationDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmApplicationDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmApplicationDocumentDto> ApplicationDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationDocumentDto>> ApplicationDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationDocumentDto>> ApplicationDocumentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmApplicationDocumentDto>> ApplicationDocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
