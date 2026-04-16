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

/// <summary>
/// CRM Course Intake management endpoints.
/// </summary>
[AuthorizeUser]
public class CrmCourseIntakeController : BaseApiController
{
    private readonly IMemoryCache _cache;

    public CrmCourseIntakeController(IServiceManager serviceManager, IMemoryCache cache) : base(serviceManager)
    {
        _cache = cache;
    }

    /// <summary>
    /// Retrieves all course intakes for dropdown list.
    /// </summary>
    [HttpGet(RouteConstants.CrmCourseIntakeDDL)]
    [ResponseCache(Duration = 300)]
    public async Task<IActionResult> CourseIntakesForDDLAsync(CancellationToken cancellationToken = default)
    {
        var courseIntakes = await _serviceManager.CrmCourseIntakes.CourseIntakesForDDLAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!courseIntakes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCourseIntakeDDLDto>(), "No course intakes found."));

        return Ok(ApiResponseHelper.Success(courseIntakes, "Course intakes retrieved successfully"));
    }

    /// <summary>
    /// Retrieves paginated summary grid of course intakes.
    /// </summary>
    [HttpPost(RouteConstants.CrmCourseIntakeSummary)]
    public async Task<IActionResult> CourseIntakeSummaryAsync([FromBody] GridOptions options, CancellationToken cancellationToken = default)
    {
        if (options == null)
            throw new NullModelBadRequestException(nameof(GridOptions));

        var summaryGrid = await _serviceManager.CrmCourseIntakes.CourseIntakesSummaryAsync(options, cancellationToken);

        if (!summaryGrid.Items.Any())
            return Ok(ApiResponseHelper.Success(new GridEntity<CrmCourseIntakeDto>(), "No course intakes found."));

        return Ok(ApiResponseHelper.Success(summaryGrid, "Course intake summary retrieved successfully"));
    }

    /// <summary>
    /// Creates a new course intake record using CRUD Record pattern.
    /// </summary>
    [HttpPost(RouteConstants.CreateCrmCourseIntake)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> CreateCourseIntakeAsync([FromBody] CreateCrmCourseIntakeRecord record, CancellationToken cancellationToken = default)
    {
        var createdCourseIntake = await _serviceManager.CrmCourseIntakes.CreateAsync(record, cancellationToken);

        if (createdCourseIntake.CourseIntakeId <= 0)
            throw new InvalidCreateOperationException("Failed to create course intake record.");

        return Ok(ApiResponseHelper.Created(createdCourseIntake, "Course intake created successfully."));
    }

    /// <summary>
    /// Updates an existing course intake record using CRUD Record pattern.
    /// </summary>
    [HttpPut(RouteConstants.UpdateCrmCourseIntake)]
    [ServiceFilter(typeof(EmptyObjectFilterAttribute))]
    public async Task<IActionResult> UpdateCourseIntakeAsync([FromRoute] int key, [FromBody] UpdateCrmCourseIntakeRecord record, CancellationToken cancellationToken = default)
    {
        if (key != record.CourseIntakeId)
            throw new IdMismatchBadRequestException(key.ToString(), nameof(UpdateCrmCourseIntakeRecord));

        var updatedCourseIntake = await _serviceManager.CrmCourseIntakes.UpdateAsync(record, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Updated(updatedCourseIntake, "Course intake updated successfully."));
    }

    /// <summary>
    /// Deletes a course intake record using CRUD Record pattern.
    /// </summary>
    [HttpDelete(RouteConstants.DeleteCrmCourseIntake)]
    public async Task<IActionResult> DeleteCourseIntakeAsync([FromRoute] int key, CancellationToken cancellationToken = default)
    {
        var deleteRecord = new DeleteCrmCourseIntakeRecord(key);
        await _serviceManager.CrmCourseIntakes.DeleteAsync(deleteRecord, trackChanges: false, cancellationToken: cancellationToken);
        return Ok(ApiResponseHelper.NoContent<object>("Course intake deleted successfully"));
    }

    /// <summary>
    /// Retrieves a course intake by ID.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmCourseIntake)]
    public async Task<IActionResult> CourseIntakeAsync([FromRoute] int id, CancellationToken cancellationToken = default)
    {
        if (id <= 0)
            throw new IdParametersBadRequestException();

        var courseIntake = await _serviceManager.CrmCourseIntakes.CourseIntakeAsync(id, trackChanges: false, cancellationToken: cancellationToken);

        return Ok(ApiResponseHelper.Success(courseIntake, "Course intake retrieved successfully"));
    }

    /// <summary>
    /// Retrieves all course intakes.
    /// </summary>
    [HttpGet(RouteConstants.ReadCrmCourseIntakes)]
    public async Task<IActionResult> CourseIntakesAsync(CancellationToken cancellationToken = default)
    {
        var courseIntakes = await _serviceManager.CrmCourseIntakes.CourseIntakesAsync(trackChanges: false, cancellationToken: cancellationToken);

        if (!courseIntakes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCourseIntakeDto>(), "No course intakes found."));

        return Ok(ApiResponseHelper.Success(courseIntakes, "Course intakes retrieved successfully"));
    }

    /// <summary>
    /// Retrieves course intakes by course ID.
    /// </summary>
    [HttpGet(RouteConstants.CrmCourseIntakesByCourse)]
    public async Task<IActionResult> CourseIntakesByCourseAsync([FromRoute] int courseId, CancellationToken cancellationToken = default)
    {
        if (courseId <= 0)
            throw new IdParametersBadRequestException();

        var courseIntakes = await _serviceManager.CrmCourseIntakes.CourseIntakesByCourseIdAsync(courseId, trackChanges: false, cancellationToken: cancellationToken);

        if (!courseIntakes.Any())
            return Ok(ApiResponseHelper.Success(Enumerable.Empty<CrmCourseIntakeDto>(), "No course intakes found for this course."));

        return Ok(ApiResponseHelper.Success(courseIntakes, "Course intakes retrieved successfully"));
    }
}
