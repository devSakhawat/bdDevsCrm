using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IApproverTypeToGroupMappingService
{
    Task<ApproverTypeToGroupMappingDto> CreateAsync(CreateApproverTypeToGroupMappingRecord record, CancellationToken cancellationToken = default);
    Task<ApproverTypeToGroupMappingDto> UpdateAsync(UpdateApproverTypeToGroupMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteApproverTypeToGroupMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ApproverTypeToGroupMappingDto> ApproverTypeToGroupMappingAsync(int approverTypeMapId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverTypeToGroupMappingDto>> ApproverTypeToGroupMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<ApproverTypeToGroupMappingDDLDto>> ApproverTypeToGroupMappingsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<ApproverTypeToGroupMappingDto>> ApproverTypeToGroupMappingsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
