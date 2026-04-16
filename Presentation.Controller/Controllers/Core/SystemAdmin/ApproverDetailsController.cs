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
/// ApproverDetails management endpoints.
/// </summary>
[AuthorizeUser]
public class ApproverDetailsController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public ApproverDetailsController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all approver details for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.ApproverDetailsDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ApproverDetailsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var approverDetails = await _serviceManager.ApproverDetails.ApproverDetailsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverDetails.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverDetailsDDLDto>(), "No approver details found."));

        return Ok(ApiResponseHelper.Success(approverDetails, "Approver details retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of approver details.
    /// </summary>
    [HttpPost(RouteConstants.ApproverDetailsSummary)]
    public async Task<IActionResult> ApproverDetailsSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.ApproverDetails.ApproverDetailsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ApproverDetailsDto>(), "No approver details found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Approver details summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new approver details record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateApproverDetails)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateApproverDetailsAsync([FromBody] CreateApproverDetailsRecord record, CancellationToken cancellationToken = default)
    {
        var createdApproverDetails = await _serviceManager.ApproverDetails.CreateAsync(record, cancellationToken);

        if (createdApproverDetails.RemarksId <= 0)
            throw new InvalidCreateOperationException("Failed to create approver details record.");

        return Ok(ApiResponseHelper.Created(createdApproverDetails, "Approver details created successfully."));
    }

    /// <summary>
    /// Updates an existing approver details record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateApproverDetails)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateApproverDetailsAsync([FromRoute] int key, [FromBody] UpdateApproverDetailsRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.RemarksId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateApproverDetailsRecord));

        var updatedApproverDetails = await _serviceManager.ApproverDetails.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedApproverDetails, "Approver details updated successfully."));
    }

    /// <summary>
    /// Deletes an approver details record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteApproverDetails)]
    public async Task<IActionResult> DeleteApproverDetailsAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteApproverDetailsRecord(key);
        await _serviceManager.ApproverDetails.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Approver details deleted successfully"));
    }

    /// <summary>
    /// Retrieves an approver details by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverDetail)]
    public async Task<IActionResult> ApproverDetailAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var approverDetail = await _serviceManager.ApproverDetails.ApproverDetailAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(approverDetail, "Approver details retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all approver details.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverDetails)]
    public async Task<IActionResult> ApproverDetailsAsync(CancellationToken cancellationToken = default)
    {
        var approverDetails = await _serviceManager.ApproverDetails.ApproverDetailsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverDetails.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverDetailsDto>(), "No approver details found."));

        return Ok(ApiResponseHelper.Success(approverDetails, "Approver details retrieved successfully"));
    }
}
