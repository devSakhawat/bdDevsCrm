using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IApproverOrderService
{
    Task<ApproverOrderDto> CreateAsync(CreateApproverOrderRecord record, CancellationToken cancellationToken = default);
    Task<ApproverOrderDto> UpdateAsync(UpdateApproverOrderRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteApproverOrderRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ApproverOrderDto> ApproverOrderAsync(int approverOrderId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverOrderDto>> ApproverOrdersAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverOrderDDLDto>> ApproverOrdersForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<ApproverOrderDto>> ApproverOrdersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
