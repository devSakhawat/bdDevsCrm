using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.LinkFactories;

namespace Presentation.Controllers.Core.SystemAdmin;

/// <summary>
/// Access control management endpoints.
///
/// [AuthorizeUser] at class-level ensures:
///    - Every request validates user via attribute
///    - CurrentUser / CurrentUserId available from BaseApiController
///    - No auth checks needed in controller methods
///    - Exceptions handled by StandardExceptionMiddleware
/// </summary>
[AuthorizeUser]
public class AccessControlController : BaseApiController
{
    private readonly IMemoryCache _cache;
    private readonly ILinkFactory<AccessControlDto> _linkFactory;

    public AccessControlController(IServiceManager serviceManager, IMemoryCache cache, ILinkFactory<AccessControlDto> linkFactory) : base(serviceManager)
    {
        _cache = cache;
        _linkFactory = linkFactory;
    }

    /// <summary>
    /// Retrieves paginated summary grid of access controls.
    /// </summary>
    [HttpPost(RouteConstants.AccessControlSummary)]
    public async Task<IActionResult> AccessControlSummaryAsync([FromBody] CRMGridOptions options)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(CRMGridOptions));

        var summaryGrid = await _serviceManager.AccessControls.SummaryAsync(trackChanges: false, options);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AccessControlDto>(), "No access controls found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Access controls retrieved successfully"));
    }

    /// <summary>
    /// Creates a new access control record.
    /// </summary>
    [HttpPost(RouteConstants.CreateAccessControl)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAccessControlAsync([FromBody] AccessControlDto modelDto)
    {
        var createdAccessControl = await _serviceManager.AccessControls.CreateAsync(modelDto);

        if (createdAccessControl.AccessId <= 0)
            throw new InvalidCreateOperationException("Failed to create access control record.");

        return Ok(ApiResponseHelper.Created(createdAccessControl, "Access control created successfully."));
    }

    /// <summary>
    /// Updates an existing access control record.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAccessControl)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAccessControlAsync([FromRoute] int key, [FromBody] AccessControlDto modelDto ,CancellationToken cancellationToken = default)
    {
        if (key != modelDto.AccessId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(AccessControlDto));

        var updatedAccessControl = await _serviceManager.AccessControls.UpdateAsync(key, modelDto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAccessControl, "Access control updated successfully."));
    }

    /// <summary>
    /// Deletes an access control record by ID.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAccessControl)]
    public async Task<IActionResult> DeleteAccessControlAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.AccessControls.DeleteAsync(key, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Access control deleted successfully"));
    }

    /// <summary>
    /// Retrieves an access control by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAccessControl)]
    public async Task<IActionResult> AccessControlAsync([FromRoute] int id)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var accessControl = await _serviceManager.AccessControls.AccessControlAsync(id, trackChanges: false);

        return Ok(ApiResponseHelper.Success(accessControl, "Access control retrieved successfully"));
    }

    ///// <summary>
    ///// Retrieves access controls for dropdown list.
    ///// </summary>
    //[HttpGet(RouteConstants.AccessControlForDDL)]
    //public async Task<IActionResult> AccessControlsForDDLAsync()
    //{
    //    var accessControls = await _serviceManager.AccessControls.AccessControlsForDDLAsync();

    //    if (!accessControls.Any())
    //        return Ok(ApiResponseHelper.Success(Enumerable.Empty<AccessControlForDDLDto>(), "No access controls found."));

    //    return Ok(ApiResponseHelper.Success(accessControls, "Access controls retrieved successfully"));
    //}
}
