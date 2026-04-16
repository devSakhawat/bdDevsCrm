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
/// Thana management endpoints.
/// </summary>
[AuthorizeUser]
public class ThanaController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public ThanaController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all thanas for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.ThanaDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ThanasForDDLAsync(CancellationToken cancellationToken = default)
    {
        var thanas = await _serviceManager.Thanas.ThanasForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!thanas.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ThanaDDLDto>(), "No thanas found."));

        return Ok(ApiResponseHelper.Success(thanas, "Thanas retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of thanas.
    /// </summary>
    [HttpPost(RouteConstants.ThanaSummary)]
    public async Task<IActionResult> ThanaSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.Thanas.ThanasSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ThanaDto>(), "No thanas found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Thana summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new thana record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateThana)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateThanaAsync([FromBody] CreateThanaRecord record, CancellationToken cancellationToken = default)
    {
        var createdThana = await _serviceManager.Thanas.CreateAsync(record, cancellationToken);

        if (createdThana.ThanaId <= 0)
            throw new InvalidCreateOperationException("Failed to create thana record.");

        return Ok(ApiResponseHelper.Created(createdThana, "Thana created successfully."));
    }

    /// <summary>
    /// Updates an existing thana record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateThana)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateThanaAsync([FromRoute] int key, [FromBody] UpdateThanaRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ThanaId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateThanaRecord));

        var updatedThana = await _serviceManager.Thanas.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedThana, "Thana updated successfully."));
    }

    /// <summary>
    /// Deletes a thana record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteThana)]
    public async Task<IActionResult> DeleteThanaAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteThanaRecord(key);
        await _serviceManager.Thanas.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Thana deleted successfully"));
    }

    /// <summary>
    /// Retrieves a thana by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadThana)]
    public async Task<IActionResult> ThanaAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var thana = await _serviceManager.Thanas.ThanaAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(thana, "Thana retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all thanas.
    /// </summary>
    [HttpGet(RouteConstants.ReadThanas)]
    public async Task<IActionResult> ThanasAsync(CancellationToken cancellationToken = default)
    {
        var thanas = await _serviceManager.Thanas.ThanasAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!thanas.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ThanaDto>(), "No thanas found."));

        return Ok(ApiResponseHelper.Success(thanas, "Thanas retrieved successfully"));
    }

    /// <summary>
    /// Retrieves thanas by district ID.
    /// </summary>
    [HttpGet(RouteConstants.ThanasByDistrict)]
    public async Task<IActionResult> ThanasByDistrictAsync([FromRoute] int districtId, CancellationToken cancellationToken = default)
    {
        if (districtId <= 0)
            throw new IdParametersBadRequestException();

        var thanas = await _serviceManager.Thanas.ThanasByDistrictIdAsync(districtId, trackChanges: false, cancellationToken: cancellationToken);

        if (!thanas.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ThanaDto>(), "No thanas found for this district."));

        return Ok(ApiResponseHelper.Success(thanas, "Thanas retrieved successfully"));
    }
}
