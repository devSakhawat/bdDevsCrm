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
public class CrmApplicationController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmApplicationController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmApplicationSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplications.ApplicationsSummaryAsync(options, cancellationToken), "Applications retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmApplication)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmApplicationRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmApplications.CreateAsync(record, cancellationToken), "Application created successfully."));

    [HttpPut(RouteConstants.UpdateCrmApplication)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmApplicationRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApplicationId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmApplicationRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmApplications.UpdateAsync(record, false, cancellationToken), "Application updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmApplication)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmApplications.DeleteAsync(new DeleteCrmApplicationRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Application deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmApplication)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplications.ApplicationAsync(id, false, cancellationToken), "Application retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmApplications)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplications.ApplicationsAsync(false, cancellationToken), "Applications retrieved successfully."));

    [HttpGet(RouteConstants.CrmApplicationsByStudentId)]
    public async Task<IActionResult> ByStudentAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplications.ApplicationsByStudentIdAsync(studentId, false, cancellationToken), "Applications retrieved successfully."));

    [HttpGet(RouteConstants.CrmApplicationBoard)]
    public async Task<IActionResult> BoardAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmApplications.ApplicationsBoardAsync(cancellationToken), "Application board retrieved successfully."));

    [HttpPost(RouteConstants.CrmApplicationStatusTransition)]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmApplicationStatusRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Updated(await _serviceManager.CrmApplications.ChangeStatusAsync(record, cancellationToken), "Application status updated successfully."));
}
