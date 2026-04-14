using Domain.Contracts.Repositories;
using Domain.Entities.Entities.DMS;

namespace Domain.Contracts.DMS;

/// <summary>
/// Repository interface for DmsDocumentAccessLog entity operations.
/// </summary>
public interface IDmsDocumentAccessLogRepository : IRepositoryBase<DmsDocumentAccessLog>
{
  /// <summary>
  /// s all document access logs ordered by AccessLogId.
  /// </summary>
  Task<IEnumerable<DmsDocumentAccessLog>> AccessLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s access logs by document ID.
  /// </summary>
  Task<IEnumerable<DmsDocumentAccessLog>> AccessLogsByDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s access logs by user ID.
  /// </summary>
  Task<IEnumerable<DmsDocumentAccessLog>> AccessLogsByUserAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default);

  /// <summary>
  /// s a single access log by ID.
  /// </summary>
  Task<DmsDocumentAccessLog?> AccessLogAsync(int accessLogId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new access log.
	/// </summary>
	Task<DmsDocumentAccessLog> CreateAccessLogAsync(DmsDocumentAccessLog accessLog, CancellationToken cancellationToken = default);

  /// <summary>
  /// Updates an existing access log.
  /// </summary>
  void UpdateAccessLog(DmsDocumentAccessLog accessLog);

	/// <summary>
	/// Deletes an access log.
	/// </summary>
	Task DeleteAccessLogAsync(DmsDocumentAccessLog accessLog, bool trackChanges, CancellationToken cancellationToken = default);
}





//using Domain.Entities.Entities.DMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace bdDevCRM.RepositoriesContracts.DMS;

//public interface IDmsDocumentAccessLogRepository : IRepositoryBase<DmsDocumentAccessLog>
//{
//  Task<IEnumerable<DmsDocumentAccessLog>> LogsByDocumentIdAsync(int documentId, bool trackChanges);
//  void CreateLog(DmsDocumentAccessLog log);
//}