using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IApproverHistoryService
{
    Task<ApproverHistoryDto> CreateAsync(CreateApproverHistoryRecord record, CancellationToken cancellationToken = default);
    Task<ApproverHistoryDto> UpdateAsync(UpdateApproverHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteApproverHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ApproverHistoryDto> ApproverHistoryAsync(int assignApproverId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverHistoryDto>> ApproverHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverHistoryDDLDto>> ApproverHistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<ApproverHistoryDto>> ApproverHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
