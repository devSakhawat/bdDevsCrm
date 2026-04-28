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
public class CrmStudentAcademicProfileController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmStudentAcademicProfileController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    [HttpPost(RouteConstants.CrmStudentAcademicProfileSummary)]
    public async Task<IActionResult> SummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null) throw new NullModelBadRequestException(nameof(GridOptions));
        var summary = await _serviceManager.CrmStudentAcademicProfiles.StudentAcademicProfilesSummaryAsync(options, cancellationToken);
        return Ok(ApiResponseHelper.Success(summary, "Summary retrieved successfully"));
    }

    [HttpPost(RouteConstants.CreateCrmStudentAcademicProfile)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateAsync([FromBody] CreateCrmStudentAcademicProfileRecord record, CancellationToken cancellationToken = default)
    {
        var created = await _serviceManager.CrmStudentAcademicProfiles.CreateAsync(record, cancellationToken);
        if (created.StudentAcademicProfileId <= 0) throw new InvalidCreateOperationException("Failed to create record.");
        return Ok(ApiResponseHelper.Created(created, "Record created successfully."));
    }

    [HttpPut(RouteConstants.UpdateCrmStudentAcademicProfile)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateAsync([FromRoute] int key, [FromBody] UpdateCrmStudentAcademicProfileRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.StudentAcademicProfileId) throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmStudentAcademicProfileRecord));
        var updated = await _serviceManager.CrmStudentAcademicProfiles.UpdateAsync(record, false, cancellationToken);
        return Ok(ApiResponseHelper.Updated(updated, "Record updated successfully."));
    }

    [HttpDelete(RouteConstants.DeleteCrmStudentAcademicProfile)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        await _serviceManager.CrmStudentAcademicProfiles.DeleteAsync(new DeleteCrmStudentAcademicProfileRecord(key), false, cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Record deleted successfully"));
    }

    [HttpGet(RouteConstants.ReadStudentAcademicProfiles)]
    public async Task<IActionResult> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var records = await _serviceManager.CrmStudentAcademicProfiles.StudentAcademicProfilesAsync(false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

    [HttpGet(RouteConstants.ReadCrmStudentAcademicProfile)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0) throw new IdParametersBadRequestException();
        var record = await _serviceManager.CrmStudentAcademicProfiles.StudentAcademicProfileAsync(id, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(record, "Record retrieved successfully"));
    }

    [HttpGet(RouteConstants.StudentAcademicProfilesByStudentId)]
    public async Task<IActionResult> GetByParentAsync([FromRoute] int studentId, CancellationToken cancellationToken = default)
    {
        if (studentId <= 0) throw new IdParametersBadRequestException();
        var records = await _serviceManager.CrmStudentAcademicProfiles.StudentAcademicProfilesByStudentIdAsync(studentId, false, cancellationToken);
        return Ok(ApiResponseHelper.Success(records, "Records retrieved successfully"));
    }

}
