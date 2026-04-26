using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmApplicationStatusService
{
  Task<CrmApplicationStatusDto> CreateAsync(CreateCrmApplicationStatusRecord record, CancellationToken cancellationToken = default);
  Task<CrmApplicationStatusDto> UpdateAsync(UpdateCrmApplicationStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmApplicationStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmApplicationStatusDto> ApplicationStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmApplicationStatusDto>> ApplicationStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmApplicationStatusDDLDto>> ApplicationStatusForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmApplicationStatusDto>> ApplicationStatusSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
