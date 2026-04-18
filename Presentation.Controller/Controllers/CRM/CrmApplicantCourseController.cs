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
using bdDevs.Shared.Extensions;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Presentation.Controllers.CRM;

/// <summary>
/// CRM Applicant Course management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmApplicantCourseController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmApplicantCourseController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves paginated summary grid of applicant courses.
    /// </summary>
    [HttpPost(RouteConstants.CrmApplicantCourseSummary)]
    public async Task<IActionResult> ApplicantCourseSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.ApplicantCourses.ApplicantCoursesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<ApplicantCourseDto>(), "No applicant courses found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Applicant course summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new applicant course record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmApplicantCourse)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateApplicantCourseAsync([FromBody] CreateCrmApplicantCourseRecord record, CancellationToken cancellationToken = default)
    {
        var dto = record.MapTo<ApplicantCourseDto>();
        var currentUser = await GetCurrentUserAsync();

        var createdApplicantCourse = await _serviceManager.ApplicantCourses.CreateApplicantCourseAsync(dto, currentUser, cancellationToken);

        if (createdApplicantCourse.ApplicantCourseId <= 0)
            throw new InvalidCreateOperationException("Failed to create applicant course record.");

        return Ok(ApiResponseHelper.Created(createdApplicantCourse, "Applicant course created successfully."));
    }

    /// <summary>
    /// Updates an existing applicant course record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmApplicantCourse)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateApplicantCourseAsync([FromRoute] int key, [FromBody] UpdateCrmApplicantCourseRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.ApplicantCourseId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmApplicantCourseRecord));

        var dto = record.MapTo<ApplicantCourseDto>();
        var updatedApplicantCourse = await _serviceManager.ApplicantCourses.UpdateApplicantCourseAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedApplicantCourse, "Applicant course updated successfully."));
    }

    /// <summary>
    /// Deletes an applicant course record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmApplicantCourse)]
    public async Task<IActionResult> DeleteApplicantCourseAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmApplicantCourseRecord(key);
        var dto = new ApplicantCourseDto { ApplicantCourseId = key };
        await _serviceManager.ApplicantCourses.DeleteApplicantCourseAsync(key, dto, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Applicant course deleted successfully"));
    }

    /// <summary>
    /// Retrieves an applicant course by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmApplicantCourse)]
    public async Task<IActionResult> ApplicantCourseAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var applicantCourse = await _serviceManager.ApplicantCourses.ApplicantCourseAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(applicantCourse, "Applicant course retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all applicant courses.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmApplicantCourses)]
    public async Task<IActionResult> ApplicantCoursesAsync(CancellationToken cancellationToken = default)
    {
        var applicantCourses = await _serviceManager.ApplicantCourses.ApplicantCoursesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!applicantCourses.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApplicantCourseDto>(), "No applicant courses found."));

        return Ok(ApiResponseHelper.Success(applicantCourses, "Applicant courses retrieved successfully"));
    }

    /// <summary>
    /// Retrieves applicant courses by application ID.
    /// </summary>
    [HttpGet(RouteConstants.CrmApplicantCoursesByApplicationId)]
    public async Task<IActionResult> ApplicantCoursesByApplicationIdAsync([FromRoute] int applicationId, CancellationToken cancellationToken = default)
    {
        if (applicationId <= 0)
            throw new IdParametersBadRequestException();

        var applicantCourses = await _serviceManager.ApplicantCourses.ApplicantCoursesByApplicationIdAsync(applicationId, trackChanges: false, cancellationToken: cancellationToken);

        if (!applicantCourses.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<ApplicantCourseDto>(), "No applicant courses found for this application."));

        return Ok(ApiResponseHelper.Success(applicantCourses, "Applicant courses retrieved successfully"));
    }

    private async Task<UsersDto> GetCurrentUserAsync()
    {
        var userId = User?.FindFirst("UserId")?.Value;
        if (string.IsNullOrEmpty(userId) || !int.TryParse(userId, out int parsedUserId))
        {
            return new UsersDto { UserId = 1, Username = "system" };
        }
        return new UsersDto { UserId = parsedUserId };
    }
}
