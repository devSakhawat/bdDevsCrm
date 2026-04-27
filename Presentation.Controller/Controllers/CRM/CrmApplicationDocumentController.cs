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
public class CrmApplicationDocumentController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmApplicationDocumentController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmApplicationDocumentSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationDocuments.ApplicationDocumentsSummaryAsync(options, cancellationToken), "Application documents retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmApplicationDocument)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmApplicationDocumentRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmApplicationDocuments.CreateAsync(record, cancellationToken), "Application document created successfully."));

    [HttpPut(RouteConstants.UpdateCrmApplicationDocument)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmApplicationDocumentRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApplicationDocumentId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmApplicationDocumentRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmApplicationDocuments.UpdateAsync(record, false, cancellationToken), "Application document updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmApplicationDocument)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmApplicationDocuments.DeleteAsync(new DeleteCrmApplicationDocumentRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Application document deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmApplicationDocument)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationDocuments.ApplicationDocumentAsync(id, false, cancellationToken), "Application document retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmApplicationDocuments)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationDocuments.ApplicationDocumentsAsync(false, cancellationToken), "Application documents retrieved successfully."));

    [HttpGet(RouteConstants.ApplicationDocumentsByApplicationId)]
    public async Task<IActionResult> ByApplicationAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplicationDocuments.ApplicationDocumentsByApplicationIdAsync(applicationId, false, cancellationToken), "Application documents retrieved successfully."));
}
