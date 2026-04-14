

// Class: QueryAnalyzerRepository
using Domain.Entities.Entities.System;
using Domain.Contracts.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin
{
	public class QueryAnalyzerRepository : RepositoryBase<ReportBuilder>, IQueryAnalyzerRepository
	{
		public QueryAnalyzerRepository(CrmContext context) : base(context) { }

		/// <summary>
		/// Retrieves customized report information.
		/// </summary>
		public async Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportsInfoAsync(bool trackChanges, CancellationToken cancellationToken = default)
		{
			string query = @"SELECT tblQueryAnalyzer.ReportHeader ,tblQueryAnalyzer.ReportTitle ,tblQueryAnalyzer.ReportHeaderId
                            FROM (
                                SELECT ReportHeader + ' (Report)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,1 AS SortOrder
                                FROM ReportBuilder
                                WHERE IsActive = 1 AND QueryType = 1
                                UNION ALL
                                SELECT ReportHeader + ' (Document)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,2 AS SortOrder
                                FROM ReportBuilder
                                WHERE IsActive = 1 AND QueryType = 4
                            ) tblQueryAnalyzer
                            ORDER BY tblQueryAnalyzer.SortOrder ,ReportHeader";

			return await AdoExecuteListQueryAsync<QueryAnalyzerDto>(query, null, cancellationToken);
		}

		/// <summary>
		/// Retrieves customized reports by permission.
		/// </summary>
		public async Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportsByPermissionAsync(Users currentUser, string condition, bool trackChanges, CancellationToken cancellationToken = default)
		{
			string query = string.Format(@"SELECT t.ReportHeader ,t.ReportTitle ,t.ReportHeaderId
                                            FROM (
                                                SELECT ReportHeader + ' (Report)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,1 AS SortOrder
                                                FROM ReportBuilder
                                                WHERE IsActive = 1 AND QueryType = 1
                                                UNION ALL
                                                SELECT ReportHeader + ' (Document)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,2 AS SortOrder
                                                FROM ReportBuilder
                                                WHERE IsActive = 1 AND QueryType = 4
                                            ) T
                                            {0}
                                            ORDER BY t.SortOrder ,ReportHeader", condition);

			return await AdoExecuteListQueryAsync<QueryAnalyzerDto>(query, null, cancellationToken);
		}

		/// <summary>
		/// Retrieves group permissions for query analyzer reports.
		/// </summary>
		public async Task<IEnumerable<QueryAnalyzerDto>> GroupPermissionsForQueryAnalyzerReportAsync(Users currentUser, CancellationToken cancellationToken = default)
		{
			string query = string.Format(@"SELECT DISTINCT REFERENCEID AS ReportHeaderId
                                            FROM GroupPermission
                                            INNER JOIN GroupMember ON GroupMember.GroupId = GroupPermission.GROUPID
                                            WHERE UserId = {0} AND PERMISSIONTABLENAME = 'Customized Report'", currentUser.UserId);

			return await AdoExecuteListQueryAsync<QueryAnalyzerDto>(query, null, cancellationToken);
		}
	}
}






//using Domain.Entities.Entities;
//using Domain.Entities.Entities.System;
//using Domain.Contracts.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
//using Infrastructure.Sql.Context;

//namespace Infrastructure.Repositories.Core.SystemAdmin;

//public class QueryAnalyzerRepository : RepositoryBase<ReportBuilder>, IQueryAnalyzerRepository
//{
//  private const string SELECT_GROUPPERMISSION_BY_GROUPID = "Select * from GROUPPERMISSION where GROUPID = {0}";

//  public QueryAnalyzerRepository(CrmContext context) : base(context) { }

//  // summary data must be returned in a specific format like this.
//  public async Task<IEnumerable<QueryAnalyzer>> CustomizedReportInfo(bool trackChanges)
//  {
//    string queryAnalyzerQuery = $"SELECT tblQueryAnalyzer.ReportHeader ,tblQueryAnalyzer.ReportTitle ,tblQueryAnalyzer.ReportHeaderId\r\nFROM (\r\n\tSELECT ReportHeader + ' (Report)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,1 AS SortOrder\r\n\tFROM ReportBuilder\r\n\tWHERE IsActive = 1 AND QueryType = 1\r\n\t\r\n\tUNION ALL\r\n\t\r\n\tSELECT ReportHeader + ' (Document)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,2 AS SortOrder\r\n\tFROM ReportBuilder\r\n\tWHERE IsActive = 1 AND QueryType = 4\r\n\t) tblQueryAnalyzer\r\nORDER BY tblQueryAnalyzer.SortOrder ,ReportHeader";
//    IEnumerable<QueryAnalyzer> queryAnalyzers = await ExecuteListQuery<QueryAnalyzer>(queryAnalyzerQuery);
//    return queryAnalyzers;
//  }

//  public async Task<IEnumerable<QueryAnalyzer>> CustomizedReportByPermission(Users currentUser ,string condition ,bool trackChanges)
//  {
//    string queryAnalyzerQuery = string.Format(@"SELECT t.ReportHeader ,t.ReportTitle ,t.ReportHeaderId
//FROM (
//	SELECT ReportHeader + ' (Report)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,1 AS SortOrder
//	FROM ReportBuilder
//	WHERE IsActive = 1 AND QueryType = 1

//	UNION ALL

//	SELECT ReportHeader + ' (Document)' AS ReportHeader ,ReportTitle ,ReportHeaderId ,2 AS SortOrder
//	FROM ReportBuilder
//	WHERE IsActive = 1 AND QueryType = 4
//	) T
//{0}
//ORDER BY t.SortOrder ,ReportHeader", condition);

//    IEnumerable<QueryAnalyzer> queryAnalyzers = await ExecuteListQuery<QueryAnalyzer>(queryAnalyzerQuery);
//    return queryAnalyzers;
//  }

//  public async Task<IEnumerable<QueryAnalyzer>> GroupPermissionForQueryAnalyzerReport(Users currentUser)
//  {
//    string queryAnalyzerQuery = string.Format(@"SELECT DISTINCT REFERENCEID AS ReportHeaderId
//FROM GroupPermission
//INNER JOIN GroupMember ON GroupMember.GroupId = GroupPermission.GROUPID
//WHERE UserId = {0} AND PERMISSIONTABLENAME = 'Customized Report'", currentUser.UserId);
//    var groupPermissionList = await ExecuteListQuery<QueryAnalyzer>(queryAnalyzerQuery);
//    return groupPermissionList;
//  }
//}
