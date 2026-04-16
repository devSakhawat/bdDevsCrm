using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IGroupMemberService
{
    Task<GroupMemberDto> CreateAsync(CreateGroupMemberRecord record, CancellationToken cancellationToken = default);
    Task<GroupMemberDto> UpdateAsync(UpdateGroupMemberRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteGroupMemberRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GroupMemberDto> GroupMemberAsync(int groupId, int userId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupMemberDto>> GroupMembersAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<GroupMemberDto>> GroupMembersForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<GroupMemberDto>> GroupMembersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
