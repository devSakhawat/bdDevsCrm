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
public class CrmCounsellingSessionController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCounsellingSessionController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpPost(RouteConstants.CrmCounsellingSessionSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmCounsellingSessions.CounsellingSessionsSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmCounsellingSession)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmCounsellingSessionRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmCounsellingSessions.CreateAsync(record, cancellationToken);
        if (created.CounsellingSessionId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmCounsellingSession)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmCounsellingSessionRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CounsellingSessionId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCounsellingSessionRecord));
        var updated = await _serviceManager.CrmCounsellingSessions.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmCounsellingSession)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmCounsellingSessions.DeleteAsync(new DeleteCrmCounsellingSessionRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadCounsellingSessions)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmCounsellingSessions.CounsellingSessionsAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmCounsellingSession)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmCounsellingSessions.CounsellingSessionAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.CounsellingSessionsByLeadId)]
    public async Task<IActionResult> GetByParentAsync([FromRoute] int leadId, CancellationToken cancellationToken = default)
    {
        if (leadId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmCounsellingSessions.CounsellingSessionsByLeadIdAsync(leadId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.CrmCounsellingSessionEligibility)]
    public async Task<IActionResult> EligibleProgramsAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
    {
        if (studentId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmCounsellingSessions.EligibleProgramsAsync(studentId, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

}
