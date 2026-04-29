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
public class CrmCounsellingTypeController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCounsellingTypeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpGet(RouteConstants.CrmCounsellingTypeDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> ForDDLAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCounsellingTypes.CounsellingTypeForDDLAsync(cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCounsellingTypeDDLDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CrmCounsellingTypeSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summary = await _serviceManager.CrmCounsellingTypes.CounsellingTypeSummaryAsync(options, cancellationToken);
        if (!summary.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmCounsellingTypeDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(summary, "Records retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmCounsellingType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCounsellingTypeRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmCounsellingTypes.CreateAsync(record, cancellationToken);
        if (created.CounsellingTypeId <= 0)
            throw new InvalidCreateOperationException("Failed to create record.");

        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmCounsellingType)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCounsellingTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CounsellingTypeId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCounsellingTypeRecord));

        var updated = await _serviceManager.CrmCounsellingTypes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmCounsellingType)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmCounsellingTypeRecord(key);
        await _serviceManager.CrmCounsellingTypes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCounsellingType)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var record = await _serviceManager.CrmCounsellingTypes.CounsellingTypeAsync(id, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCounsellingTypes)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCounsellingTypes.CounsellingTypesAsync(trackChanges: false, cancellationToken: cancellationToken);
        if (!records.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCounsellingTypeDto>(), "No records found."));

        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }
}
