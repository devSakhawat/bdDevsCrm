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
/// ApproverHistory management endpoints.
/// </summary>
[AuthorizeUser]
public class ApproverHistoryController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public ApproverHistoryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all approver histories for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.ApproverHistoryDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ApproverHistoriesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var approverHistories = await _serviceManager.ApproverHistory.ApproverHistoriesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverHistories.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverHistoryDDLDto>(), "No approver histories found."));

        return Ok(ApiResponseHelper.Success(approverHistories, "Approver histories retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of approver histories.
    /// </summary>
    [HttpPost(RouteConstants.ApproverHistorySummary)]
    public async Task<IActionResult> ApproverHistorySummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.ApproverHistory.ApproverHistoriesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ApproverHistoryDto>(), "No approver histories found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Approver history summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new approver history record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateApproverHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateApproverHistoryAsync([FromBody] CreateApproverHistoryRecord record, CancellationToken cancellationToken = default)
    {
        var createdApproverHistory = await _serviceManager.ApproverHistory.CreateAsync(record, cancellationToken);

        if (createdApproverHistory.AssignApproverId <= 0)
            throw new InvalidCreateOperationException("Failed to create approver history record.");

        return Ok(ApiResponseHelper.Created(createdApproverHistory, "Approver history created successfully."));
    }

    /// <summary>
    /// Updates an existing approver history record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateApproverHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateApproverHistoryAsync([FromRoute] int key, [FromBody] UpdateApproverHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AssignApproverId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateApproverHistoryRecord));

        var updatedApproverHistory = await _serviceManager.ApproverHistory.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedApproverHistory, "Approver history updated successfully."));
    }

    /// <summary>
    /// Deletes an approver history record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteApproverHistory)]
    public async Task<IActionResult> DeleteApproverHistoryAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteApproverHistoryRecord(key);
        await _serviceManager.ApproverHistory.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Approver history deleted successfully"));
    }

    /// <summary>
    /// Retrieves an approver history by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverHistory)]
    public async Task<IActionResult> ApproverHistoryAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var approverHistory = await _serviceManager.ApproverHistory.ApproverHistoryAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(approverHistory, "Approver history retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all approver histories.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverHistories)]
    public async Task<IActionResult> ApproverHistoriesAsync(CancellationToken cancellationToken = default)
    {
        var approverHistories = await _serviceManager.ApproverHistory.ApproverHistoriesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverHistories.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverHistoryDto>(), "No approver histories found."));

        return Ok(ApiResponseHelper.Success(approverHistories, "Approver histories retrieved successfully"));
    }
}
