using Application.Shared.Grid;
using bdDevs.Shared;
using bdDevs.Shared.Constants;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Services;
using Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Presentation.ActionFilters;
using Presentation.AuthorizeAttributes;

namespace Presentation.Controllers.CRM;

[AuthorizeUser]
public class CrmApplicationConditionController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmApplicationConditionController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmApplicationConditionSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationConditions.ApplicationConditionsSummaryAsync(options, cancellationToken), "Application conditions retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmApplicationCondition)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmApplicationConditionRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmApplicationConditions.CreateAsync(record, cancellationToken), "Application condition created successfully."));

    [HttpPut(RouteConstants.UpdateCrmApplicationCondition)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmApplicationConditionRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApplicationConditionId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmApplicationConditionRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmApplicationConditions.UpdateAsync(record, false, cancellationToken), "Application condition updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmApplicationCondition)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmApplicationConditions.DeleteAsync(new DeleteCrmApplicationConditionRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Application condition deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmApplicationCondition)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationConditions.ApplicationConditionAsync(id, false, cancellationToken), "Application condition retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmApplicationConditions)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationConditions.ApplicationConditionsAsync(false, cancellationToken), "Application conditions retrieved successfully."));

    [HttpGet(RouteConstants.ApplicationConditionsByApplicationId)]
    public async Task<IActionResult> ByApplicationAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationConditions.ApplicationConditionsByApplicationIdAsync(applicationId, false, cancellationToken), "Application conditions retrieved successfully."));

    [HttpPost(RouteConstants.CrmApplicationConditionStatusTransition)]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmApplicationConditionStatusRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Updated(await _serviceManager.CrmApplicationConditions.ChangeStatusAsync(record, cancellationToken), "Application condition status updated successfully."));
}
