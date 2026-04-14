
// CrmApplicantCourseService.cs
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
/// CRM Applicant Course service implementing business logic for applicant course management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmApplicantCourseService : ICrmApplicantCourseService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmApplicantCourseService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmApplicantCourseService"/> with required dependencies.
	/// </summary>
	public CrmApplicantCourseService(IRepositoryManager repository, ILogger<CrmApplicantCourseService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new applicant course record.
	/// </summary>
	public async Task<ApplicantCourseDto> CreateApplicantCourseAsync(ApplicantCourseDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(ApplicantCourseDto));

		if (entityForCreate.ApplicantCourseId != 0)
			throw new InvalidCreateOperationException("ApplicantCourseId must be 0 for new record.");

		bool dup = await _repository.CrmApplicantCourses.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId && x.CourseId == entityForCreate.CourseId,
						cancellationToken: cancellationToken);

		if (dup)
			throw new DuplicateRecordException("ApplicantCourse", "ApplicantId-CourseId combination");

		_logger.LogInformation("Creating new applicant course. ApplicantId: {ApplicantId}, CourseId: {CourseId}, Time: {Time}",
						entityForCreate.ApplicantId, entityForCreate.CourseId, DateTime.UtcNow);

		var courseEntity = MyMapper.JsonClone<ApplicantCourseDto, CrmApplicantCourse>(entityForCreate);
		courseEntity.CreatedDate = DateTime.UtcNow;
		courseEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmApplicantCourses.CreateAsync(courseEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Applicant course could not be saved to the database.");

		_logger.LogInformation("Applicant course created successfully. ID: {ApplicantCourseId}, Time: {Time}",
						courseEntity.ApplicantCourseId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantCourse, ApplicantCourseDto>(courseEntity);
	}

	/// <summary>
	/// Updates an existing applicant course record.
	/// </summary>
	public async Task<ApplicantCourseDto> UpdateApplicantCourseAsync(int applicantCourseId, ApplicantCourseDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(ApplicantCourseDto));

		if (applicantCourseId != modelDto.ApplicantCourseId)
			throw new BadRequestException(applicantCourseId.ToString(), nameof(ApplicantCourseDto));

		_logger.LogInformation("Updating applicant course. ID: {ApplicantCourseId}, Time: {Time}", applicantCourseId, DateTime.UtcNow);

		var courseEntity = await _repository.CrmApplicantCourses
						.FirstOrDefaultAsync(x => x.ApplicantCourseId == applicantCourseId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("ApplicantCourse", "ApplicantCourseId", applicantCourseId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmApplicantCourse, ApplicantCourseDto>(courseEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmApplicantCourses.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("ApplicantCourse", "ApplicantCourseId", applicantCourseId.ToString());

		_logger.LogInformation("Applicant course updated successfully. ID: {ApplicantCourseId}, Time: {Time}",
						applicantCourseId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantCourse, ApplicantCourseDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an applicant course record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteApplicantCourseAsync(int applicantCourseId, ApplicantCourseDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(ApplicantCourseDto));

		if (applicantCourseId != modelDto.ApplicantCourseId)
			throw new BadRequestException(applicantCourseId.ToString(), nameof(ApplicantCourseDto));

		_logger.LogInformation("Deleting applicant course. ID: {ApplicantCourseId}, Time: {Time}", applicantCourseId, DateTime.UtcNow);

		var courseEntity = await _repository.CrmApplicantCourses
						.FirstOrDefaultAsync(x => x.ApplicantCourseId == applicantCourseId, trackChanges, cancellationToken)
						?? throw new NotFoundException("ApplicantCourse", "ApplicantCourseId", applicantCourseId.ToString());

		await _repository.CrmApplicantCourses.DeleteAsync(x => x.ApplicantCourseId == applicantCourseId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("ApplicantCourse", "ApplicantCourseId", applicantCourseId.ToString());

		_logger.LogInformation("Applicant course deleted successfully. ID: {ApplicantCourseId}, Time: {Time}",
						applicantCourseId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single applicant course record by its ID.
	/// </summary>
	public async Task<ApplicantCourseDto> ApplicantCourseAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant course. ID: {ApplicantCourseId}, Time: {Time}", id, DateTime.UtcNow);

		var course = await _repository.CrmApplicantCourses
						.FirstOrDefaultAsync(x => x.ApplicantCourseId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("ApplicantCourse", "ApplicantCourseId", id.ToString());

		_logger.LogInformation("Applicant course fetched successfully. ID: {ApplicantCourseId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantCourse, ApplicantCourseDto>(course);
	}

	/// <summary>
	/// Retrieves all applicant course records from the database.
	/// </summary>
	public async Task<IEnumerable<ApplicantCourseDto>> ApplicantCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all applicant courses. Time: {Time}", DateTime.UtcNow);

		var courses = await _repository.CrmApplicantCourses.CrmApplicantCoursesAsync(trackChanges, cancellationToken);

		if (!courses.Any())
		{
			_logger.LogWarning("No applicant courses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantCourseDto>();
		}

		var coursesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantCourse, ApplicantCourseDto>(courses);

		_logger.LogInformation("Applicant courses fetched successfully. Count: {Count}, Time: {Time}",
						coursesDto.Count(), DateTime.UtcNow);

		return coursesDto;
	}

	/// <summary>
	/// Retrieves active applicant course records from the database.
	/// </summary>
	public async Task<IEnumerable<ApplicantCourseDto>> ActiveApplicantCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active applicant courses. Time: {Time}", DateTime.UtcNow);

		var courses = await _repository.CrmApplicantCourses.CrmApplicantCoursesAsync(trackChanges, cancellationToken);

		if (!courses.Any())
		{
			_logger.LogWarning("No active applicant courses found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantCourseDto>();
		}

		var coursesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantCourse, ApplicantCourseDto>(courses);

		_logger.LogInformation("Active applicant courses fetched successfully. Count: {Count}, Time: {Time}",
						coursesDto.Count(), DateTime.UtcNow);

		return coursesDto;
	}

	/// <summary>
	/// Retrieves applicant courses by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<ApplicantCourseDto>> ApplicantCoursesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("ApplicantCoursesByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching applicant courses for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var courses = await _repository.CrmApplicantCourses.CrmApplicantCoursesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!courses.Any())
		{
			_logger.LogWarning("No applicant courses found for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
			return Enumerable.Empty<ApplicantCourseDto>();
		}

		var coursesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantCourse, ApplicantCourseDto>(courses);

		_logger.LogInformation("Applicant courses fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, coursesDto.Count(), DateTime.UtcNow);

		return coursesDto;
	}

	/// <summary>
	/// Retrieves a single applicant course by applicant ID and course ID.
	/// </summary>
	public async Task<ApplicantCourseDto> ApplicantCourseByApplicantAndCourseIdAsync(int applicantId, int courseId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant course by applicant ID: {ApplicantId} and course ID: {CourseId}, Time: {Time}",
						applicantId, courseId, DateTime.UtcNow);

		var course = await _repository.CrmApplicantCourses
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId && x.CourseId == courseId, trackChanges, cancellationToken)
						?? throw new NotFoundException("ApplicantCourse", "ApplicantId-CourseId", $"{applicantId}-{courseId}");

		_logger.LogInformation("Applicant course fetched successfully. ID: {ApplicantCourseId}, Time: {Time}",
						course.ApplicantCourseId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantCourse, ApplicantCourseDto>(course);
	}

	/// <summary>
	/// Retrieves a lightweight list of all applicant courses suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<ApplicantCourseDto>> ApplicantCourseForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant courses for dropdown list. Time: {Time}", DateTime.UtcNow);

		var courses = await _repository.CrmApplicantCourses.CrmApplicantCoursesAsync(trackChanges, cancellationToken);

		if (!courses.Any())
		{
			_logger.LogWarning("No applicant courses found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantCourseDto>();
		}

		var coursesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantCourse, ApplicantCourseDto>(courses);

		_logger.LogInformation("Applicant courses fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						coursesDto.Count(), DateTime.UtcNow);

		return coursesDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all applicant courses.
	/// </summary>
	public async Task<GridEntity<ApplicantCourseDto>> ApplicantCoursesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    ac.ApplicantCourseId,
                    ac.ApplicantId,
                    ac.CourseId,
                    ac.CountryId,
                    ac.CountryName,
                    ac.InstituteId,
                    ac.InstituteName,
                    ac.CourseTitle,
                    ac.IntakeMonthId,
                    ac.IntakeMonth,
                    ac.IntakeYearId,
                    ac.IntakeYear,
                    ac.ApplicationFee,
                    ac.CurrencyId,
                    ac.CurrencyName,
                    ac.PaymentMethodId,
                    ac.PaymentMethod,
                    ac.PaymentReferenceNumber,
                    ac.PaymentDate,
                    ac.Remarks,
                    ac.CreatedDate,
                    ac.CreatedBy,
                    ac.UpdatedDate,
                    ac.UpdatedBy,
                    app.ApplicationStatus,
                    c.CourseTitle AS CourseFullTitle
                FROM ApplicantCourse ac
                LEFT JOIN CrmApplication app ON ac.ApplicantId = app.ApplicationId
                LEFT JOIN CrmCourse c ON ac.CourseId = c.CourseId";

		const string orderBy = "ac.CreatedDate DESC";

		_logger.LogInformation("Fetching applicant courses summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmApplicantCourses.AdoGridDataAsync<ApplicantCourseDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmApplicantCourse service implementing business logic for CrmApplicantCourse management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmApplicantCourseService : ICrmApplicantCourseService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmApplicantCourseService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmApplicantCourseService(IRepositoryManager repository, ILogger<CrmApplicantCourseService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmApplicantCourse records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmApplicantCourseDto>> CrmApplicantCourseSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmApplicantCourse summary grid");

//        string query = "SELECT * FROM CrmApplicantCourse";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmApplicantCourses.AdoGridDataAsync<CrmApplicantCourseDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmApplicantCourse records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmApplicantCourseDto>> CrmApplicantCoursesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmApplicantCourse records");

//        var records = await _repository.CrmApplicantCourses.CrmApplicantCoursesAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmApplicantCourse records found");
//            return Enumerable.Empty<CrmApplicantCourseDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmApplicantCourse, CrmApplicantCourseDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmApplicantCourse record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantCourseDto> CrmApplicantCourseAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmApplicantCourseAsync called with invalid id: {CrmApplicantCourseId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmApplicantCourse record with ID: {CrmApplicantCourseId}", id);

//        var record = await _repository.CrmApplicantCourses.CrmApplicantCourseAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmApplicantCourse record not found with ID: {CrmApplicantCourseId}", id);
//            throw new NotFoundException("CrmApplicantCourse", "CrmApplicantCourseId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmApplicantCourse, CrmApplicantCourseDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmApplicantCourse record asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantCourseDto> CreateAsync(CrmApplicantCourseDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmApplicantCourseDto));

//        _logger.LogInformation("Creating new CrmApplicantCourse record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmApplicantCourses.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmApplicantCourse", "Name");

//        // Map and create
//        CrmApplicantCourse entity = MyMapper.JsonClone<CrmApplicantCourseDto, CrmApplicantCourse>(modelDto);
//        modelDto.CrmApplicantCourseId = await _repository.CrmApplicantCourses.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantCourse record created successfully with ID: {CrmApplicantCourseId}", modelDto.CrmApplicantCourseId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmApplicantCourse record asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantCourseDto> UpdateAsync(int key, CrmApplicantCourseDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmApplicantCourseDto));

//        if (key != modelDto.CrmApplicantCourseId)
//            throw new BadRequestException(key.ToString(), nameof(CrmApplicantCourseDto));

//        _logger.LogInformation("Updating CrmApplicantCourse record with ID: {CrmApplicantCourseId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmApplicantCourses.ByIdAsync(
//            x => x.CrmApplicantCourseId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmApplicantCourse", "CrmApplicantCourseId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmApplicantCourses.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmApplicantCourseId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmApplicantCourse", "Name");

//        // Map and update
//        CrmApplicantCourse entity = MyMapper.JsonClone<CrmApplicantCourseDto, CrmApplicantCourse>(modelDto);
//        _repository.CrmApplicantCourses.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantCourse record updated successfully: {CrmApplicantCourseId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmApplicantCourse record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmApplicantCourse record with ID: {CrmApplicantCourseId}", key);

//        var record = await _repository.CrmApplicantCourses.ByIdAsync(
//            x => x.CrmApplicantCourseId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmApplicantCourse", "CrmApplicantCourseId", key.ToString());

//        await _repository.CrmApplicantCourses.DeleteAsync(x => x.CrmApplicantCourseId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantCourse record deleted successfully: {CrmApplicantCourseId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmApplicantCourse records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmApplicantCourseForDDLDto>> CrmApplicantCoursesForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmApplicantCourse records for dropdown list");

//        var records = await _repository.CrmApplicantCourses.ListWithSelectAsync(
//            x => new CrmApplicantCourse
//            {
//                CrmApplicantCourseId = x.CrmApplicantCourseId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmApplicantCourseForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmApplicantCourse, CrmApplicantCourseForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
