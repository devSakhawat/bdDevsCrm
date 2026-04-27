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
public class CrmVisaApplicationController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmVisaApplicationController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmVisaApplicationSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaApplications.VisaApplicationsSummaryAsync(options, cancellationToken), "Visa applications retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmVisaApplication)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmVisaApplicationRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmVisaApplications.CreateAsync(record, cancellationToken), "Visa application created successfully."));

    [HttpPut(RouteConstants.UpdateCrmVisaApplication)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmVisaApplicationRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.VisaApplicationId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmVisaApplicationRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmVisaApplications.UpdateAsync(record, false, cancellationToken), "Visa application updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmVisaApplication)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmVisaApplications.DeleteAsync(new DeleteCrmVisaApplicationRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Visa application deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmVisaApplication)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaApplications.VisaApplicationAsync(id, false, cancellationToken), "Visa application retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmVisaApplications)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaApplications.VisaApplicationsAsync(false, cancellationToken), "Visa applications retrieved successfully."));

    [HttpGet(RouteConstants.VisaApplicationsByApplicationId)]
    public async Task<IActionResult> ByApplicationAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaApplications.VisaApplicationsByApplicationIdAsync(applicationId, false, cancellationToken), "Visa applications retrieved successfully."));

    [HttpGet(RouteConstants.VisaApplicationsByStudentId)]
    public async Task<IActionResult> ByStudentAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmVisaApplications.VisaApplicationsByStudentIdAsync(studentId, false, cancellationToken), "Visa applications retrieved successfully."));

    [HttpPost(RouteConstants.CrmVisaApplicationStatusTransition)]
    public async Task<IActionResult> ChangeStatusAsync([FromBody] ChangeCrmVisaApplicationStatusRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Updated(await _serviceManager.CrmVisaApplications.ChangeStatusAsync(record, cancellationToken), "Visa application status updated successfully."));
}
