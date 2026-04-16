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
/// Access Restriction management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AuthorizeUser]
public class AccessRestrictionController : BaseApiController
{
    public AccessRestrictionController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all access restrictions for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AccessRestrictionDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AccessRestrictionsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var accessRestrictions = await _serviceManager.AccessRestrictions.AccessRestrictionsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!accessRestrictions.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AccessRestrictionDDLDto>(), "No access restrictions found."));

        return Ok(ApiResponseHelper.Success(accessRestrictions, "Access restrictions retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of access restrictions.
    /// </summary>
    [HttpPost(RouteConstants.AccessRestrictionSummary)]
    public async Task<IActionResult> AccessRestrictionSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AccessRestrictions.AccessRestrictionsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AccessRestrictionDto>(), "No access restrictions found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Access restriction summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new access restriction record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAccessRestriction)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAccessRestrictionAsync([FromBody] CreateAccessRestrictionRecord record, CancellationToken cancellationToken = default)
    {
        var createdAccessRestriction = await _serviceManager.AccessRestrictions.CreateAsync(record, cancellationToken);

        if (createdAccessRestriction.AccessRestrictionId <= 0)
            throw new InvalidCreateOperationException("Failed to create access restriction record.");

        return Ok(ApiResponseHelper.Created(createdAccessRestriction, "Access restriction created successfully."));
    }

    /// <summary>
    /// Updates an existing access restriction record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAccessRestriction)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAccessRestrictionAsync([FromRoute] int key, [FromBody] UpdateAccessRestrictionRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AccessRestrictionId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAccessRestrictionRecord));

        var updatedAccessRestriction = await _serviceManager.AccessRestrictions.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAccessRestriction, "Access restriction updated successfully."));
    }

    /// <summary>
    /// Deletes an access restriction record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAccessRestriction)]
    public async Task<IActionResult> DeleteAccessRestrictionAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAccessRestrictionRecord(key);
        await _serviceManager.AccessRestrictions.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Access restriction deleted successfully"));
    }

    /// <summary>
    /// Retrieves an access restriction by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAccessRestriction)]
    public async Task<IActionResult> AccessRestrictionAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var accessRestriction = await _serviceManager.AccessRestrictions.AccessRestrictionAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(accessRestriction, "Access restriction retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all access restrictions.
    /// </summary>
    [HttpGet(RouteConstants.ReadAccessRestrictions)]
    public async Task<IActionResult> AccessRestrictionsAsync(CancellationToken cancellationToken = default)
    {
        var accessRestrictions = await _serviceManager.AccessRestrictions.AccessRestrictionsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!accessRestrictions.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AccessRestrictionDto>(), "No access restrictions found."));

        return Ok(ApiResponseHelper.Success(accessRestrictions, "Access restrictions retrieved successfully"));
    }
}
