using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IGroupService
{

	Task<MenuDto> CheckMenuPermissionAsync(string rawPath, UsersDto objUser, CancellationToken cancellationToken = default);
	/// <summary>
	/// Retrieves paginated summary grid of groups asynchronously.
	/// </summary>
	Task<GridEntity<GroupSummaryDto>> GroupSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves group permissions by group ID asynchronously.
	/// </summary>
	Task<IEnumerable<GroupPermissionDto>> GroupPermissionsByGroupIdAsync(int groupId, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves all access controls asynchronously.
	/// </summary>
	Task<IEnumerable<AccessControlDto>> AccessControlsAsync(CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new group asynchronously.
	/// </summary>
	Task<GroupDto> CreateAsync(GroupDto modelDto, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing group asynchronously.
	/// </summary>
	Task<GroupDto> UpdateAsync(int key, GroupDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a group by ID asynchronously.
	/// </summary>
	Task DeleteAsync(int key, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single Group record by its ID.
	/// </summary>
	Task<GroupDto> GroupAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves groups for dropdown list asynchronously.
	/// </summary>
	Task<IEnumerable<GroupDDLDto>> GroupsForDDLAsync(CancellationToken cancellationToken = default);




	//Task<GroupDto> CreateAsync(GroupDto modelDto);
	//Task<GroupDto> UpdateAsync(int key, GroupDto modelDto);

	//Task<GridEntity<GroupSummaryDto>> GroupSummary(bool trackChanges, GridOptions options);

	//Task<IEnumerable<GroupPermissionDto>> GroupPermisionsbyGroupId(int groupId);

	//Task<IEnumerable<AccessControlDto>> Accesses();
	//Task<IEnumerable<GroupPermissionDto>> AccessPermisionForCurrentUser(int moduleId, int userId);

	//// from user settings
	//Task<IEnumerable<GroupForUserSettings>> Groups(bool trackChanges);
	//Task<IEnumerable<GroupForUserSettings>> GroupsByUserId(int userId, bool trackChanges);
	//Task<IEnumerable<GroupMemberDto>> GroupMemberByUserId(int userId, bool trackChanges);

	//// get menu permission from controller.
	//Task<MenuDto> CheckMenuPermission(string rawPath, UsersDto objUser);
}
