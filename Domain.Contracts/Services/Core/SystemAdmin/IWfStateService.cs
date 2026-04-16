using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IWfStateService
{
    Task<WfstateDto> CreateAsync(CreateWfStateRecord record, CancellationToken cancellationToken = default);
    Task<WfstateDto> UpdateAsync(UpdateWfStateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteWfStateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<WfstateDto> WfStateAsync(int wfStateId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<WfstateDto>> WfStatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<WfstateDto>> WfStatesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<WfstateDto>> WfStatesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
