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
public class CrmVisaStatusHistoryController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmVisaStatusHistoryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmVisaStatusHistorySummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaStatusHistories.VisaStatusHistoriesSummaryAsync(options, cancellationToken), "Visa status histories retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmVisaStatusHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmVisaStatusHistoryRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmVisaStatusHistories.CreateAsync(record, cancellationToken), "Visa status history created successfully."));

    [HttpPut(RouteConstants.UpdateCrmVisaStatusHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmVisaStatusHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.VisaStatusHistoryId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmVisaStatusHistoryRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmVisaStatusHistories.UpdateAsync(record, false, cancellationToken), "Visa status history updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmVisaStatusHistory)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmVisaStatusHistories.DeleteAsync(new DeleteCrmVisaStatusHistoryRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Visa status history deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmVisaStatusHistory)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaStatusHistories.VisaStatusHistoryAsync(id, false, cancellationToken), "Visa status history retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmVisaStatusHistories)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaStatusHistories.VisaStatusHistoriesAsync(false, cancellationToken), "Visa status histories retrieved successfully."));

    [HttpGet(RouteConstants.VisaStatusHistoriesByVisaApplicationId)]
    public async Task<IActionResult> ByVisaAsync([FromRoute] int visaApplicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaStatusHistories.VisaStatusHistoriesByVisaApplicationIdAsync(visaApplicationId, false, cancellationToken), "Visa status histories retrieved successfully."));
}
