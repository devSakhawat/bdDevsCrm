using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmDocumentTypeService
{
  Task<CrmDocumentTypeDto> CreateAsync(CreateCrmDocumentTypeRecord record, CancellationToken cancellationToken = default);
  Task<CrmDocumentTypeDto> UpdateAsync(UpdateCrmDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmDocumentTypeDto> DocumentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmDocumentTypeDto>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmDocumentTypeDDLDto>> DocumentTypeForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmDocumentTypeDto>> DocumentTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
