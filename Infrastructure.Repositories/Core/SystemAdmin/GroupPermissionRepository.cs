

// Class: GroupPermissionRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin
{
	public class GroupPermissionRepository : RepositoryBase<GroupPermission>, IGroupPermissionRepository
	{
		public GroupPermissionRepository(CRMContext context) : base(context) { }

		/// <summary>
		/// Retrieves access permissions for the current user based on module and delegation logic.
		/// </summary>
		public async Task<IEnumerable<GroupPermission>> AccessPermissionsForCurrentUserAsync(int moduleId, int userId, CancellationToken cancellationToken = default)
		{
			string sql = string.Format(@"
SELECT DISTINCT *
FROM GroupPermission gp
WHERE gp.PermissionTableName = 'Access'
  AND gp.ParentPermission = {0}
  AND gp.GroupId IN (
      SELECT gm.GroupId
      FROM GroupMember gm
      WHERE gm.UserId = {1}
      UNION
      SELECT gm2.GroupId
      FROM GroupMember gm2
      WHERE gm2.UserId IN (
          SELECT DISTINCT u.UserId
          FROM DeligationInfo di
          INNER JOIN Users u ON u.EmployeeId = di.HrRecordId
          INNER JOIN Users dg ON dg.EmployeeId = di.DeligatedHrRecordId
          WHERE dg.UserId = {1}
            AND '{2}' BETWEEN di.FromDate AND di.ToDate
            AND di.IsActive = 1
      )
  )", moduleId, userId, DateTime.Now.ToString("MM/dd/yyyy"));

			return await AdoExecuteListQueryAsync<GroupPermission>(sql, null, cancellationToken);
		}
	}
}



//using Domain.Entities.Entities.System;
//using Domain.Contracts.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Infrastructure.Sql.Context;

//namespace Infrastructure.Repositories.Core.SystemAdmin;

//public class GroupPermissionRepository : RepositoryBase<GroupPermission>, IGroupPermissionRepository
//{
//  private const string SELECT_ACCESSPERMISSION_BYMODULE_AND_USER =
//            @"Select distinct * 
//from GroupPermission
//where  PermissionTableName = 'Access' and ParentPermission = {0} and
//GroupId in (Select GroupId from GroupMember where UserId = {1}
//union
//Select GroupId 
//from GroupMember where UserId in (
//Select distinct Users.UserId from DeligationInfo  
//inner join Users on Users.EmployeeId = DeligationInfo.HrRecordId  
//inner join Users Dg on Dg.EmployeeId = DeligationInfo.DeligatedHrRecordId 
//where Dg.UserId = {1} and '{2}' between FromDate and ToDate and DeligationInfo.IsActive = 1))";

//  public GroupPermissionRepository(CRMContext context) : base(context) { }

//  public async Task<IEnumerable<GroupPermission>> AccessPermisionForCurrentUser(int moduleId, int userId)
//  {
//    string sql = string.Format(SELECT_ACCESSPERMISSION_BYMODULE_AND_USER, moduleId, userId, DateTime.Now.ToString("MM/dd/yyyy"));
//    IEnumerable<GroupPermission> groupPermissionRepositoriesDto = await ExecuteListQuery<GroupPermission>(sql);
//    return groupPermissionRepositoriesDto;
//  }


//}
