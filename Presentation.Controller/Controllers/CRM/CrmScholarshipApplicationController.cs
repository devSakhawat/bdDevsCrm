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
public class CrmScholarshipApplicationController : BaseApiController
{
    private readonly IMemoryCache _cache;
    public CrmScholarshipApplicationController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager) => _cache = cache;

    [HttpPost(RouteConstants.CrmScholarshipApplicationSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        return Ok(ApiResponseHelper.Success(await _serviceManager.CrmScholarshipApplications.ScholarshipApplicationsSummaryAsync(options, cancellationToken), "Scholarship applications retrieved successfully."));
    }

    [HttpPost(RouteConstants.CreateCrmScholarshipApplication)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmScholarshipApplicationRecord record, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Created(await _serviceManager.CrmScholarshipApplications.CreateAsync(record, cancellationToken), "Scholarship application created successfully."));

    [HttpPut(RouteConstants.UpdateCrmScholarshipApplication)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmScholarshipApplicationRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ScholarshipApplicationId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmScholarshipApplicationRecord));
        return Ok(ApiResponseHelper.Updated(await _serviceManager.CrmScholarshipApplications.UpdateAsync(record, false, cancellationToken), "Scholarship application updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmScholarshipApplication)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmScholarshipApplications.DeleteAsync(new DeleteCrmScholarshipApplicationRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Scholarship application deleted successfully."));
    }

    [HttpGet(RouteConstants.ReadCrmScholarshipApplication)]
    public async Task<IActionResult> GetAsync([FromRoute] int id, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmScholarshipApplications.ScholarshipApplicationAsync(id, false, cancellationToken), "Scholarship application retrieved successfully."));

    [HttpGet(RouteConstants.ReadCrmScholarshipApplications)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmScholarshipApplications.ScholarshipApplicationsAsync(false, cancellationToken), "Scholarship applications retrieved successfully."));

    [HttpGet(RouteConstants.ScholarshipApplicationsByApplicationId)]
    public async Task<IActionResult> ByApplicationAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmScholarshipApplications.ScholarshipApplicationsByApplicationIdAsync(applicationId, false, cancellationToken), "Scholarship applications retrieved successfully."));

    [HttpGet(RouteConstants.CrmScholarshipCommissionImpact)]
    public async Task<IActionResult> CommissionImpactAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
        => Ok(ApiResponseHelper.Success(await _serviceManager.CrmScholarshipApplications.CommissionImpactAsync(applicationId, cancellationToken), "Scholarship commission impact retrieved successfully."));
}
