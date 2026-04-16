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
/// Apps Token Info management endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[AuthorizeUser]
public class AppsTokenInfoController : BaseApiController
{
    public AppsTokenInfoController(IServiceManager serviceManager) : base(serviceManager)
    {
    }

    /// <summary>
    /// Retrieves all apps token infos for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AppsTokenInfoDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AppsTokenInfosForDDLAsync(CancellationToken cancellationToken = default)
    {
        var appsTokenInfos = await _serviceManager.AppsTokenInfos.AppsTokenInfosForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!appsTokenInfos.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AppsTokenInfoDDLDto>(), "No apps token infos found."));

        return Ok(ApiResponseHelper.Success(appsTokenInfos, "Apps token infos retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of apps token infos.
    /// </summary>
    [HttpPost(RouteConstants.AppsTokenInfoSummary)]
    public async Task<IActionResult> AppsTokenInfoSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AppsTokenInfos.AppsTokenInfosSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AppsTokenInfoDto>(), "No apps token infos found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Apps token info summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new apps token info record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAppsTokenInfo)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAppsTokenInfoAsync([FromBody] CreateAppsTokenInfoRecord record, CancellationToken cancellationToken = default)
    {
        var createdAppsTokenInfo = await _serviceManager.AppsTokenInfos.CreateAsync(record, cancellationToken);

        if (createdAppsTokenInfo.AppsTokenInfoId <= 0)
            throw new InvalidCreateOperationException("Failed to create apps token info record.");

        return Ok(ApiResponseHelper.Created(createdAppsTokenInfo, "Apps token info created successfully."));
    }

    /// <summary>
    /// Updates an existing apps token info record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAppsTokenInfo)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAppsTokenInfoAsync([FromRoute] int key, [FromBody] UpdateAppsTokenInfoRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AppsTokenInfoId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAppsTokenInfoRecord));

        var updatedAppsTokenInfo = await _serviceManager.AppsTokenInfos.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAppsTokenInfo, "Apps token info updated successfully."));
    }

    /// <summary>
    /// Deletes an apps token info record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAppsTokenInfo)]
    public async Task<IActionResult> DeleteAppsTokenInfoAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAppsTokenInfoRecord(key);
        await _serviceManager.AppsTokenInfos.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Apps token info deleted successfully"));
    }

    /// <summary>
    /// Retrieves an apps token info by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAppsTokenInfo)]
    public async Task<IActionResult> AppsTokenInfoAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var appsTokenInfo = await _serviceManager.AppsTokenInfos.AppsTokenInfoAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(appsTokenInfo, "Apps token info retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all apps token infos.
    /// </summary>
    [HttpGet(RouteConstants.ReadAppsTokenInfos)]
    public async Task<IActionResult> AppsTokenInfosAsync(CancellationToken cancellationToken = default)
    {
        var appsTokenInfos = await _serviceManager.AppsTokenInfos.AppsTokenInfosAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!appsTokenInfos.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AppsTokenInfoDto>(), "No apps token infos found."));

        return Ok(ApiResponseHelper.Success(appsTokenInfos, "Apps token infos retrieved successfully"));
    }
}
