using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmApplicationConditionService
{
    Task<CrmApplicationConditionDto> CreateAsync(CreateCrmApplicationConditionRecord record, CancellationToken cancellationToken = default);
    Task<CrmApplicationConditionDto> UpdateAsync(UpdateCrmApplicationConditionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmApplicationConditionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmApplicationConditionDto> ApplicationConditionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationConditionDto>> ApplicationConditionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationConditionDto>> ApplicationConditionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmApplicationConditionDto>> ApplicationConditionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
    Task<CrmApplicationConditionDto> ChangeStatusAsync(ChangeCrmApplicationConditionStatusRecord record, CancellationToken cancellationToken = default);
}
