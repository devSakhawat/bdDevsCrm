using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmVisaStatusService
{
  Task<CrmVisaStatusDto> CreateAsync(CreateCrmVisaStatusRecord record, CancellationToken cancellationToken = default);
  Task<CrmVisaStatusDto> UpdateAsync(UpdateCrmVisaStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmVisaStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmVisaStatusDto> VisaStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmVisaStatusDto>> VisaStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmVisaStatusDDLDto>> VisaStatusForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmVisaStatusDto>> VisaStatusSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
