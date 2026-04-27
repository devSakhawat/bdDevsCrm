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
public class CrmDocumentVerificationHistoryController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmDocumentVerificationHistoryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmDocumentVerificationHistorySummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmDocumentVerificationHistories.DocumentVerificationHistoriesSummaryAsync(options, cancellationToken), "Document verification histories retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmDocumentVerificationHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmDocumentVerificationHistoryRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmDocumentVerificationHistories.CreateAsync(record, cancellationToken), "Document verification history created successfully."));

    [HttpPut(RouteConstants.UpdateCrmDocumentVerificationHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmDocumentVerificationHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.DocumentVerificationHistoryId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmDocumentVerificationHistoryRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmDocumentVerificationHistories.UpdateAsync(record, false, cancellationToken), "Document verification history updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmDocumentVerificationHistory)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmDocumentVerificationHistories.DeleteAsync(new DeleteCrmDocumentVerificationHistoryRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Document verification history deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmDocumentVerificationHistory)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmDocumentVerificationHistories.DocumentVerificationHistoryAsync(id, false, cancellationToken), "Document verification history retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmDocumentVerificationHistories)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmDocumentVerificationHistories.DocumentVerificationHistoriesAsync(false, cancellationToken), "Document verification histories retrieved successfully."));

    [HttpGet(RouteConstants.DocumentVerificationHistoriesByDocumentId)]
    public async Task<IActionResult> ByDocumentAsync([FromRoute] int documentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmDocumentVerificationHistories.DocumentVerificationHistoriesByDocumentIdAsync(documentId, false, cancellationToken), "Document verification histories retrieved successfully."));
}
