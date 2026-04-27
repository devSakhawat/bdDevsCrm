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

/// <summary>CrmEnquiry management endpoints.</summary>
[AuthorizeUser]
public class CrmEnquiryController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmEnquiryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>Retrieves paginated summary grid.</summary>
    [HttpPost(RouteConstants.CrmEnquirySummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summaryGrid = await _serviceManager.CrmEnquiries.EnquiriesSummaryAsync(options, cancellationToken);
        if (!summaryGrid.Items.Any()) return Ok(ApiResponseHelper.Success(new GridEntity<CrmEnquiryDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>Creates a new enquiry record.</summary>
    [HttpPost(RouteConstants.CreateCrmEnquiry)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmEnquiryRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmEnquiries.CreateAsync(record, cancellationToken);
        if (created.EnquiryId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>Updates an existing enquiry record.</summary>
    [HttpPut(RouteConstants.UpdateCrmEnquiry)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmEnquiryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.EnquiryId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmEnquiryRecord));
        var updated = await _serviceManager.CrmEnquiries.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>Deletes an enquiry record.</summary>
    [HttpDelete(RouteConstants.DeleteCrmEnquiry)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmEnquiryRecord(key);
        await _serviceManager.CrmEnquiries.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>Retrieves an enquiry record by ID.</summary>
    [HttpGet(RouteConstants.ReadCrmEnquiry)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmEnquiries.EnquiryAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>Retrieves all enquiry records.</summary>
    [HttpGet(RouteConstants.ReadCrmEnquiries)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmEnquiries.EnquiriesAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmEnquiryDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    /// <summary>Retrieves enquiries for dropdown list.</summary>
    [HttpGet(RouteConstants.CrmEnquiryDDL)]
    public async Task<IActionResult> GetForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmEnquiries.EnquiryForDDLAsync(cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmEnquiryDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
