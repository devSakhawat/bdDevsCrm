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
/// Access control management endpoints.
/// </summary>
[AuthorizeUser]
public class AccessControlController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public AccessControlController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all access controls for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AccessControls)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AccesscontrolsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var accessControls = await _serviceManager.AccessControls.AccesscontrolsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!accessControls.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AccessControlDto>(), "No access controls found."));

        return Ok(ApiResponseHelper.Success(accessControls, "Access controls retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of access controls.
    /// </summary>
    [HttpPost(RouteConstants.AccessControlSummary)]
    public async Task<IActionResult> AccessControlSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AccessControls.AccesscontrolsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AccessControlDto>(), "No access controls found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Access control summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new access control record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAccessControl)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAccessControlAsync([FromBody] CreateAccesscontrolRecord record, CancellationToken cancellationToken = default)
    {
        var createdAccessControl = await _serviceManager.AccessControls.CreateAsync(record, cancellationToken);

        if (createdAccessControl.AccessId <= 0)
            throw new InvalidCreateOperationException("Failed to create access control record.");

        return Ok(ApiResponseHelper.Created(createdAccessControl, "Access control created successfully."));
    }

    /// <summary>
    /// Updates an existing access control record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAccessControl)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAccessControlAsync([FromRoute] int key, [FromBody] UpdateAccesscontrolRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AccessId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAccesscontrolRecord));

        var updatedAccessControl = await _serviceManager.AccessControls.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAccessControl, "Access control updated successfully."));
    }

    /// <summary>
    /// Deletes an access control record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAccessControl)]
    public async Task<IActionResult> DeleteAccessControlAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAccesscontrolRecord(key);
        await _serviceManager.AccessControls.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Access control deleted successfully"));
    }

    /// <summary>
    /// Retrieves an access control by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAccessControl)]
    public async Task<IActionResult> AccessControlAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        if (key <= 0)
            throw new IdParametersBadRequestException();

        var accessControl = await _serviceManager.AccessControls.AccesscontrolAsync(key, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(accessControl, "Access control retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all access controls.
    /// </summary>
    [HttpGet(RouteConstants.ReadAccessControls)]
    public async Task<IActionResult> AccessControlsAsync(CancellationToken cancellationToken = default)
    {
        var accessControls = await _serviceManager.AccessControls.AccesscontrolsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!accessControls.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AccessControlDto>(), "No access controls found."));

        return Ok(ApiResponseHelper.Success(accessControls, "Access controls retrieved successfully"));
    }
}
