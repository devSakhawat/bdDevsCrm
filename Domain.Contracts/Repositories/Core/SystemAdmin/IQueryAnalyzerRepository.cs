// Interface: IQueryAnalyzerRepository
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin
{
  public interface IQueryAnalyzerRepository : IRepositoryBase<ReportBuilder>
  {
    Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportsInfoAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<QueryAnalyzerDto>> CustomizedReportsByPermissionAsync(Users currentUser, string condition, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<QueryAnalyzerDto>> GroupPermissionsForQueryAnalyzerReportAsync(Users currentUser, CancellationToken cancellationToken = default);
  }
}


//using Domain.Entities.Entities.System;
//using Domain.Contracts.Repositories;

//namespace Domain.Contracts.Core.SystemAdmin;

//public interface IQueryAnalyzerRepository : IRepositoryBase<ReportBuilder>
//{

//  Task<IEnumerable<QueryAnalyzer>> CustomizedReportInfo(bool trackChanges);

//  Task<IEnumerable<QueryAnalyzer>> CustomizedReportByPermission(Users currentUser, string condition, bool trackChanges);

//  Task<IEnumerable<QueryAnalyzer>> GroupPermissionForQueryAnalyzerReport(Users currentUser);

//}
