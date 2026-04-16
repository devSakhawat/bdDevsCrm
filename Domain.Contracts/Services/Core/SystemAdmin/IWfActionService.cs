using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IWfActionService
{
    Task<WfActionDto> CreateAsync(CreateWfActionRecord record, CancellationToken cancellationToken = default);
    Task<WfActionDto> UpdateAsync(UpdateWfActionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteWfActionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<WfActionDto> WfActionAsync(int wfActionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<WfActionDto>> WfActionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<WfActionDto>> WfActionsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<WfActionDto>> WfActionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
