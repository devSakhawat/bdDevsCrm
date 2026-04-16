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
/// Audit Trail management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AuthorizeUser]
public class AuditTrailController : BaseApiController
{
    public AuditTrailController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all audit trails for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AuditTrailDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AuditTrailsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var auditTrails = await _serviceManager.AuditTrails.AuditTrailsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!auditTrails.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AuditTrailDDLDto>(), "No audit trails found."));

        return Ok(ApiResponseHelper.Success(auditTrails, "Audit trails retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of audit trails.
    /// </summary>
    [HttpPost(RouteConstants.AuditTrailSummary)]
    public async Task<IActionResult> AuditTrailSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AuditTrails.AuditTrailsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AuditTrailDto>(), "No audit trails found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Audit trail summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new audit trail record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAuditTrail)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAuditTrailAsync([FromBody] CreateAuditTrailRecord record, CancellationToken cancellationToken = default)
    {
        var createdAuditTrail = await _serviceManager.AuditTrails.CreateAsync(record, cancellationToken);

        if (createdAuditTrail.AuditId <= 0)
            throw new InvalidCreateOperationException("Failed to create audit trail record.");

        return Ok(ApiResponseHelper.Created(createdAuditTrail, "Audit trail created successfully."));
    }

    /// <summary>
    /// Updates an existing audit trail record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAuditTrail)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAuditTrailAsync([FromRoute] int key, [FromBody] UpdateAuditTrailRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AuditId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAuditTrailRecord));

        var updatedAuditTrail = await _serviceManager.AuditTrails.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAuditTrail, "Audit trail updated successfully."));
    }

    /// <summary>
    /// Deletes an audit trail record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAuditTrail)]
    public async Task<IActionResult> DeleteAuditTrailAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAuditTrailRecord(key);
        await _serviceManager.AuditTrails.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Audit trail deleted successfully"));
    }

    /// <summary>
    /// Retrieves an audit trail by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAuditTrail)]
    public async Task<IActionResult> AuditTrailAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var auditTrail = await _serviceManager.AuditTrails.AuditTrailAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(auditTrail, "Audit trail retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all audit trails.
    /// </summary>
    [HttpGet(RouteConstants.ReadAuditTrails)]
    public async Task<IActionResult> AuditTrailsAsync(CancellationToken cancellationToken = default)
    {
        var auditTrails = await _serviceManager.AuditTrails.AuditTrailsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!auditTrails.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AuditTrailDto>(), "No audit trails found."));

        return Ok(ApiResponseHelper.Success(auditTrails, "Audit trails retrieved successfully"));
    }
}
