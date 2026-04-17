using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

public class AccessRestrictionRepository : RepositoryBase<AccessRestriction>, IAccessRestrictionRepository
{
	public AccessRestrictionRepository(CrmContext context) : base(context) { }

	#region Priority 1: Basic Retrieval (EF Core)

	public async Task<AccessRestriction?> AccessRestrictionAsync(int accessRestrictionId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(x => x.AccessRestrictionId == accessRestrictionId, trackChanges, cancellationToken);
	}

	public async Task<IEnumerable<AccessRestriction>> AccessRestrictionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(null, trackChanges, cancellationToken);
	}

	#endregion

	#region Priority 2: Retrieval with Filters (ADO.NET)

	/// <summary>
	/// Retrieves groups associated with a specific HR record ID using ADO.NET.
	/// </summary>
	public async Task<IEnumerable<Groups>> GroupsByHrRecordIdAsync(int hrRecordId, CancellationToken cancellationToken = default)
	{
		string query = string.Format(@"Select GroupId 
from GroupMember
inner join Users on Users.UserId = GroupMember.UserId
inner join Employment on Employment.HRRecordId = Users.EmployeeId
where HRRecordId = {0}", hrRecordId);

		return await AdoExecuteListQueryAsync<Groups>(query, null, cancellationToken);
	}

	/// <summary>
	/// Retrieves access restriction data based on HR record ID and group condition using ADO.NET.
	/// </summary>
	public async Task<IEnumerable<AccessRestriction>> AccessRestrictionsByHrRecordIdAsync(int hrRecordId, CancellationToken cancellationToken = default)
	{
		var query = string.Format(@"select Distinct ReferenceId,ReferenceType,ParentReference,ChiledParentReference from AccessRestriction where HrRecordId = {0}", hrRecordId);

		return await AdoExecuteListQueryAsync<AccessRestriction>(query, null, cancellationToken);
	}

	/// <summary>
	/// Generates a list of access restriction conditions for a company using ADO.NET.
	/// </summary>
	public async Task<IEnumerable<AccessRestriction>> AccessRestrictionConditionsAsync(int hrRecordId, int type, string groupCondition, CancellationToken cancellationToken = default)
	{
		var query = string.Format(@"select Distinct ReferenceId,ReferenceType,ParentReference,ChiledParentReference from AccessRestriction where (HrRecordId = {0} {2}) and ReferenceType={1}", hrRecordId, type, groupCondition);

		return await AdoExecuteListQueryAsync<AccessRestriction>(query, null, cancellationToken);
	}

	#endregion

	#region Priority 3: Business Logic / Generation

	/// <summary>
	/// Generates a SQL condition string for company access restrictions.
	/// </summary>
	public async Task<string> GenerateAccessRestrictionConditionAsync(int hrRecordId, CancellationToken cancellationToken = default)
	{
		var condition = string.Empty;

		// Fetching data using the class method (which uses ADO.NET for complex group fetching)
		var objGroups = await GroupsByHrRecordIdAsync(hrRecordId, cancellationToken);
		var groupCondition = string.Empty;

		if (objGroups != null && objGroups.Any())
		{
			var gids = string.Join(",", objGroups.Select(g => g.GroupId));
			if (!string.IsNullOrEmpty(gids))
			{
				groupCondition = $" or GroupId in ({gids})";
			}
		}

		IEnumerable<AccessRestriction> objAccessData = await AccessRestrictionConditionsAsync(hrRecordId, 1, groupCondition, cancellationToken);

		if (objAccessData != null && objAccessData.Any())
		{
			var ids = string.Join(",", objAccessData.Select(access => access.ReferenceId));
			condition = $"CompanyId in ({ids})";
		}

		return condition;
	}

	#endregion

	/// <summary>
	/// Creates a new access Restriction.
	/// </summary>
	public async Task<AccessRestriction> CreateAccessRestrictionAsync(AccessRestriction accessRestriction, CancellationToken cancellationToken = default)
	{
		int accessRestrictionId = await CreateAndIdAsync(accessRestriction, cancellationToken);
		accessRestriction.AccessRestrictionId = accessRestrictionId;
		return accessRestriction;
	}

	/// <summary>
	/// Updates an existing access Restriction.
	/// </summary>
	public void UpdateAccessRestriction(AccessRestriction accessRestriction) => UpdateByState(accessRestriction);

	/// <summary>
	/// Deletes an access Restriction.
	/// </summary>
	public async Task DeleteAccessRestrictionAsync(AccessRestriction accessRestriction, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.AccessRestrictionId == accessRestriction.AccessRestrictionId, trackChanges, cancellationToken);
}





//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Infrastructure.Sql.Context;
//using System.Data;
//using System.Text.RegularExpressions;
//using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

//namespace Infrastructure.Repositories.Core.SystemAdmin;


//public class AccessRestrictionRepository : RepositoryBase<AccessRestriction>, IAccessRestrictionRepository
//{
//  public AccessRestrictionRepository(CrmContext context) : base(context) { }

//  public async Task<IEnumerable<Groups>> AccessRestrictionGroupsByHrrecordId(int hrRecordId, CancellationToken cancellationToken)
//  {
//    string query = string.Format(@"Select GroupId 
//from GroupMember
//inner join Users on Users.UserId = GroupMember.UserId
//inner join Employment on Employment.HRRecordId = Users.EmployeeId
//where HRRecordId = {0}", hrRecordId);
//    IEnumerable<Groups> groups = await ExecuteListQueryAsync<Groups>(query, cancellationToken: cancellationToken);
//    return groups;
//  }

//  public async Task<IEnumerable<AccessRestriction>> AccessRestrictionByHrRecordId(int hrRecordId, string groupCondition, CancellationToken cancellationToken)
//  {
//    var query = string.Format(@"select Distinct ReferenceId,ReferenceType,ParentReference,ChiledParentReference from AccessRestriction where (HrRecordId = {0} {1})", hrRecordId, groupCondition);

//    IEnumerable<AccessRestriction> accessRestrictionData = await ExecuteListQueryAsync<AccessRestriction>(query, cancellationToken: cancellationToken);
//    return accessRestrictionData;
//  }

//  public async Task<IEnumerable<Groups>> GroupInfo(int hrRecordId, CancellationToken cancellationToken)
//  {
//    string query = string.Format(@"Select GroupId from GroupMember
//inner join Users on Users.UserId = GroupMember.UserId
//inner join Employment on Employment.HRRecordId = Users.EmployeeId
//where HRRecordId = {0}", hrRecordId);

//    IEnumerable<Groups> accessRestrictionData = await ExecuteListQueryAsync<Groups>(query, cancellationToken: cancellationToken);
//    return accessRestrictionData;
//  }

//  public async Task<IEnumerable<AccessRestriction>> GenerateAccessRestrictionConditionListForCompany(int hrRecordId, int type, string gpcondition, CancellationToken cancellationToken)
//  {
//    var query = string.Format(@"select Distinct ReferenceId,ReferenceType,ParentReference,ChiledParentReference from AccessRestriction where (HrRecordId = {0} {2}) and ReferenceType={1}", hrRecordId, type, gpcondition);

//    var data = await ExecuteListQueryAsync<AccessRestriction>(query, cancellationToken: cancellationToken);
//    return data;
//  }

//  public async Task<string> GenerateAccessRestrictionConditionForCompany(int hrRecordId, CancellationToken cancellationToken)
//  {
//    var condition = string.Empty;
//    var objGroups = await GroupInfo(hrRecordId, cancellationToken);
//    var groupCondition = string.Empty;

//    if (objGroups != null && objGroups.Any())
//    {
//      var gids = string.Join(",", objGroups.Select(g => g.GroupId));
//      if (!string.IsNullOrEmpty(gids))
//      {
//        groupCondition = $" or GroupId in ({gids})";
//      }
//    }

//    IEnumerable<AccessRestriction> objAccessData = await GenerateAccessRestrictionConditionListForCompany(hrRecordId, 1, groupCondition, cancellationToken);
//    if (objAccessData != null && objAccessData.Any())
//    {
//      var ids = string.Join(",", objAccessData.Select(access => access.ReferenceId));
//      condition = $"CompanyId in ({ids})";
//    }

//    return condition;
//  }



//}
