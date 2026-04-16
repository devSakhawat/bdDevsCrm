using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.DMS;
using bdDevs.Shared.Records.DMS;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;

namespace Presentation.Controllers.DMS;

/// <summary>
/// DMS File Update History management endpoints.
/// </summary>
[AuthorizeUser]
public class DmsFileUpdateHistoryController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public DmsFileUpdateHistoryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all file update histories for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.DmsFileUpdateHistoryDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> FileUpdateHistoriesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var fileUpdateHistories = await _serviceManager.DmsFileUpdateHistories.FileUpdateHistoriesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!fileUpdateHistories.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DmsFileUpdateHistoryDDLDto>(), "No file update histories found."));

        return Ok(ApiResponseHelper.Success(fileUpdateHistories, "File update histories retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of file update histories.
    /// </summary>
    [HttpPost(RouteConstants.DmsFileUpdateHistorySummary)]
    public async Task<IActionResult> FileUpdateHistorySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.DmsFileUpdateHistories.FileUpdateHistoriesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<DmsFileUpdateHistoryDto>(), "No file update histories found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "File update history summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new file update history record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateDmsFileUpdateHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateFileUpdateHistoryAsync([FromBody] CreateDmsFileUpdateHistoryRecord record, CancellationToken cancellationToken = default)
    {
        var createdFileUpdateHistory = await _serviceManager.DmsFileUpdateHistories.CreateAsync(record, cancellationToken);

        if (createdFileUpdateHistory.Id <= 0)
            throw new InvalidCreateOperationException("Failed to create file update history record.");

        return Ok(ApiResponseHelper.Created(createdFileUpdateHistory, "File update history created successfully."));
    }

    /// <summary>
    /// Updates an existing file update history record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateDmsFileUpdateHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateFileUpdateHistoryAsync([FromRoute] int key, [FromBody] UpdateDmsFileUpdateHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.Id)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateDmsFileUpdateHistoryRecord));

        var updatedFileUpdateHistory = await _serviceManager.DmsFileUpdateHistories.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedFileUpdateHistory, "File update history updated successfully."));
    }

    /// <summary>
    /// Deletes a file update history record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteDmsFileUpdateHistory)]
    public async Task<IActionResult> DeleteFileUpdateHistoryAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteDmsFileUpdateHistoryRecord(key);
        await _serviceManager.DmsFileUpdateHistories.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("File update history deleted successfully"));
    }

    /// <summary>
    /// Retrieves a file update history by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadDmsFileUpdateHistory)]
    public async Task<IActionResult> FileUpdateHistoryAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var fileUpdateHistory = await _serviceManager.DmsFileUpdateHistories.FileUpdateHistoryAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(fileUpdateHistory, "File update history retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all file update histories.
    /// </summary>
    [HttpGet(RouteConstants.ReadDmsFileUpdateHistories)]
    public async Task<IActionResult> FileUpdateHistoriesAsync(CancellationToken cancellationToken = default)
    {
        var fileUpdateHistories = await _serviceManager.DmsFileUpdateHistories.FileUpdateHistoriesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!fileUpdateHistories.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DmsFileUpdateHistoryDto>(), "No file update histories found."));

        return Ok(ApiResponseHelper.Success(fileUpdateHistories, "File update histories retrieved successfully"));
    }

    /// <summary>
    /// Retrieves file update histories by entity ID.
    /// </summary>
    [HttpGet(RouteConstants.DmsFileUpdateHistoriesByEntity)]
    public async Task<IActionResult> FileUpdateHistoriesByEntityAsync([FromRoute] string entityId, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(entityId))
            throw new IdParametersBadRequestException();

        var fileUpdateHistories = await _serviceManager.DmsFileUpdateHistories.FileUpdateHistoriesByEntityAsync(entityId, trackChanges: false, cancellationToken: cancellationToken);

        if (!fileUpdateHistories.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<DmsFileUpdateHistoryDto>(), "No file update histories found for this entity."));

        return Ok(ApiResponseHelper.Success(fileUpdateHistories, "File update histories retrieved successfully"));
    }
}
