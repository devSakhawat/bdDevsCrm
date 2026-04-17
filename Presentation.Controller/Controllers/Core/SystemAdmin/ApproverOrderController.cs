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
/// ApproverOrder management endpoints.
/// </summary>
[AuthorizeUser]
public class ApproverOrderController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public ApproverOrderController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all approver orders for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.ApproverOrderDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ApproverOrdersForDDLAsync(CancellationToken cancellationToken = default)
    {
        var approverOrders = await _serviceManager.ApproverOrders.ApproverOrdersForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverOrders.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverOrderDDLDto>(), "No approver orders found."));

        return Ok(ApiResponseHelper.Success(approverOrders, "Approver orders retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of approver orders.
    /// </summary>
    [HttpPost(RouteConstants.ApproverOrderSummary)]
    public async Task<IActionResult> ApproverOrderSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.ApproverOrders.ApproverOrdersSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ApproverOrderDto>(), "No approver orders found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Approver order summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new approver order record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateApproverOrder)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateApproverOrderAsync([FromBody] CreateApproverOrderRecord record, CancellationToken cancellationToken = default)
    {
        var createdApproverOrder = await _serviceManager.ApproverOrders.CreateAsync(record, cancellationToken);

        if (createdApproverOrder.ApproverOrderId <= 0)
            throw new InvalidCreateOperationException("Failed to create approver order record.");

        return Ok(ApiResponseHelper.Created(createdApproverOrder, "Approver order created successfully."));
    }

    /// <summary>
    /// Updates an existing approver order record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateApproverOrder)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateApproverOrderAsync([FromRoute] int key, [FromBody] UpdateApproverOrderRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApproverOrderId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateApproverOrderRecord));

        var updatedApproverOrder = await _serviceManager.ApproverOrders.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedApproverOrder, "Approver order updated successfully."));
    }

    /// <summary>
    /// Deletes an approver order record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteApproverOrder)]
    public async Task<IActionResult> DeleteApproverOrderAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteApproverOrderRecord(key);
        await _serviceManager.ApproverOrders.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Approver order deleted successfully"));
    }

    /// <summary>
    /// Retrieves an approver order by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverOrder)]
    public async Task<IActionResult> ApproverOrderAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var approverOrder = await _serviceManager.ApproverOrders.ApproverOrderAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(approverOrder, "Approver order retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all approver orders.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverOrders)]
    public async Task<IActionResult> ApproverOrdersAsync(CancellationToken cancellationToken = default)
    {
        var approverOrders = await _serviceManager.ApproverOrders.ApproverOrdersAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverOrders.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverOrderDto>(), "No approver orders found."));

        return Ok(ApiResponseHelper.Success(approverOrders, "Approver orders retrieved successfully"));
    }
}
