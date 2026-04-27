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

/// <summary>CrmAgent management endpoints.</summary>
[AuthorizeUser]
public class CrmAgentController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmAgentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>Retrieves paginated summary grid.</summary>
    [HttpPost(RouteConstants.CrmAgentSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summaryGrid = await _serviceManager.CrmAgents.AgentsSummaryAsync(options, cancellationToken);
        if (!summaryGrid.Items.Any()) return Ok(ApiResponseHelper.Success(new GridEntity<CrmAgentDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(summaryGrid, "Summary retrieved successfully"));
    }

    /// <summary>Creates a new agent record.</summary>
    [HttpPost(RouteConstants.CreateCrmAgent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmAgentRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmAgents.CreateAsync(record, cancellationToken);
        if (created.AgentId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    /// <summary>Updates an existing agent record.</summary>
    [HttpPut(RouteConstants.UpdateCrmAgent)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmAgentRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.AgentId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmAgentRecord));
        var updated = await _serviceManager.CrmAgents.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    /// <summary>Deletes an agent record.</summary>
    [HttpDelete(RouteConstants.DeleteCrmAgent)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmAgentRecord(key);
        await _serviceManager.CrmAgents.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    /// <summary>Retrieves an agent record by ID.</summary>
    [HttpGet(RouteConstants.ReadCrmAgent)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmAgents.AgentAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    /// <summary>Retrieves all agent records.</summary>
    [HttpGet(RouteConstants.ReadCrmAgents)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmAgents.AgentsAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmAgentDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    /// <summary>Retrieves agents for dropdown list.</summary>
    [HttpGet(RouteConstants.CrmAgentDDL)]
    public async Task<IActionResult> GetForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmAgents.AgentForDDLAsync(cancellationToken: cancellationToken);
        if (!records.Any()) return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmAgentDto>(), "No records found."));
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
