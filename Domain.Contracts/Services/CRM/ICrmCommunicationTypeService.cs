using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCommunicationTypeService
{
  Task<CrmCommunicationTypeDto> CreateAsync(CreateCrmCommunicationTypeRecord record, CancellationToken cancellationToken = default);
  Task<CrmCommunicationTypeDto> UpdateAsync(UpdateCrmCommunicationTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmCommunicationTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmCommunicationTypeDto> CommunicationTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmCommunicationTypeDto>> CommunicationTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmCommunicationTypeDDLDto>> CommunicationTypeForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmCommunicationTypeDto>> CommunicationTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
