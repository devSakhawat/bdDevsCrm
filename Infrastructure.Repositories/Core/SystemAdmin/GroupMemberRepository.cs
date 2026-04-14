
// Class: GroupMemberRepository
using Domain.Entities.Entities.System;
using bdDevCRM.RepositoriesContracts.Core.SystemAdmin;
using bdDevCRM.s.Core.SystemAdmin;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

public class GroupMemberRepository : RepositoryBase<GroupMember>, IGroupMemberRepository
{
	public GroupMemberRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves group members by user ID using raw SQL.
	/// </summary>
	public async Task<IEnumerable<GroupMember>> GroupMembersByUserIdAsync(int userId, CancellationToken cancellationToken = default)
	{
		string query = string.Format("Select * from GroupMember where UserId = {0}", userId);
		return await AdoExecuteListQueryAsync<GroupMember>(query, null, cancellationToken);
	}


}



//using bdDevCRM.Entities.Entities;
//using Domain.Entities.Entities.System;
//using bdDevCRM.RepositoriesContracts.Core.SystemAdmin;
//using bdDevCRM.s.Core.SystemAdmin;
//using bdDevCRM.Sql.Context;

//namespace bdDevCRM.Repositories.Core.SystemAdmin;

//public class GroupMemberRepository : RepositoryBase<GroupMember>, IGroupMemberRepository
//{
//  private const string SELECT_GROUPMEMBER_BY_USERID = "Select * from GroupMember where UserId = {0}";

//  public GroupMemberRepository(CRMContext context) : base(context) { }

//  // Group Member by UserId for user settings.
//  public async Task<IEnumerable<GroupMember>> GroupMemberByUserId(int userId, bool trackChanges)
//  {
//    string groupMemberQuery = string.Format(SELECT_GROUPMEMBER_BY_USERID, userId);
//    IEnumerable<GroupMember> groupMembersByUser = await ExecuteListQuery<GroupMember>(groupMemberQuery);
//    return groupMembersByUser.ToList();
//  }

//  //public async Task<IEnumerable<GroupMemberPermission>> GroupMemberPermisionsbyGroupMemberId(int GroupMemberId)
//  //{
//  //  string query = string.Format(SELECT_GroupMemberPERMISSION_BY_GroupMemberID, GroupMemberId);
//  //  IEnumerable<GroupMemberPermission> GroupMemberPermissions = await ExecuteListQuery<GroupMemberPermission>(query);
//  //  return GroupMemberPermissions.AsQueryable();
//  //}

//  //public async Task<IEnumerable<AccessControl>> Accesses()
//  //{
//  //  string query = string.Format(SELECT_ALL_ACCESS_CONTROL);
//  //  IEnumerable<AccessControl> GroupMemberPermissions = await ExecuteListQuery<AccessControl>(query);
//  //  return GroupMemberPermissions;
//  //}







//}
