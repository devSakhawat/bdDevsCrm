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
public class CrmSystemConfigurationController : BaseApiController
{
    public CrmSystemConfigurationController(IServiceManager serviceManager) : base(serviceManager) { }

    [HttpPost(RouteConstants.CrmSystemConfigSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmSystemConfigurations.SystemConfigurationsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmSystemConfig)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmSystemConfigurationRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmSystemConfigurations.CreateAsync(record, cancellationToken);
        if (created.ConfigId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmSystemConfig)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmSystemConfigurationRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ConfigId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmSystemConfigurationRecord));
        var updated = await _serviceManager.CrmSystemConfigurations.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmSystemConfig)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmSystemConfigurations.DeleteAsync(new DeleteCrmSystemConfigurationRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmSystemConfig)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmSystemConfigurations.SystemConfigurationAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmSystemConfigs)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmSystemConfigurations.SystemConfigurationsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmSystemConfigByKey)]
    public async Task<IActionResult> GetByKeyAsync([FromRoute] string key, CancellationToken cancellationToken = default)
    {
        var record = await _serviceManager.CrmSystemConfigurations.SystemConfigurationByKeyAsync(key, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }
}
