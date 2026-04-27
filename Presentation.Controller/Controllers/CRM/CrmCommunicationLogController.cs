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
public class CrmCommunicationLogController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCommunicationLogController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmCommunicationLogSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommunicationLogs.CommunicationLogsSummaryAsync(options, cancellationToken), "Communication logs retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmCommunicationLog)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCommunicationLogRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmCommunicationLogs.CreateAsync(record, cancellationToken), "Communication log created successfully."));

    [HttpPut(RouteConstants.UpdateCrmCommunicationLog)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCommunicationLogRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CommunicationLogId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCommunicationLogRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmCommunicationLogs.UpdateAsync(record, false, cancellationToken), "Communication log updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmCommunicationLog)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmCommunicationLogs.DeleteAsync(new DeleteCrmCommunicationLogRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Communication log deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmCommunicationLog)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommunicationLogs.CommunicationLogAsync(id, false, cancellationToken), "Communication log retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmCommunicationLogs)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommunicationLogs.CommunicationLogsAsync(false, cancellationToken), "Communication logs retrieved successfully."));

    [HttpGet(RouteConstants.CrmCommunicationLogsByEntity)]
    public async Task<IActionResult> ByEntityAsync([FromRoute] byte entityType, [FromRoute] int entityId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmCommunicationLogs.CommunicationLogsByEntityAsync(entityType, entityId, false, cancellationToken), "Communication logs retrieved successfully."));
}
