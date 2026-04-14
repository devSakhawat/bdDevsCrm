using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for group data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class GroupRepository : RepositoryBase<Groups>, IGroupRepository
{
	public GroupRepository(CrmContext context) : base(context) { }



	public async Task<Menu> CheckMenuPermission(string rawUrl, Users objUser, CancellationToken cancellationToken = default)
	{
		rawUrl ??= string.Empty;

		//string query = @"
		//  SELECT DISTINCT mnu.* ,mdl.ModuleName
		//  FROM GroupPermission gp
		//  INNER JOIN Menu   mnu ON mnu.MenuId   = gp.ReferenceId
		//  INNER JOIN Module mdl ON mdl.ModuleId = mnu.ModuleId
		//  INNER JOIN GroupMember gm ON gm.GroupId = gp.GroupId
		//  WHERE gp.PermissionTableName = 'Menu'
		//    AND gm.UserId = @UserId
		//    AND mnu.MenuPath LIKE @MenuPath";

		//var parameters = new SqlParameter[]
		//{
		//  new SqlParameter("@UserId",   objUser.UserId),
		//  new SqlParameter("@MenuPath", $"%{rawUrl}%"),
		//};

		string query = string.Format(@"
      SELECT DISTINCT mnu.* ,mdl.ModuleName
      FROM GroupPermission gp
      INNER JOIN Menu   mnu ON mnu.MenuId   = gp.ReferenceId
      INNER JOIN Module mdl ON mdl.ModuleId = mnu.ModuleId
      INNER JOIN GroupMember gm ON gm.GroupId = gp.GroupId
      WHERE gp.PermissionTableName = 'Menu'
        AND gm.UserId = {0}
        AND mnu.MenuPath LIKE '%{1}%'
        AND mnu.ParentMenu = 0
      ", objUser.UserId, rawUrl);

		//var parameters = new SqlParameter[]
		//{
		//  //new SqlParameter("@UserId",   objUser.UserId),
		//  //new SqlParameter("@MenuPath", $"%{rawUrl}%"),
		//};

		//Menu result = await ExecuteSingleData<Menu>(query);
		Menu result = await AdoExecuteSingleDataAsync<Menu>(query, cancellationToken: cancellationToken);
		return result;
	}

	/// <summary>
	/// Retrieves group permissions by group ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<GroupPermission>> GroupPermissionsByGroupIdAsync(int groupId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM GroupPermission WHERE GroupId = {groupId}";
		return await AdoExecuteListQueryAsync<GroupPermission>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves all access controls asynchronously.
	/// </summary>
	public async Task<IEnumerable<AccessControl>> AccessControlsAsync(CancellationToken cancellationToken = default)
	{
		string query = "SELECT * FROM AccessControl WHERE IsActive = 1";
		return await AdoExecuteListQueryAsync<AccessControl>(query, null, cancellationToken);
	}

	/// <summary>
	/// Checks menu permission for a specific user and URL asynchronously.
	/// </summary>
	public async Task<Menu> CheckMenuPermissionAsync(string rawUrl, Users objUser, CancellationToken cancellationToken = default)
	{
		rawUrl ??= string.Empty;
		string query = $@"
            SELECT DISTINCT mnu.*, mdl.ModuleName
            FROM GroupPermission gp
            INNER JOIN Menu mnu ON mnu.MenuId = gp.ReferenceId
            INNER JOIN Module mdl ON mdl.ModuleId = mnu.ModuleId
            INNER JOIN GroupMember gm ON gm.GroupId = gp.GroupId
            WHERE gp.PermissionTableName = 'Menu'
              AND gm.UserId = {objUser.UserId}
              AND mnu.MenuPath LIKE '%{rawUrl}%'
              AND mnu.ParentMenu = 0";

		return await AdoExecuteSingleDataAsync<Menu>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new group.
	/// </summary>
	public async Task<Groups> CreateGroupAsync(Groups group, CancellationToken cancellationToken = default)
	{
		int groupId = await CreateAndIdAsync(group, cancellationToken);
		group.GroupId = groupId;
		return group;
	}

	/// <summary>
	/// Updates an existing group.
	/// </summary>
	public void UpdateGroup(Groups group) => UpdateByState(group);

	/// <summary>
	/// Deletes a group.
	/// </summary>
	public async Task DeleteGroupAsync(Groups group, bool trackChanges, CancellationToken cancellationToken = default) 
=> await DeleteAsync(x => x.GroupId == group.GroupId, trackChanges, cancellationToken);
}
