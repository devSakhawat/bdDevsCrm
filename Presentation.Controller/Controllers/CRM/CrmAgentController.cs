using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.AuthorizeAttributes;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmAgentController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmAgentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpGet(RouteConstants.CrmAgentDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmAgents.AgentForDDLAsync(cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmAgentDDLDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CrmAgentSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summary = await _serviceManager.CrmAgents.AgentSummaryAsync(options, cancellationToken);
        if (!summary.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmAgentDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmAgent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmAgentRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmAgents.CreateAsync(record, cancellationToken);
        if (created.AgentId <= 0)
            throw new InvalidCreateOperationException("Failed to create record.");

        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmAgent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmAgentRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AgentId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmAgentRecord));

        var updated = await _serviceManager.CrmAgents.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmAgent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmAgentRecord(key);
        await _serviceManager.CrmAgents.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmAgent)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var record = await _serviceManager.CrmAgents.AgentAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmAgents)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmAgents.AgentsAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmAgentDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
