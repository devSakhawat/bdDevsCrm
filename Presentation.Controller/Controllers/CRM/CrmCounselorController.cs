using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;

namespace Presentation.Controllers.CRM;

/// <summary>CrmCounselor management endpoints.</summary>
[AuthorizeUser]
public class CrmCounselorController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCounselorController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>Retrieves paginated summary grid.</summary>
    [HttpPost(RouteConstants.CrmCounselorSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summaryGrid = await _serviceManager.CrmCounselors.CounselorsSummaryAsync(options, cancellationToken);
        if (!summaryGrid.Items.Any()) return Ok(ApiResponseHelper.Success(new GridEntity<CrmCounselorDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>Creates a new counselor record.</summary>
    [HttpPost(RouteConstants.CreateCrmCounselor)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCounselorRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmCounselors.CreateAsync(record, cancellationToken);
        if (created.CounselorId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>Updates an existing counselor record.</summary>
    [HttpPut(RouteConstants.UpdateCrmCounselor)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCounselorRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CounselorId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCounselorRecord));
        var updated = await _serviceManager.CrmCounselors.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>Deletes a counselor record.</summary>
    [HttpDelete(RouteConstants.DeleteCrmCounselor)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmCounselorRecord(key);
        await _serviceManager.CrmCounselors.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>Retrieves a counselor record by ID.</summary>
    [HttpGet(RouteConstants.ReadCrmCounselor)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmCounselors.CounselorAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>Retrieves all counselor records.</summary>
    [HttpGet(RouteConstants.ReadCrmCounselors)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCounselors.CounselorsAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCounselorDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    /// <summary>Retrieves counselors for dropdown list.</summary>
    [HttpGet(RouteConstants.CrmCounselorDDL)]
    public async Task<IActionResult> GetForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCounselors.CounselorForDDLAsync(cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCounselorDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
