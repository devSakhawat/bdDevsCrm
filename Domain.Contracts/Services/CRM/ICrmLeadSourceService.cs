using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmLeadSourceService
{
  Task<CrmLeadSourceDto> CreateAsync(CreateCrmLeadSourceRecord record, CancellationToken cancellationToken = default);
  Task<CrmLeadSourceDto> UpdateAsync(UpdateCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(DeleteCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmLeadSourceDto> LeadSourceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmLeadSourceDto>> LeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmLeadSourceDDLDto>> LeadSourceForDDLAsync(CancellationToken cancellationToken = default);
  Task<GridEntity<CrmLeadSourceDto>> LeadSourceSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
