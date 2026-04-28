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
public class CrmCommissionTypeController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCommissionTypeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpGet(RouteConstants.CrmCommissionTypeDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCommissionTypes.CommissionTypeForDDLAsync(cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCommissionTypeDDLDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CrmCommissionTypeSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summary = await _serviceManager.CrmCommissionTypes.CommissionTypeSummaryAsync(options, cancellationToken);
        if (!summary.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmCommissionTypeDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmCommissionType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCommissionTypeRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmCommissionTypes.CreateAsync(record, cancellationToken);
        if (created.CommissionTypeId <= 0)
            throw new InvalidCreateOperationException("Failed to create record.");

        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmCommissionType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCommissionTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CommissionTypeId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCommissionTypeRecord));

        var updated = await _serviceManager.CrmCommissionTypes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmCommissionType)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmCommissionTypeRecord(key);
        await _serviceManager.CrmCommissionTypes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCommissionTypes)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var record = await _serviceManager.CrmCommissionTypes.CommissionTypeAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCommissionTypes)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCommissionTypes.CommissionTypesAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCommissionTypeDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
