// CrmCourseService.cs
using bdDevCRM.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.ServicesContract.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevCRM.Shared.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

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
	/// Creates a new course record.
	/// </summary>
	public async Task<CrmCourseDto> CreateCourseAsync(CrmCourseDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(CrmCourseDto));

		if (entityForCreate.CourseId != 0)
			throw new InvalidCreateOperationException("CourseId must be 0 for new record.");

		bool courseExists = await _repository.CrmCourses.ExistsAsync(
						x => x.CourseTitle != null
								&& x.CourseTitle.Trim().ToLower() == entityForCreate.CourseTitle!.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (courseExists)
			throw new DuplicateRecordException("CrmCourse", "CourseTitle");

		_logger.LogInformation("Creating new course. Title: {CourseTitle}, Time: {Time}",
						entityForCreate.CourseTitle, DateTime.UtcNow);

		var courseEntity = MyMapper.JsonClone<CrmCourseDto, CrmCourse>(entityForCreate);
		//courseEntity.CreatedDate = DateTime.UtcNow;
		//courseEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmCourses.CreateAsync(courseEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Course could not be saved to the database.");

		_logger.LogInformation("Course created successfully. ID: {CourseId}, Time: {Time}",
						courseEntity.CourseId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmCourse, CrmCourseDto>(courseEntity);
	}

	/// <summary>
	/// Updates an existing course record.
	/// </summary>
	public async Task<CrmCourseDto> UpdateCourseAsync(int courseId, CrmCourseDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmCourseDto));

		if (courseId != modelDto.CourseId)
			throw new BadRequestException(courseId.ToString(), nameof(CrmCourseDto));

		_logger.LogInformation("Updating course. ID: {CourseId}, Time: {Time}", courseId, DateTime.UtcNow);

		var courseEntity = await _repository.CrmCourses
						.FirstOrDefaultAsync(x => x.CourseId == courseId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmCourse", "CourseId", courseId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmCourse, CrmCourseDto>(courseEntity, modelDto);
		//updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmCourses.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmCourse", "CourseId", courseId.ToString());

		_logger.LogInformation("Course updated successfully. ID: {CourseId}, Time: {Time}",
						courseId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmCourse, CrmCourseDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a course record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteCourseAsync(int courseId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (courseId <= 0)
			throw new BadRequestException(courseId.ToString(), nameof(CrmCourseDto));

		_logger.LogInformation("Deleting course. ID: {CourseId}, Time: {Time}", courseId, DateTime.UtcNow);

		var courseEntity = await _repository.CrmCourses
						.FirstOrDefaultAsync(x => x.CourseId == courseId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmCourse", "CourseId", courseId.ToString());

		await _repository.CrmCourses.DeleteAsync(x => x.CourseId == courseId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmCourse", "CourseId", courseId.ToString());

		_logger.LogInformation("Course deleted successfully. ID: {CourseId}, Time: {Time}",
						courseId, DateTime.UtcNow);

		return affected;
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

		return MyMapper.JsonClone<CrmCourse, CrmCourseDto>(course);
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

		var coursesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmCourse, CrmCourseDto>(courses);

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

		var coursesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmCourse, CrmCourseDto>(courses);

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

		return MyMapper.JsonClone<CrmCourse, CrmCourseDto>(course);
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



//using bdDevCRM.Entities.Entities.CRM;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevCRM.s.CRM;
//using bdDevCRM.ServicesContract.CRM;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using bdDevCRM.Shared.Exceptions;
//using Application.Shared.Grid;
//using bdDevCRM.Utilities.OthersLibrary;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace bdDevCRM.Services.CRM;

///// <summary>
///// CrmCourse service implementing business logic for CrmCourse management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmCourseService : ICrmCourseService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmCourseService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmCourseService(IRepositoryManager repository, ILogger<CrmCourseService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmCourse records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmCourseDto>> CrmCourseSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmCourse summary grid");

//        string query = "SELECT * FROM CrmCourse";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmCourses.AdoGridDataAsync<CrmCourseDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmCourse records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmCourseDto>> CrmCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmCourse records");

//        var records = await _repository.CrmCourses.CrmCoursesAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmCourse records found");
//            return Enumerable.Empty<CrmCourseDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmCourse, CrmCourseDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmCourse record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmCourseDto> CrmCourseAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmCourseAsync called with invalid id: {CrmCourseId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmCourse record with ID: {CrmCourseId}", id);

//        var record = await _repository.CrmCourses.CrmCourseAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmCourse record not found with ID: {CrmCourseId}", id);
//            throw new NotFoundException("CrmCourse", "CrmCourseId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmCourse, CrmCourseDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmCourse record asynchronously.
//    /// </summary>
//    public async Task<CrmCourseDto> CreateAsync(CrmCourseDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmCourseDto));

//        _logger.LogInformation("Creating new CrmCourse record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmCourses.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmCourse", "Name");

//        // Map and create
//        CrmCourse entity = MyMapper.JsonClone<CrmCourseDto, CrmCourse>(modelDto);
//        modelDto.CrmCourseId = await _repository.CrmCourses.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmCourse record created successfully with ID: {CrmCourseId}", modelDto.CrmCourseId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmCourse record asynchronously.
//    /// </summary>
//    public async Task<CrmCourseDto> UpdateAsync(int key, CrmCourseDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmCourseDto));

//        if (key != modelDto.CrmCourseId)
//            throw new BadRequestException(key.ToString(), nameof(CrmCourseDto));

//        _logger.LogInformation("Updating CrmCourse record with ID: {CrmCourseId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmCourses.ByIdAsync(
//            x => x.CrmCourseId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmCourse", "CrmCourseId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmCourses.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmCourseId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmCourse", "Name");

//        // Map and update
//        CrmCourse entity = MyMapper.JsonClone<CrmCourseDto, CrmCourse>(modelDto);
//        _repository.CrmCourses.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmCourse record updated successfully: {CrmCourseId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmCourse record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmCourse record with ID: {CrmCourseId}", key);

//        var record = await _repository.CrmCourses.ByIdAsync(
//            x => x.CrmCourseId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmCourse", "CrmCourseId", key.ToString());

//        await _repository.CrmCourses.DeleteAsync(x => x.CrmCourseId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmCourse record deleted successfully: {CrmCourseId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmCourse records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmCourseForDDLDto>> CrmCoursesForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmCourse records for dropdown list");

//        var records = await _repository.CrmCourses.ListWithSelectAsync(
//            x => new CrmCourse
//            {
//                CrmCourseId = x.CrmCourseId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmCourseForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmCourse, CrmCourseForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
