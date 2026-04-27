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
public class CrmStudentStatusHistoryController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmStudentStatusHistoryController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpPost(RouteConstants.CrmStudentStatusHistorySummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmStudentStatusHistories.StudentStatusHistoriesSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmStudentStatusHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmStudentStatusHistoryRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmStudentStatusHistories.CreateAsync(record, cancellationToken);
        if (created.StudentStatusHistoryId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmStudentStatusHistory)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmStudentStatusHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.StudentStatusHistoryId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmStudentStatusHistoryRecord));
        var updated = await _serviceManager.CrmStudentStatusHistories.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmStudentStatusHistory)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmStudentStatusHistories.DeleteAsync(new DeleteCrmStudentStatusHistoryRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadStudentStatusHistories)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmStudentStatusHistories.StudentStatusHistoriesAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmStudentStatusHistory)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmStudentStatusHistories.StudentStatusHistoryAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.StudentStatusHistoriesByStudentId)]
    public async Task<IActionResult> GetByParentAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
    {
        if (studentId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmStudentStatusHistories.StudentStatusHistoriesByStudentIdAsync(studentId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

}
