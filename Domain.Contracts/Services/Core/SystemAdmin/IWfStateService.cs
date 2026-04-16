using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IWfStateService
{
    Task<WfStateDto> CreateAsync(CreateWfStateRecord record, CancellationToken cancellationToken = default);
    Task<WfStateDto> UpdateAsync(UpdateWfStateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteWfStateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<WfStateDto> WfStateAsync(int wfStateId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<WfStateDto>> WfStatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<WfStateDto>> WfStatesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<WfStateDto>> WfStatesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
