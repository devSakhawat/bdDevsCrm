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
/// Audit Log management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AuthorizeUser]
public class AuditLogController : BaseApiController
{
    public AuditLogController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all audit logs for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AuditLogDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AuditLogsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var auditLogs = await _serviceManager.AuditLogs.AuditLogsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!auditLogs.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AuditLogDDLDto>(), "No audit logs found."));

        return Ok(ApiResponseHelper.Success(auditLogs, "Audit logs retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of audit logs.
    /// </summary>
    [HttpPost(RouteConstants.AuditLogSummary)]
    public async Task<IActionResult> AuditLogSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AuditLogs.AuditLogsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AuditLogDto>(), "No audit logs found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Audit log summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new audit log record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAuditLog)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAuditLogAsync([FromBody] CreateAuditLogRecord record, CancellationToken cancellationToken = default)
    {
        var createdAuditLog = await _serviceManager.AuditLogs.CreateAsync(record, cancellationToken);

        if (createdAuditLog.AuditId <= 0)
            throw new InvalidCreateOperationException("Failed to create audit log record.");

        return Ok(ApiResponseHelper.Created(createdAuditLog, "Audit log created successfully."));
    }

    /// <summary>
    /// Updates an existing audit log record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAuditLog)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAuditLogAsync([FromRoute] long key, [FromBody] UpdateAuditLogRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AuditId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAuditLogRecord));

        var updatedAuditLog = await _serviceManager.AuditLogs.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAuditLog, "Audit log updated successfully."));
    }

    /// <summary>
    /// Deletes an audit log record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAuditLog)]
    public async Task<IActionResult> DeleteAuditLogAsync([FromRoute] long key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAuditLogRecord(key);
        await _serviceManager.AuditLogs.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Audit log deleted successfully"));
    }

    /// <summary>
    /// Retrieves an audit log by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAuditLog)]
    public async Task<IActionResult> AuditLogAsync([FromRoute] long id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var auditLog = await _serviceManager.AuditLogs.AuditLogAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(auditLog, "Audit log retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all audit logs.
    /// </summary>
    [HttpGet(RouteConstants.ReadAuditLogs)]
    public async Task<IActionResult> AuditLogsAsync(CancellationToken cancellationToken = default)
    {
        var auditLogs = await _serviceManager.AuditLogs.AuditLogsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!auditLogs.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AuditLogDto>(), "No audit logs found."));

        return Ok(ApiResponseHelper.Success(auditLogs, "Audit logs retrieved successfully"));
    }
}
