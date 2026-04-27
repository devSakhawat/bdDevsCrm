using Presentation.AuthorizeAttributes;
using Domain.Contracts.Services;
using bdDevs.Shared;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Exceptions;
using bdDevs.Shared.Constants;
using Application.Shared.Grid;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmAgentLeadController : BaseApiController
{
    public CrmAgentLeadController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmAgentLeadSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmAgentLeads.AgentLeadsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmAgentLead)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmAgentLeadRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmAgentLeads.CreateAsync(record, cancellationToken);
        if (created.AgentLeadId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmAgentLead)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmAgentLeadRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AgentLeadId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmAgentLeadRecord));
        var updated = await _serviceManager.CrmAgentLeads.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmAgentLead)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmAgentLeads.DeleteAsync(new DeleteCrmAgentLeadRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmAgentLead)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmAgentLeads.AgentLeadAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmAgentLeads)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmAgentLeads.AgentLeadsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmAgentLeadByLeadId)]
    public async Task<IActionResult> GetByLeadIdAsync([FromRoute] int leadId, CancellationToken cancellationToken = default)
    {
        if (leadId <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmAgentLeads.AgentLeadByLeadIdAsync(leadId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmAgentLeadsByAgentId)]
    public async Task<IActionResult> GetByAgentIdAsync([FromRoute] int agentId, CancellationToken cancellationToken = default)
    {
        if (agentId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmAgentLeads.AgentLeadsByAgentIdAsync(agentId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
