using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAssignApproverService
{
    Task<AssignApproverDto> CreateAsync(CreateAssignApproverRecord record, CancellationToken cancellationToken = default);
    Task<AssignApproverDto> UpdateAsync(UpdateAssignApproverRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAssignApproverRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AssignApproverDto> AssignApproverAsync(int assignApproverId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssignApproverDto>> AssignApproversAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AssignApproverDDLDto>> AssignApproversForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AssignApproverDto>> AssignApproversSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
