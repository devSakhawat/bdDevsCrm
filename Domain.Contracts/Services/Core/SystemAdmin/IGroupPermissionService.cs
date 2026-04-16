using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IGroupPermissionService
{
    Task<GroupPermissionDto> CreateAsync(CreateGroupPermissionRecord record, CancellationToken cancellationToken = default);
    Task<GroupPermissionDto> UpdateAsync(UpdateGroupPermissionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteGroupPermissionRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GroupPermissionDto> GroupPermissionAsync(int permissionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupPermissionDto>> GroupPermissionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupPermissionDto>> GroupPermissionsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<GroupPermissionDto>> GroupPermissionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
