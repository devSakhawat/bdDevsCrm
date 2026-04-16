using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IApproverTypeService
{
    Task<ApproverTypeDto> CreateAsync(CreateApproverTypeRecord record, CancellationToken cancellationToken = default);
    Task<ApproverTypeDto> UpdateAsync(UpdateApproverTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteApproverTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ApproverTypeDto> ApproverTypeAsync(int approverTypeId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverTypeDto>> ApproverTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverTypeDDLDto>> ApproverTypesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<ApproverTypeDto>> ApproverTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
