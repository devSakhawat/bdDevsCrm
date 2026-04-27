using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmLeadStageService
{
  Task<CrmLeadStageDto> CreateAsync(CreateCrmLeadStageRecord record, CancellationToken cancellationToken = default);
  Task<CrmLeadStageDto> UpdateAsync(UpdateCrmLeadStageRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmLeadStageRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmLeadStageDto> LeadStageAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmLeadStageDto>> LeadStagesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmLeadStageDDLDto>> LeadStageForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmLeadStageDto>> LeadStageSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
