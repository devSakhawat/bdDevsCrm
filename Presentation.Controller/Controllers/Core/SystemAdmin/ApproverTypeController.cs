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
/// ApproverType management endpoints.
/// </summary>
[AuthorizeUser]
public class ApproverTypeController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public ApproverTypeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all approver types for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.ApproverTypeDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ApproverTypesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var approverTypes = await _serviceManager.ApproverType.ApproverTypesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverTypes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverTypeDDLDto>(), "No approver types found."));

        return Ok(ApiResponseHelper.Success(approverTypes, "Approver types retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of approver types.
    /// </summary>
    [HttpPost(RouteConstants.ApproverTypeSummary)]
    public async Task<IActionResult> ApproverTypeSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.ApproverType.ApproverTypesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ApproverTypeDto>(), "No approver types found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Approver type summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new approver type record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateApproverType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateApproverTypeAsync([FromBody] CreateApproverTypeRecord record, CancellationToken cancellationToken = default)
    {
        var createdApproverType = await _serviceManager.ApproverType.CreateAsync(record, cancellationToken);

        if (createdApproverType.ApproverTypeId <= 0)
            throw new InvalidCreateOperationException("Failed to create approver type record.");

        return Ok(ApiResponseHelper.Created(createdApproverType, "Approver type created successfully."));
    }

    /// <summary>
    /// Updates an existing approver type record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateApproverType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateApproverTypeAsync([FromRoute] int key, [FromBody] UpdateApproverTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApproverTypeId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateApproverTypeRecord));

        var updatedApproverType = await _serviceManager.ApproverType.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedApproverType, "Approver type updated successfully."));
    }

    /// <summary>
    /// Deletes an approver type record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteApproverType)]
    public async Task<IActionResult> DeleteApproverTypeAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteApproverTypeRecord(key);
        await _serviceManager.ApproverType.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Approver type deleted successfully"));
    }

    /// <summary>
    /// Retrieves an approver type by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverType)]
    public async Task<IActionResult> ApproverTypeAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var approverType = await _serviceManager.ApproverType.ApproverTypeAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(approverType, "Approver type retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all approver types.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverTypes)]
    public async Task<IActionResult> ApproverTypesAsync(CancellationToken cancellationToken = default)
    {
        var approverTypes = await _serviceManager.ApproverType.ApproverTypesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverTypes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverTypeDto>(), "No approver types found."));

        return Ok(ApiResponseHelper.Success(approverTypes, "Approver types retrieved successfully"));
    }
}
