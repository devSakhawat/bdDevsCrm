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

[AuthorizeUser]
public class CrmFollowUpController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmFollowUpController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpPost(RouteConstants.CrmFollowUpSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summaryGrid = await _serviceManager.CrmFollowUps.FollowUpsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmFollowUp)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmFollowUpRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmFollowUps.CreateAsync(record, cancellationToken);
        if (created.FollowUpId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmFollowUp)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmFollowUpRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.FollowUpId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmFollowUpRecord));
        var updated = await _serviceManager.CrmFollowUps.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpPost(RouteConstants.CrmFollowUpStatusTransition)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmFollowUpStatusRecord record, CancellationToken cancellationToken = default)
    {
        var updated = await _serviceManager.CrmFollowUps.ChangeStatusAsync(record, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Follow-up state updated successfully."));
    }

    [HttpPost(RouteConstants.CrmProcessOverdueFollowUps)]
    public async Task<IActionResult> ProcessOverdueAsync(CancellationToken cancellationToken = default)
    {
        var affected = await _serviceManager.CrmFollowUps.ProcessOverdueFollowUpsAsync(cancellationToken);
        return Ok(ApiResponseHelper.Success(affected, "Overdue follow-up job completed successfully."));
    }

    [HttpPost(RouteConstants.CrmMarkUnresponsiveLeads)]
    public async Task<IActionResult> ProcessUnresponsiveAsync(CancellationToken cancellationToken = default)
    {
        var affected = await _serviceManager.CrmFollowUps.ProcessUnresponsiveLeadsAsync(cancellationToken);
        return Ok(ApiResponseHelper.Success(affected, "Unresponsive lead job completed successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmFollowUp)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmFollowUps.DeleteAsync(new DeleteCrmFollowUpRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmFollowUp)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmFollowUps.FollowUpAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmFollowUps)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmFollowUps.FollowUpsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmFollowUpsByLeadId)]
    public async Task<IActionResult> GetByLeadIdAsync([FromRoute] int leadId, CancellationToken cancellationToken = default)
    {
        if (leadId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmFollowUps.FollowUpsByLeadIdAsync(leadId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmFollowUpDDL)]
    public async Task<IActionResult> GetForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmFollowUps.FollowUpForDDLAsync(cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
