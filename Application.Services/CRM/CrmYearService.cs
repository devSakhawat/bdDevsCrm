// CrmYearService.cs
using Domain.Entities.Entities.CRM;
using Domain.Entities.Entities.System;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;


/// <summary>
/// CRM year service implementing business logic for year management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmYearService : ICrmYearService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmYearService> _logger;
	private readonly IConfiguration _config;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmYearService"/> with required dependencies.
	/// </summary>
	public CrmYearService(IRepositoryManager repository, ILogger<CrmYearService> logger, IConfiguration config)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
	}

	/// <summary>
	/// Creates a new year record.
	/// </summary>
	public async Task<CrmYearDto> CreateYearAsync(CrmYearDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(CrmYearDto));

		if (entityForCreate.YearId != 0)
			throw new InvalidCreateOperationException("YearId must be 0 for new record.");

		_logger.LogInformation("Creating new year. YearName: {YearName}, Time: {Time}",
						entityForCreate.YearName, DateTime.UtcNow);

		var yearEntity = MyMapper.JsonClone<CrmYearDto, CrmYear>(entityForCreate);
		//yearEntity.CreatedDate = DateTime.UtcNow;
		//yearEntity.CreatedBy = currentUser.UserId ?? 0;
		//yearEntity.IsActive = true;

		await _repository.CrmYears.CreateAsync(yearEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Year could not be saved to the database.");
		_logger.LogInformation("Year created successfully. ID: {YearId}, Time: {Time}",
						yearEntity.YearId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmYear, CrmYearDto>(yearEntity);
	}

	/// <summary>
	/// Updates an existing year record.
	/// </summary>
	public async Task<CrmYearDto> UpdateYearAsync(int yearId, CrmYearDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmYearDto));

		if (yearId != modelDto.YearId)
			throw new BadRequestException(yearId.ToString(), nameof(CrmYearDto));

		_logger.LogInformation("Updating year. ID: {YearId}, Time: {Time}", yearId, DateTime.UtcNow);

		var yearEntity = await _repository.CrmYears
						.FirstOrDefaultAsync(x => x.YearId == yearId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("Year", "YearId", yearId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmYear, CrmYearDto>(yearEntity, modelDto);
		//updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmYears.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("Year", "YearId", yearId.ToString());
		_logger.LogInformation("Year updated successfully. ID: {YearId}, Time: {Time}",
						yearId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmYear, CrmYearDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a year record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteYearAsync(int yearId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (yearId <= 0)
			throw new BadRequestException(yearId.ToString(), nameof(CrmYearDto));

		_logger.LogInformation("Deleting year. ID: {YearId}, Time: {Time}", yearId, DateTime.UtcNow);

		var yearEntity = await _repository.CrmYears
						.FirstOrDefaultAsync(x => x.YearId == yearId, trackChanges, cancellationToken)
						?? throw new NotFoundException("Year", "YearId", yearId.ToString());

		await _repository.CrmYears.DeleteAsync(x => x.YearId == yearId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("Year", "YearId", yearId.ToString());
		_logger.LogWarning("Year deleted successfully. ID: {YearId}, Time: {Time}",
						yearId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single year record by its ID.
	/// </summary>
	public async Task<CrmYearDto> YearAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching year. ID: {YearId}, Time: {Time}", id, DateTime.UtcNow);
		var year = await _repository.CrmYears
						.FirstOrDefaultAsync(x => x.YearId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("Year", "YearId", id.ToString());

		_logger.LogInformation("Year fetched successfully. ID: {YearId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmYear, CrmYearDto>(year);
	}

	/// <summary>
	/// Retrieves all year records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> YearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all years. Time: {Time}", DateTime.UtcNow);
		var years = await _repository.CrmYears.ListAsync(x => x.YearId, trackChanges, cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No years found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmYear, CrmYearDto>(years);
		_logger.LogInformation("Years fetched successfully. Count: {Count}, Time: {Time}",
						yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves active year records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> ActiveYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active years. Time: {Time}", DateTime.UtcNow);

		var years = await _repository.CrmYears.CrmYearsAsync(trackChanges, cancellationToken);
		//var years = await _repository.CrmYears.GetActiveYearAsync(trackChanges, cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No active years found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmYear, CrmYearDto>(years);

		_logger.LogInformation("Active years fetched successfully. Count: {Count}, Time: {Time}",
						yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all years suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> YearForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching years for dropdown list. Time: {Time}", DateTime.UtcNow);
		var years = await _repository.CrmYears.CrmYearsAsync(false, cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No years found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmYear, CrmYearDto>(years);
		_logger.LogInformation("Years fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves years by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<CrmYearDto>> YearsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("YearsByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching years for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
		//var applicantCourses = await _repository.CrmApplicantCourses.GetApplicantCoursesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);
		var applicantCourses = await _repository.CrmApplicantCourses.CrmApplicantCoursesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!applicantCourses.Any())
		{
			_logger.LogWarning("No applicant courses found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var intakeYearIds = applicantCourses.Where(ac => ac.IntakeYearId > 0).Select(ac => ac.IntakeYearId).Distinct().ToList();

		if (!intakeYearIds.Any())
		{
			_logger.LogWarning("No intake year IDs found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var years = await _repository.CrmYears.ListByConditionAsync(
						x => intakeYearIds.Contains(x.YearId),
						x => x.YearName,
						trackChanges: trackChanges,
						descending: false,
						cancellationToken: cancellationToken);

		if (!years.Any())
		{
			_logger.LogWarning("No years found for applicant ID: {ApplicantId}, Time: {Time}",
							applicantId, DateTime.UtcNow);
			return Enumerable.Empty<CrmYearDto>();
		}

		var yearsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmYear, CrmYearDto>(years);

		_logger.LogInformation("Years fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, yearsDto.Count(), DateTime.UtcNow);

		return yearsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all years.
	/// </summary>
	public async Task<GridEntity<CrmYearDto>> YearsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching years summary grid. Time: {Time}", DateTime.UtcNow);

		const string sql = @"
SELECT YearId, YearName, YearCode, Description, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy
FROM CrmYear";

		const string orderBy = "YearName ASC";

		//return await _repository.CrmYears.GridData<CrmYearDto>(sql, options, orderBy, string.Empty, cancellationToken);
		return await _repository.CrmYears.AdoGridDataAsync<CrmYearDto>(sql, options, orderBy, string.Empty, cancellationToken);
	}
}



//using Domain.Entities.Entities.CRM;
//using Domain.Contracts.Services.Core.SystemAdmin;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Contracts.Services.CRM;
//using bdDevs.Shared.DataTransferObjects.CRM;
//using Domain.Exceptions;
//using Application.Shared.Grid;
//using Application.Services.Mappings;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;

//namespace Application.Services.CRM;

///// <summary>
///// CrmYear service implementing business logic for CrmYear management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmYearService : ICrmYearService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmYearService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmYearService(IRepositoryManager repository, ILogger<CrmYearService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmYear records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmYearDto>> CrmYearSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmYear summary grid");

//        string query = "SELECT * FROM CrmYear";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmYears.AdoGridDataAsync<CrmYearDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmYear records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmYearDto>> CrmYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmYear records");

//        var records = await _repository.CrmYears.CrmYearsAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmYear records found");
//            return Enumerable.Empty<CrmYearDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmYear, CrmYearDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmYear record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmYearDto> CrmYearAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmYearAsync called with invalid id: {CrmYearId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmYear record with ID: {CrmYearId}", id);

//        var record = await _repository.CrmYears.CrmYearAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmYear record not found with ID: {CrmYearId}", id);
//            throw new NotFoundException("CrmYear", "CrmYearId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmYear, CrmYearDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmYear record asynchronously.
//    /// </summary>
//    public async Task<CrmYearDto> CreateAsync(CrmYearDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmYearDto));

//        _logger.LogInformation("Creating new CrmYear record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmYears.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmYear", "Name");

//        // Map and create
//        CrmYear entity = MyMapper.JsonClone<CrmYearDto, CrmYear>(modelDto);
//        modelDto.CrmYearId = await _repository.CrmYears.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmYear record created successfully with ID: {CrmYearId}", modelDto.CrmYearId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmYear record asynchronously.
//    /// </summary>
//    public async Task<CrmYearDto> UpdateAsync(int key, CrmYearDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmYearDto));

//        if (key != modelDto.CrmYearId)
//            throw new BadRequestException(key.ToString(), nameof(CrmYearDto));

//        _logger.LogInformation("Updating CrmYear record with ID: {CrmYearId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmYears.ByIdAsync(
//            x => x.CrmYearId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmYear", "CrmYearId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmYears.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmYearId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmYear", "Name");

//        // Map and update
//        CrmYear entity = MyMapper.JsonClone<CrmYearDto, CrmYear>(modelDto);
//        _repository.CrmYears.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmYear record updated successfully: {CrmYearId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmYear record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmYear record with ID: {CrmYearId}", key);

//        var record = await _repository.CrmYears.ByIdAsync(
//            x => x.CrmYearId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmYear", "CrmYearId", key.ToString());

//        await _repository.CrmYears.DeleteAsync(x => x.CrmYearId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmYear record deleted successfully: {CrmYearId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmYear records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmYearForDDLDto>> CrmYearsForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmYear records for dropdown list");

//        var records = await _repository.CrmYears.ListWithSelectAsync(
//            x => new CrmYear
//            {
//                CrmYearId = x.CrmYearId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmYearForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmYear, CrmYearForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
