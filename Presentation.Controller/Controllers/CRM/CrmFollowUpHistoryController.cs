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
public class CrmFollowUpHistoryController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmFollowUpHistoryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpPost(RouteConstants.CrmFollowUpHistorySummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmFollowUpHistories.FollowUpHistoriesSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmFollowUpHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmFollowUpHistoryRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmFollowUpHistories.CreateAsync(record, cancellationToken);
        if (created.FollowUpHistoryId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmFollowUpHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmFollowUpHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.FollowUpHistoryId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmFollowUpHistoryRecord));
        var updated = await _serviceManager.CrmFollowUpHistories.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmFollowUpHistory)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmFollowUpHistories.DeleteAsync(new DeleteCrmFollowUpHistoryRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadFollowUpHistories)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmFollowUpHistories.FollowUpHistoriesAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmFollowUpHistory)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmFollowUpHistories.FollowUpHistoryAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.FollowUpHistoriesByFollowUpId)]
    public async Task<IActionResult> GetByParentAsync([FromRoute] int followUpId, CancellationToken cancellationToken = default)
    {
        if (followUpId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmFollowUpHistories.FollowUpHistoriesByFollowUpIdAsync(followUpId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

}
