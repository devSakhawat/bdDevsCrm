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
/// AssignApprover management endpoints.
/// </summary>
[AuthorizeUser]
public class AssignApproverController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public AssignApproverController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all assign approvers for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.AssignApproverDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> AssignApproversForDDLAsync(CancellationToken cancellationToken = default)
    {
        var assignApprovers = await _serviceManager.AssignApprover.AssignApproversForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!assignApprovers.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AssignApproverDDLDto>(), "No assign approvers found."));

        return Ok(ApiResponseHelper.Success(assignApprovers, "Assign approvers retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of assign approvers.
    /// </summary>
    [HttpPost(RouteConstants.AssignApproverSummary)]
    public async Task<IActionResult> AssignApproverSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.AssignApprover.AssignApproversSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<AssignApproverDto>(), "No assign approvers found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Assign approver summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new assign approver record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateAssignApprover)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAssignApproverAsync([FromBody] CreateAssignApproverRecord record, CancellationToken cancellationToken = default)
    {
        var createdAssignApprover = await _serviceManager.AssignApprover.CreateAsync(record, cancellationToken);

        if (createdAssignApprover.AssignApproverId <= 0)
            throw new InvalidCreateOperationException("Failed to create assign approver record.");

        return Ok(ApiResponseHelper.Created(createdAssignApprover, "Assign approver created successfully."));
    }

    /// <summary>
    /// Updates an existing assign approver record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateAssignApprover)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAssignApproverAsync([FromRoute] int key, [FromBody] UpdateAssignApproverRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AssignApproverId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateAssignApproverRecord));

        var updatedAssignApprover = await _serviceManager.AssignApprover.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedAssignApprover, "Assign approver updated successfully."));
    }

    /// <summary>
    /// Deletes an assign approver record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteAssignApprover)]
    public async Task<IActionResult> DeleteAssignApproverAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteAssignApproverRecord(key);
        await _serviceManager.AssignApprover.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Assign approver deleted successfully"));
    }

    /// <summary>
    /// Retrieves an assign approver by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadAssignApprover)]
    public async Task<IActionResult> AssignApproverAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var assignApprover = await _serviceManager.AssignApprover.AssignApproverAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(assignApprover, "Assign approver retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all assign approvers.
    /// </summary>
    [HttpGet(RouteConstants.ReadAssignApprovers)]
    public async Task<IActionResult> AssignApproversAsync(CancellationToken cancellationToken = default)
    {
        var assignApprovers = await _serviceManager.AssignApprover.AssignApproversAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!assignApprovers.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<AssignApproverDto>(), "No assign approvers found."));

        return Ok(ApiResponseHelper.Success(assignApprovers, "Assign approvers retrieved successfully"));
    }
}
