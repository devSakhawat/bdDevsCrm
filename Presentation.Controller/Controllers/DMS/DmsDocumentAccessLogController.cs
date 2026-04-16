using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.DMS;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers.DMS;

/// <summary>
/// DMS document access log management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class DmsDocumentAccessLogController : BaseApiController
{
    public DmsDocumentAccessLogController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves paginated summary grid of document access logs.
    /// </summary>
    [HttpPost("dms-document-access-log-summary")]
    public async Task<IActionResult> AccessLogSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DmsDocumentAccessLogs.AccessLogsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DmsDocumentAccessLogDto>(), "No access logs found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Access log summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new document access log record.
    /// </summary>
    [HttpPost("dms-document-access-log")]
    public async Task<IActionResult> CreateAccessLogAsync([FromBody] DmsDocumentAccessLogDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentAccessLogDto));

        var createdAccessLog = await _serviceManager.DmsDocumentAccessLogs.CreateAccessLogAsync(dto, cancellationToken);

        if (createdAccessLog.LogId <= 0)
            throw new InvalidCreateOperationException("Failed to create access log record.");

        return Ok(ApiResponseHelper.Created(createdAccessLog, "Access log created successfully."));
    }

    /// <summary>
    /// Updates an existing document access log record.
    /// </summary>
    [HttpPut("dms-document-access-log/{key}")]
    public async Task<IActionResult> UpdateAccessLogAsync([FromRoute] int key, [FromBody] DmsDocumentAccessLogDto dto, CancellationToken cancellationToken = default)
    {
        if (dto == null)
            throw new NullModelBadRequestException(nameof(DmsDocumentAccessLogDto));

        if (key != dto.LogId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(DmsDocumentAccessLogDto));

        var updatedAccessLog = await _serviceManager.DmsDocumentAccessLogs.UpdateAccessLogAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAccessLog, "Access log updated successfully."));
    }

    /// <summary>
    /// Deletes a document access log record.
    /// </summary>
    [HttpDelete("dms-document-access-log/{key}")]
    public async Task<IActionResult> DeleteAccessLogAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        if (key <= 0)
            throw new IdParametersBadRequestException();

        await _serviceManager.DmsDocumentAccessLogs.DeleteAccessLogAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Access log deleted successfully"));
    }

    /// <summary>
    /// Retrieves a document access log by ID.
    /// </summary>
    [HttpGet("dms-document-access-log/{logId:int}")]
    public async Task<IActionResult> AccessLogAsync([FromRoute] int logId, CancellationToken cancellationToken = default)
    {
        if (logId <= 0)
            throw new IdParametersBadRequestException();

        var accessLog = await _serviceManager.DmsDocumentAccessLogs.AccessLogAsync(logId, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(accessLog, "Access log retrieved successfully"));
    }
}
