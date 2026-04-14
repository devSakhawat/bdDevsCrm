using bdDevCRM.Entities.Entities.DMS;
using bdDevCRM.RepositoriesContracts.DMS;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.DMS;

/// <summary>
/// Repository implementation for DmsDocumentAccessLog entity operations.
/// </summary>
//public class DmsDocumentAccessLogRepository : RepositoryBase<DmsDocumentAccessLog>, IDmsDocumentAccessLogRepository
public class DmsDocumentAccessLogRepository : RepositoryBase<DmsDocumentAccessLog>, IDmsDocumentAccessLogRepository
{
  public DmsDocumentAccessLogRepository(CRMContext context) : base(context) { }

  /// <summary>
  /// Retrieves all document access logs ordered by LogId.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentAccessLog>> AccessLogsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.LogId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// s access logs by document ID.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentAccessLog>> AccessLogsByDocumentAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(x => x.DocumentId == documentId, x => x.LogId, trackChanges, cancellationToken: cancellationToken);
  }

  /// <summary>
  /// s access logs by user ID.
  /// </summary>
  public async Task<IEnumerable<DmsDocumentAccessLog>> AccessLogsByUserAsync(int userId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByConditionAsync(x => x.LogId == userId, x => x.LogId, trackChanges,   cancellationToken: cancellationToken);
  }

  /// <summary>
  /// s a single access log by ID.
  /// </summary>
  public async Task<DmsDocumentAccessLog?> AccessLogAsync(int accessLogId, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.LogId == accessLogId, trackChanges, cancellationToken);
  }

  /// <summary>
  /// Creates a new access log.
  /// </summary>
  public async Task<DmsDocumentAccessLog> CreateAccessLogAsync(DmsDocumentAccessLog accessLog, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(accessLog, cancellationToken);
    accessLog.LogId = newId;
    return accessLog;
  }

  /// <summary>
  /// Updates an existing access log.
  /// </summary>
  public void UpdateAccessLog(DmsDocumentAccessLog accessLog) => UpdateByState(accessLog);

  /// <summary>
  /// Deletes an access log.
  /// </summary>
  public async Task DeleteAccessLogAsync(DmsDocumentAccessLog accessLog, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.LogId == accessLog.LogId, trackChanges, cancellationToken);
  }
}







//using bdDevCRM.Entities.Entities.DMS;
//using bdDevCRM.RepositoriesContracts.DMS;
//using bdDevCRM.Sql.Context;

//namespace bdDevCRM.Repositories.DMS;

//public class DmsDocumentAccessLogRepository : RepositoryBase<DmsDocumentAccessLog>, IDmsDocumentAccessLogRepository
//{
//  public DmsDocumentAccessLogRepository(CRMContext context) : base(context) { }

//  //  access logs by DocumentId
//  public async Task<IEnumerable<DmsDocumentAccessLog>> LogsByDocumentIdAsync(int documentId, bool trackChanges) =>
//      await ListByConditionAsync(x => x.DocumentId == documentId, x => x.AccessDateTime, trackChanges);

//  // Create new log
//  public void CreateLog(DmsDocumentAccessLog log) => Create(log);
//}
