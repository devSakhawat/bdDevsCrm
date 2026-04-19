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
/// CRM Course management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmCourseController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCourseController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all courses for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CrmCourseDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> CoursesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var courses = await _serviceManager.CrmCourses.CourseForDDLAsync(cancellationToken: cancellationToken);

        if (!courses.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCourseDDLDto>(), "No courses found."));

        return Ok(ApiResponseHelper.Success(courses, "Courses retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of courses.
    /// </summary>
    [HttpPost(RouteConstants.CrmCourseSummary)]
    public async Task<IActionResult> CourseSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.CrmCourses.CoursesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmCourseDto>(), "No courses found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Course summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new course record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmCourse)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCourseAsync([FromBody] CreateCrmCourseRecord record, CancellationToken cancellationToken = default)
    {
        var dto = record.MapTo<CrmCourseDto>();
        var currentUser = await GetCurrentUserAsync();

        //var createdCourse = await _serviceManager.CrmCourses.CreateCourseAsync(dto, currentUser, cancellationToken);
        var createdCourse = await _serviceManager.CrmCourses.CreateAsync(record, currentUser, cancellationToken);

        if (createdCourse.CourseId <= 0)
            throw new InvalidCreateOperationException("Failed to create course record.");

        return Ok(ApiResponseHelper.Created(createdCourse, "Course created successfully."));
    }

    /// <summary>
    /// Updates an existing course record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmCourse)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCourseAsync([FromRoute] int key, [FromBody] UpdateCrmCourseRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CourseId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCourseRecord));

        var dto = record.MapTo<CrmCourseDto>();
        var updatedCourse = await _serviceManager.CrmCourses.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedCourse, "Course updated successfully."));
    }

    /// <summary>
    /// Deletes a course record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmCourse)]
    public async Task<IActionResult> DeleteCourseAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmCourseRecord(key);
        await _serviceManager.CrmCourses.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Course deleted successfully"));
    }

    /// <summary>
    /// Retrieves a course by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmCourse)]
    public async Task<IActionResult> CourseAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var course = await _serviceManager.CrmCourses.CourseAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(course, "Course retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all courses.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmCourses)]
    public async Task<IActionResult> CoursesAsync(CancellationToken cancellationToken = default)
    {
        var courses = await _serviceManager.CrmCourses.CoursesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!courses.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCourseDto>(), "No courses found."));

        return Ok(ApiResponseHelper.Success(courses, "Courses retrieved successfully"));
    }

    /// <summary>
    /// Retrieves courses by institute ID.
    /// </summary>
    [HttpGet(RouteConstants.CrmCoursesByInstituteId)]
    public async Task<IActionResult> CoursesByInstituteIdAsync([FromRoute] int instituteId, CancellationToken cancellationToken = default)
    {
        if (instituteId <= 0)
            throw new IdParametersBadRequestException();

        var courses = await _serviceManager.CrmCourses.CoursesByInstituteIdAsync(instituteId, trackChanges: false, cancellationToken: cancellationToken);

        if (!courses.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCourseDDLDto>(), "No courses found for this institute."));

        return Ok(ApiResponseHelper.Success(courses, "Courses retrieved successfully"));
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
