using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IApproverDetailsService
{
    Task<ApproverDetailsDto> CreateAsync(CreateApproverDetailsRecord record, CancellationToken cancellationToken = default);
    Task<ApproverDetailsDto> UpdateAsync(UpdateApproverDetailsRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteApproverDetailsRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ApproverDetailsDto> ApproverDetailAsync(int remarksId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverDetailsDto>> ApproverDetailsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverDetailsDDLDto>> ApproverDetailsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<ApproverDetailsDto>> ApproverDetailsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
