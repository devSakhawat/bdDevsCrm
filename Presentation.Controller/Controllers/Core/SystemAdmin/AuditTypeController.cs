using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Audit Type management endpoints.
/// </summary>
[AuthorizeUser]
public class AuditTypeController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public AuditTypeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all audit types for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AuditTypeDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AuditTypesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var auditTypes = await _serviceManager.AuditTypes.AuditTypesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!auditTypes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AuditTypeDDLDto>(), "No audit types found."));

        return Ok(ApiResponseHelper.Success(auditTypes, "Audit types retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of audit types.
    /// </summary>
    [HttpPost(RouteConstants.AuditTypeSummary)]
    public async Task<IActionResult> AuditTypeSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AuditTypes.AuditTypesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AuditTypeDto>(), "No audit types found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Audit type summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new audit type record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAuditType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAuditTypeAsync([FromBody] CreateAuditTypeRecord record, CancellationToken cancellationToken = default)
    {
        var createdAuditType = await _serviceManager.AuditTypes.CreateAsync(record, cancellationToken);

        if (createdAuditType.AuditTypeId <= 0)
            throw new InvalidCreateOperationException("Failed to create audit type record.");

        return Ok(ApiResponseHelper.Created(createdAuditType, "Audit type created successfully."));
    }

    /// <summary>
    /// Updates an existing audit type record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAuditType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAuditTypeAsync([FromRoute] int key, [FromBody] UpdateAuditTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AuditTypeId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAuditTypeRecord));

        var updatedAuditType = await _serviceManager.AuditTypes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAuditType, "Audit type updated successfully."));
    }

    /// <summary>
    /// Deletes an audit type record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAuditType)]
    public async Task<IActionResult> DeleteAuditTypeAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAuditTypeRecord(key);
        await _serviceManager.AuditTypes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Audit type deleted successfully"));
    }

    /// <summary>
    /// Retrieves an audit type by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAuditType)]
    public async Task<IActionResult> AuditTypeAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var auditType = await _serviceManager.AuditTypes.AuditTypeAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(auditType, "Audit type retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all audit types.
    /// </summary>
    [HttpGet(RouteConstants.ReadAuditTypes)]
    public async Task<IActionResult> AuditTypesAsync(CancellationToken cancellationToken = default)
    {
        var auditTypes = await _serviceManager.AuditTypes.AuditTypesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!auditTypes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AuditTypeDto>(), "No audit types found."));

        return Ok(ApiResponseHelper.Success(auditTypes, "Audit types retrieved successfully"));
    }
}
