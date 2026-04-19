// CrmCourseService.cs
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>
/// CRM Course service implementing business logic for course management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmCourseService : ICrmCourseService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmCourseService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmCourseService"/> with required dependencies.
	/// </summary>
	public CrmCourseService(IRepositoryManager repository, ILogger<CrmCourseService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new course record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmCourseDto> CreateAsync(CreateCrmCourseRecord record, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmCourseRecord));

		_logger.LogInformation("Creating new course. Title: {CourseTitle}, Time: {Time}",
						record.CourseTitle, DateTime.UtcNow);

		bool courseExists = await _repository.CrmCourses.ExistsAsync(
						x => x.CourseTitle != null
								&& x.CourseTitle.Trim().ToLower() == record.CourseTitle!.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (courseExists)
			throw new DuplicateRecordException("CrmCourse", "CourseTitle");

		// Map Record to Entity using Mapster
		CrmCourse course = record.MapTo<CrmCourse>();
		int courseId = await _repository.CrmCourses.CreateAndIdAsync(course, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Course created successfully. ID: {CourseId}, Time: {Time}",
						courseId, DateTime.UtcNow);

		// Return as DTO
		var resultDto = course.MapTo<CrmCourseDto>();
		resultDto.CourseId = courseId;
		return resultDto;
	}

	/// <summary>
	/// Updates an existing course record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmCourseDto> UpdateAsync(UpdateCrmCourseRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmCourseRecord));

		_logger.LogInformation("Updating course. ID: {CourseId}, Time: {Time}", record.CourseId, DateTime.UtcNow);

		// Check if course exists
		var existingCourse = await _repository.CrmCourses
						.FirstOrDefaultAsync(x => x.CourseId == record.CourseId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmCourse", "CourseId", record.CourseId.ToString());

		// Check for duplicate title (excluding current record)
		bool duplicateExists = await _repository.CrmCourses.ExistsAsync(
						x => x.CourseTitle != null
								&& x.CourseTitle.Trim().ToLower() == record.CourseTitle!.Trim().ToLower()
								&& x.CourseId != record.CourseId,
						cancellationToken: cancellationToken);

		if (duplicateExists)
			throw new DuplicateRecordException("CrmCourse", "CourseTitle");

		// Map Record to Entity using Mapster
		CrmCourse course = record.MapTo<CrmCourse>();
		_repository.CrmCourses.UpdateByState(course);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Course updated successfully. ID: {CourseId}, Time: {Time}",
						record.CourseId, DateTime.UtcNow);

		return course.MapTo<CrmCourseDto>();
	}

	/// <summary>
	/// Deletes a course record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmCourseRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.CourseId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting course. ID: {CourseId}, Time: {Time}", record.CourseId, DateTime.UtcNow);

		var courseEntity = await _repository.CrmCourses
						.FirstOrDefaultAsync(x => x.CourseId == record.CourseId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmCourse", "CourseId", record.CourseId.ToString());

		await _repository.CrmCourses.DeleteAsync(x => x.CourseId == record.CourseId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Course deleted successfully. ID: {CourseId}, Time: {Time}",
						record.CourseId, DateTime.UtcNow);
	}

	/// <summary>
	/// Retrieves a single course record by its ID.
	/// </summary>
	public async Task<CrmCourseDto> CourseAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching course. ID: {CourseId}, Time: {Time}", id, DateTime.UtcNow);

		var course = await _repository.CrmCourses
						.FirstOrDefaultAsync(x => x.CourseId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmCourse", "CourseId", id.ToString());

		_logger.LogInformation("Course fetched successfully. ID: {CourseId}, Time: {Time}",
						id, DateTime.UtcNow);

		return course.MapTo<CrmCourseDto>();
	}

	/// <summary>
	/// Retrieves all course records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmCourseDto>> CoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all courses. Time: {Time}", DateTime.UtcNow);

		var courses = await _repository.CrmCourses.CrmCoursesAsync(trackChanges, cancellationToken);

		if (!courses.Any())
		{
			_logger.LogWarning("No courses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmCourseDto>();
		}

		var coursesDto = courses.MapToList<CrmCourseDto>();

		_logger.LogInformation("Courses fetched successfully. Count: {Count}, Time: {Time}",
						coursesDto.Count(), DateTime.UtcNow);

		return coursesDto;
	}

	/// <summary>
	/// Retrieves active course records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmCourseDto>> ActiveCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active courses. Time: {Time}", DateTime.UtcNow);

		var courses = await _repository.CrmCourses.CrmCoursesAsync(trackChanges, cancellationToken);

		if (!courses.Any())
		{
			_logger.LogWarning("No active courses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmCourseDto>();
		}

		var coursesDto = courses.MapToList<CrmCourseDto>();

		_logger.LogInformation("Active courses fetched successfully. Count: {Count}, Time: {Time}",
						coursesDto.Count(), DateTime.UtcNow);

		return coursesDto;
	}

	/// <summary>
	/// Retrieves courses by the specified institute ID.
	/// </summary>
	public async Task<IEnumerable<CrmCourseDDLDto>> CoursesByInstituteIdAsync(int instituteId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (instituteId <= 0)
		{
			_logger.LogWarning("CoursesByInstituteIdAsync called with invalid instituteId: {InstituteId}", instituteId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching courses for institute ID: {InstituteId}, Time: {Time}", instituteId, DateTime.UtcNow);

		var courses = await _repository.CrmCourses.ListByWhereWithSelectAsync(
										expression: x => x.InstituteId == instituteId,
										selector: x => new CrmCourseDDLDto
										{
											CourseId = x.CourseId,
											CourseTitle = x.CourseTitle,
											ApplicationFee = x.ApplicationFee,
											CurrencyId = x.CurrencyId
										},
										orderBy: x => x.CourseTitle,
										trackChanges: trackChanges,
										cancellationToken: cancellationToken);

		if (!courses.Any())
		{
			_logger.LogWarning("No courses found for institute ID: {InstituteId}, Time: {Time}", instituteId, DateTime.UtcNow);
			return Enumerable.Empty<CrmCourseDDLDto>();
		}

		_logger.LogInformation("Courses fetched successfully for institute ID: {InstituteId}. Count: {Count}, Time: {Time}",
						instituteId, courses.Count(), DateTime.UtcNow);

		return courses;
	}

	/// <summary>
	/// Retrieves a course by its title.
	/// </summary>
	public async Task<CrmCourseDto> CourseByTitleAsync(string title, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(title))
		{
			_logger.LogWarning("CourseByTitleAsync called with null or whitespace title");
			throw new BadRequestException(nameof(title));
		}

		_logger.LogInformation("Fetching course by title: {Title}, Time: {Time}", title, DateTime.UtcNow);

		var course = await _repository.CrmCourses
						.FirstOrDefaultAsync(x => x.CourseTitle == title, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmCourse", "CourseTitle", title);

		_logger.LogInformation("Course fetched successfully. ID: {CourseId}, Time: {Time}",
						course.CourseId, DateTime.UtcNow);

		return course.MapTo<CrmCourseDto>();
	}

	/// <summary>
	/// Retrieves a lightweight list of all courses suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmCourseDDLDto>> CourseForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching courses for dropdown list. Time: {Time}", DateTime.UtcNow);

		var courseDDL = await _repository.CrmCourses.ListWithSelectAsync(selector: x => new CrmCourseDDLDto
		{
			CourseId = x.CourseId,
			CourseTitle = x.CourseTitle,
			ApplicationFee = x.ApplicationFee,
			CurrencyId = x.CurrencyId
		}
		,orderBy: x => x.CourseCategory
		,trackChanges: false
		,cancellationToken: cancellationToken);

		if (!courseDDL.Any())
		{
			_logger.LogWarning("No courses found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmCourseDDLDto>();
		}

		_logger.LogInformation("Courses fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						courseDDL.Count(), DateTime.UtcNow);

		return courseDDL;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all courses.
	/// </summary>
	public async Task<GridEntity<CrmCourseDto>> CoursesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    c.CourseId,
                    c.InstituteId,
                    c.CourseTitle,
                    c.CourseLevel,
                    c.CourseFee,
                    c.ApplicationFee,
                    c.CurrencyId,
                    c.MonthlyLivingCost,
                    c.PartTimeWorkDetails,
                    c.StartDate,
                    c.EndDate,
                    c.CourseBenefits,
                    c.LanguagesRequirement,
                    c.CourseDuration,
                    c.CourseCategory,
                    c.AwardingBody,
                    c.AdditionalInformationOfCourse,
                    c.GeneralEligibility,
                    c.FundsRequirementforVisa,
                    c.InstitutionalBenefits,
                    c.VisaRequirement,
                    c.CountryBenefits,
                    c.KeyModules,
                    c.Status,
                    c.After2YearsPswcompletingCourse,
                    c.DocumentId,
                    i.InstituteName,
                    curr.CurrencyName
                FROM CrmCourse c
                LEFT JOIN CrmInstitute i ON c.InstituteId = i.InstituteId
                LEFT JOIN CrmCurrencyInfo curr ON c.CurrencyId = curr.CurrencyId";

		const string orderBy = "CourseTitle ASC";

		_logger.LogInformation("Fetching courses summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmCourses.AdoGridDataAsync<CrmCourseDto>(sql, options, orderBy, "", cancellationToken);
	}
}
