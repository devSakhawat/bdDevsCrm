// IDmsDocumentAccessLogService.cs
using bdDevCRM.Shared.DataTransferObjects.DMS;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.DMS;

/// <summary>
/// Service contract for DMS document access log management operations.
/// Defines methods for creating, updating, deleting, and retrieving document access log data.
/// </summary>
public interface IDmsDocumentAccessLogService
{
	/// <summary>
	/// Retrieves all document access log records from the database.
	/// </summary>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A collection of all <see cref="DmsDocumentAccessLogDto"/> records.</returns>
	Task<IEnumerable<DmsDocumentAccessLogDto>> AccessLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	///// <summary>
	///// Retrieves a lightweight list of all document access logs suitable for use in dropdown lists.
	///// </summary>
	///// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	///// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	///// <returns>A collection of <see cref="DmsDocumentAccessLogDDL"/> for dropdown binding.</returns>
	//Task<IEnumerable<DmsDocumentAccessLogDDL>> AccessLogsDDLAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a paginated summary grid of all document access logs.
	/// </summary>
	/// <param name="options">The grid options including pagination, filtering, and sorting parameters.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>A <see cref="GridEntity{DmsDocumentAccessLogDto}"/> containing the paged access log data.</returns>
	Task<GridEntity<DmsDocumentAccessLogDto>> AccessLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single document access log record by its ID.
	/// </summary>
	/// <param name="id">The ID of the access log to retrieve.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The <see cref="DmsDocumentAccessLogDto"/> matching the specified ID.</returns>
	/// <exception cref="NotFoundException">Thrown when no access log is found for the given ID.</exception>
	Task<DmsDocumentAccessLogDto> AccessLogAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new document access log record.
	/// </summary>
	/// <param name="entityForCreate">The DTO containing data for the new access log.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The created <see cref="DmsDocumentAccessLogDto"/> with the newly assigned ID.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="entityForCreate"/> is null.</exception>
	/// <exception cref="InvalidCreateOperationException">Thrown when LogId is not 0 for new creation.</exception>
	Task<DmsDocumentAccessLogDto> CreateAccessLogAsync(DmsDocumentAccessLogDto entityForCreate, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing document access log record.
	/// </summary>
	/// <param name="logId">The ID of the access log to update.</param>
	/// <param name="modelDto">The DTO containing updated field values.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The updated <see cref="DmsDocumentAccessLogDto"/> reflecting the saved state.</returns>
	/// <exception cref="BadRequestException">Thrown when <paramref name="modelDto"/> is null.</exception>
	/// <exception cref="BadRequestException">Thrown when route ID does not match DTO ID.</exception>
	/// <exception cref="NotFoundException">Thrown when no access log is found for the given ID.</exception>
	Task<DmsDocumentAccessLogDto> UpdateAccessLogAsync(int logId, DmsDocumentAccessLogDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Deletes a document access log record identified by the given ID.
	/// </summary>
	/// <param name="logId">The ID of the access log to delete.</param>
	/// <param name="trackChanges">Indicates whether EF change tracking should be enabled.</param>
	/// <param name="cancellationToken">Token to monitor for cancellation requests.</param>
	/// <returns>The number of affected rows.</returns>
	/// <exception cref="IdParametersBadRequestException">Thrown when <paramref name="logId"/> is zero or negative.</exception>
	/// <exception cref="NotFoundException">Thrown when no access log record is found for the given ID.</exception>
	Task<int> DeleteAccessLogAsync(int logId, bool trackChanges, CancellationToken cancellationToken = default);
}

//using Application.Shared.Grid;
//using bdDevCRM.Shared.DataTransferObjects.DMS;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace bdDevCRM.ServiceContract.DMS;

//public interface IDmsDocumentAccessLogService
//{
//  Task<IEnumerable<DmsDocumentAccessLogDDL>> AccessLogsDDLAsync(bool trackChanges);
//  Task<GridEntity<DmsDocumentAccessLogDto>> SummaryGrid(GridOptions options);
//  Task<string> CreateNewRecordAsync(DmsDocumentAccessLogDto modelDto);
//  Task<string> UpdateNewRecordAsync(int key, DmsDocumentAccessLogDto modelDto, bool trackChanges);
//  Task<string> DeleteRecordAsync(int key, DmsDocumentAccessLogDto modelDto);
//  Task<string> SaveOrUpdate(int key, DmsDocumentAccessLogDto modelDto);
//}