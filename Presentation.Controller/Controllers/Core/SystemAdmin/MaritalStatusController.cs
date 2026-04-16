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
/// Marital Status management endpoints.
/// </summary>
[AuthorizeUser]
public class MaritalStatusController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public MaritalStatusController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all marital statuses for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.MaritalStatusDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> MaritalStatusesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var maritalStatuses = await _serviceManager.MaritalStatuses.MaritalStatusesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!maritalStatuses.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<MaritalStatusDDLDto>(), "No marital statuses found."));

        return Ok(ApiResponseHelper.Success(maritalStatuses, "Marital statuses retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of marital statuses.
    /// </summary>
    [HttpPost(RouteConstants.MaritalStatusSummary)]
    public async Task<IActionResult> MaritalStatusSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.MaritalStatuses.MaritalStatusesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<MaritalStatusDto>(), "No marital statuses found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Marital status summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new marital status record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateMaritalStatus)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateMaritalStatusAsync([FromBody] CreateMaritalStatusRecord record, CancellationToken cancellationToken = default)
    {
        var createdMaritalStatus = await _serviceManager.MaritalStatuses.CreateAsync(record, cancellationToken);

        if (createdMaritalStatus.MaritalStatusId <= 0)
            throw new InvalidCreateOperationException("Failed to create marital status record.");

        return Ok(ApiResponseHelper.Created(createdMaritalStatus, "Marital status created successfully."));
    }

    /// <summary>
    /// Updates an existing marital status record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateMaritalStatus)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateMaritalStatusAsync([FromRoute] int key, [FromBody] UpdateMaritalStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.MaritalStatusId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateMaritalStatusRecord));

        var updatedMaritalStatus = await _serviceManager.MaritalStatuses.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedMaritalStatus, "Marital status updated successfully."));
    }

    /// <summary>
    /// Deletes a marital status record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteMaritalStatus)]
    public async Task<IActionResult> DeleteMaritalStatusAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteMaritalStatusRecord(key);
        await _serviceManager.MaritalStatuses.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Marital status deleted successfully"));
    }

    /// <summary>
    /// Retrieves a marital status by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadMaritalStatus)]
    public async Task<IActionResult> MaritalStatusAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var maritalStatus = await _serviceManager.MaritalStatuses.MaritalStatusAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(maritalStatus, "Marital status retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all marital statuses.
    /// </summary>
    [HttpGet(RouteConstants.ReadMaritalStatuses)]
    public async Task<IActionResult> MaritalStatusesAsync(CancellationToken cancellationToken = default)
    {
        var maritalStatuses = await _serviceManager.MaritalStatuses.MaritalStatusesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!maritalStatuses.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<MaritalStatusDto>(), "No marital statuses found."));

        return Ok(ApiResponseHelper.Success(maritalStatuses, "Marital statuses retrieved successfully"));
    }
}
