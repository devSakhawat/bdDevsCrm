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
public class CrmSessionProgramShortlistController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmSessionProgramShortlistController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpPost(RouteConstants.CrmSessionProgramShortlistSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmSessionProgramShortlists.SessionProgramShortlistsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmSessionProgramShortlist)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmSessionProgramShortlistRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmSessionProgramShortlists.CreateAsync(record, cancellationToken);
        if (created.SessionProgramShortlistId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmSessionProgramShortlist)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmSessionProgramShortlistRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.SessionProgramShortlistId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmSessionProgramShortlistRecord));
        var updated = await _serviceManager.CrmSessionProgramShortlists.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmSessionProgramShortlist)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmSessionProgramShortlists.DeleteAsync(new DeleteCrmSessionProgramShortlistRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadSessionProgramShortlists)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmSessionProgramShortlists.SessionProgramShortlistsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmSessionProgramShortlist)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmSessionProgramShortlists.SessionProgramShortlistAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.SessionProgramShortlistsBySessionId)]
    public async Task<IActionResult> GetByParentAsync([FromRoute] int sessionId, CancellationToken cancellationToken = default)
    {
        if (sessionId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmSessionProgramShortlists.SessionProgramShortlistsBySessionIdAsync(sessionId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

}
