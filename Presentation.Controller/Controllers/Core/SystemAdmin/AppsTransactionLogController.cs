using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Apps Transaction Log management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AuthorizeUser]
public class AppsTransactionLogController : BaseApiController
{
    public AppsTransactionLogController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all apps transaction logs for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AppsTransactionLogDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AppsTransactionLogsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var appsTransactionLogs = await _serviceManager.AppsTransactionLogs.AppsTransactionLogsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!appsTransactionLogs.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AppsTransactionLogDDLDto>(), "No apps transaction logs found."));

        return Ok(ApiResponseHelper.Success(appsTransactionLogs, "Apps transaction logs retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of apps transaction logs.
    /// </summary>
    [HttpPost(RouteConstants.AppsTransactionLogSummary)]
    public async Task<IActionResult> AppsTransactionLogSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AppsTransactionLogs.AppsTransactionLogsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AppsTransactionLogDto>(), "No apps transaction logs found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Apps transaction log summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new apps transaction log record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAppsTransactionLog)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAppsTransactionLogAsync([FromBody] CreateAppsTransactionLogRecord record, CancellationToken cancellationToken = default)
    {
        var createdAppsTransactionLog = await _serviceManager.AppsTransactionLogs.CreateAsync(record, cancellationToken);

        if (createdAppsTransactionLog.TransactionLogId <= 0)
            throw new InvalidCreateOperationException("Failed to create apps transaction log record.");

        return Ok(ApiResponseHelper.Created(createdAppsTransactionLog, "Apps transaction log created successfully."));
    }

    /// <summary>
    /// Updates an existing apps transaction log record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAppsTransactionLog)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAppsTransactionLogAsync([FromRoute] int key, [FromBody] UpdateAppsTransactionLogRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.TransactionLogId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAppsTransactionLogRecord));

        var updatedAppsTransactionLog = await _serviceManager.AppsTransactionLogs.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAppsTransactionLog, "Apps transaction log updated successfully."));
    }

    /// <summary>
    /// Deletes an apps transaction log record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAppsTransactionLog)]
    public async Task<IActionResult> DeleteAppsTransactionLogAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAppsTransactionLogRecord(key);
        await _serviceManager.AppsTransactionLogs.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Apps transaction log deleted successfully"));
    }

    /// <summary>
    /// Retrieves an apps transaction log by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAppsTransactionLog)]
    public async Task<IActionResult> AppsTransactionLogAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var appsTransactionLog = await _serviceManager.AppsTransactionLogs.AppsTransactionLogAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(appsTransactionLog, "Apps transaction log retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all apps transaction logs.
    /// </summary>
    [HttpGet(RouteConstants.ReadAppsTransactionLogs)]
    public async Task<IActionResult> AppsTransactionLogsAsync(CancellationToken cancellationToken = default)
    {
        var appsTransactionLogs = await _serviceManager.AppsTransactionLogs.AppsTransactionLogsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!appsTransactionLogs.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AppsTransactionLogDto>(), "No apps transaction logs found."));

        return Ok(ApiResponseHelper.Success(appsTransactionLogs, "Apps transaction logs retrieved successfully"));
    }
}
