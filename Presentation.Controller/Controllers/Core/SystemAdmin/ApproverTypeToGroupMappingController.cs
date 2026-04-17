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
/// ApproverTypeToGroupMapping management endpoints.
/// </summary>
[AuthorizeUser]
public class ApproverTypeToGroupMappingController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public ApproverTypeToGroupMappingController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all approver type to group mappings for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.ApproverTypeToGroupMappingDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ApproverTypeToGroupMappingsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var approverTypeToGroupMappings = await _serviceManager.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingsForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverTypeToGroupMappings.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverTypeToGroupMappingDDLDto>(), "No approver type to group mappings found."));

        return Ok(ApiResponseHelper.Success(approverTypeToGroupMappings, "Approver type to group mappings retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of approver type to group mappings.
    /// </summary>
    [HttpPost(RouteConstants.ApproverTypeToGroupMappingSummary)]
    public async Task<IActionResult> ApproverTypeToGroupMappingSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingsSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ApproverTypeToGroupMappingDto>(), "No approver type to group mappings found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Approver type to group mapping summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new approver type to group mapping record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateApproverTypeToGroupMapping)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateApproverTypeToGroupMappingAsync([FromBody] CreateApproverTypeToGroupMappingRecord record, CancellationToken cancellationToken = default)
    {
        var createdApproverTypeToGroupMapping = await _serviceManager.ApproverTypeToGroupMappings.CreateAsync(record, cancellationToken);

        if (createdApproverTypeToGroupMapping.ApproverTypeMapId <= 0)
            throw new InvalidCreateOperationException("Failed to create approver type to group mapping record.");

        return Ok(ApiResponseHelper.Created(createdApproverTypeToGroupMapping, "Approver type to group mapping created successfully."));
    }

    /// <summary>
    /// Updates an existing approver type to group mapping record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateApproverTypeToGroupMapping)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateApproverTypeToGroupMappingAsync([FromRoute] int key, [FromBody] UpdateApproverTypeToGroupMappingRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApproverTypeMapId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateApproverTypeToGroupMappingRecord));

        var updatedApproverTypeToGroupMapping = await _serviceManager.ApproverTypeToGroupMappings.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedApproverTypeToGroupMapping, "Approver type to group mapping updated successfully."));
    }

    /// <summary>
    /// Deletes an approver type to group mapping record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteApproverTypeToGroupMapping)]
    public async Task<IActionResult> DeleteApproverTypeToGroupMappingAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteApproverTypeToGroupMappingRecord(key);
        await _serviceManager.ApproverTypeToGroupMappings.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Approver type to group mapping deleted successfully"));
    }

    /// <summary>
    /// Retrieves an approver type to group mapping by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverTypeToGroupMapping)]
    public async Task<IActionResult> ApproverTypeToGroupMappingAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var approverTypeToGroupMapping = await _serviceManager.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(approverTypeToGroupMapping, "Approver type to group mapping retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all approver type to group mappings.
    /// </summary>
    [HttpGet(RouteConstants.ReadApproverTypeToGroupMappings)]
    public async Task<IActionResult> ApproverTypeToGroupMappingsAsync(CancellationToken cancellationToken = default)
    {
        var approverTypeToGroupMappings = await _serviceManager.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingsAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!approverTypeToGroupMappings.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApproverTypeToGroupMappingDto>(), "No approver type to group mappings found."));

        return Ok(ApiResponseHelper.Success(approverTypeToGroupMappings, "Approver type to group mappings retrieved successfully"));
    }
}
