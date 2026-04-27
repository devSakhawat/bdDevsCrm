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

/// <summary>CrmLead management endpoints.</summary>
[AuthorizeUser]
public class CrmLeadController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmLeadController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>Retrieves paginated summary grid.</summary>
    [HttpPost(RouteConstants.CrmLeadSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summaryGrid = await _serviceManager.CrmLeads.LeadsSummaryAsync(options, cancellationToken);
        if (!summaryGrid.Items.Any()) return Ok(ApiResponseHelper.Success(new GridEntity<CrmLeadDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>Creates a new lead record.</summary>
    [HttpPost(RouteConstants.CreateCrmLead)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmLeadRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmLeads.CreateAsync(record, cancellationToken);
        if (created.LeadId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>Updates an existing lead record.</summary>
    [HttpPut(RouteConstants.UpdateCrmLead)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmLeadRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.LeadId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmLeadRecord));
        var updated = await _serviceManager.CrmLeads.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>Deletes a lead record.</summary>
    [HttpDelete(RouteConstants.DeleteCrmLead)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmLeadRecord(key);
        await _serviceManager.CrmLeads.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>Retrieves a lead record by ID.</summary>
    [HttpGet(RouteConstants.ReadCrmLead)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmLeads.LeadAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>Retrieves all lead records.</summary>
    [HttpGet(RouteConstants.ReadCrmLeads)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmLeads.LeadsAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmLeadDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    /// <summary>Retrieves leads for dropdown list.</summary>
    [HttpGet(RouteConstants.CrmLeadDDL)]
    public async Task<IActionResult> GetForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmLeads.LeadForDDLAsync(cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmLeadDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
