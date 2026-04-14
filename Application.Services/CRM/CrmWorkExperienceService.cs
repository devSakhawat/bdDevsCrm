// CrmWorkExperienceService.cs
using bdDevCRM.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.ServicesContract.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevCRM.Shared.Exceptions;
using Application.Shared.Grid;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CRM Work Experience service implementing business logic for work experience management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmWorkExperienceService : ICrmWorkExperienceService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmWorkExperienceService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmWorkExperienceService"/> with required dependencies.
	/// </summary>
	public CrmWorkExperienceService(IRepositoryManager repository, ILogger<CrmWorkExperienceService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new work experience record.
	/// </summary>
	public async Task<WorkExperienceHistoryDto> CreateWorkExperienceAsync(WorkExperienceHistoryDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(WorkExperienceHistoryDto));

		if (entityForCreate.WorkExperienceId != 0)
			throw new InvalidCreateOperationException("WorkExperienceId must be 0 for new record.");

		_logger.LogInformation("Creating new work experience. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var workExpEntity = MyMapper.JsonClone<WorkExperienceHistoryDto, CrmWorkExperience>(entityForCreate);
		workExpEntity.CreatedDate = DateTime.UtcNow;
		workExpEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmWorkExperiences.CreateAsync(workExpEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Work experience could not be saved to the database.");

		_logger.LogInformation("Work experience created successfully. ID: {WorkExperienceId}, Time: {Time}",
						workExpEntity.WorkExperienceId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmWorkExperience, WorkExperienceHistoryDto>(workExpEntity);
	}

	/// <summary>
	/// Updates an existing work experience record.
	/// </summary>
	public async Task<WorkExperienceHistoryDto> UpdateWorkExperienceAsync(int workExperienceId, WorkExperienceHistoryDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(WorkExperienceHistoryDto));

		if (workExperienceId != modelDto.WorkExperienceId)
			throw new BadRequestException(workExperienceId.ToString(), nameof(WorkExperienceHistoryDto));

		_logger.LogInformation("Updating work experience. ID: {WorkExperienceId}, Time: {Time}", workExperienceId, DateTime.UtcNow);

		var workExpEntity = await _repository.CrmWorkExperiences
						.FirstOrDefaultAsync(x => x.WorkExperienceId == workExperienceId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmWorkExperience", "WorkExperienceId", workExperienceId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmWorkExperience, WorkExperienceHistoryDto>(workExpEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmWorkExperiences.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmWorkExperience", "WorkExperienceId", workExperienceId.ToString());

		_logger.LogInformation("Work experience updated successfully. ID: {WorkExperienceId}, Time: {Time}",
						workExperienceId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmWorkExperience, WorkExperienceHistoryDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a work experience record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteWorkExperienceAsync(int workExperienceId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (workExperienceId <= 0)
			throw new BadRequestException(workExperienceId.ToString(), nameof(WorkExperienceHistoryDto));

		_logger.LogInformation("Deleting work experience. ID: {WorkExperienceId}, Time: {Time}", workExperienceId, DateTime.UtcNow);

		var workExpEntity = await _repository.CrmWorkExperiences
						.FirstOrDefaultAsync(x => x.WorkExperienceId == workExperienceId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmWorkExperience", "WorkExperienceId", workExperienceId.ToString());

		await _repository.CrmWorkExperiences.DeleteAsync(x => x.WorkExperienceId == workExperienceId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmWorkExperience", "WorkExperienceId", workExperienceId.ToString());

		_logger.LogInformation("Work experience deleted successfully. ID: {WorkExperienceId}, Time: {Time}",
						workExperienceId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single work experience record by its ID.
	/// </summary>
	public async Task<WorkExperienceHistoryDto> WorkExperienceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching work experience. ID: {WorkExperienceId}, Time: {Time}", id, DateTime.UtcNow);

		var workExp = await _repository.CrmWorkExperiences
						.FirstOrDefaultAsync(x => x.WorkExperienceId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmWorkExperience", "WorkExperienceId", id.ToString());

		_logger.LogInformation("Work experience fetched successfully. ID: {WorkExperienceId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmWorkExperience, WorkExperienceHistoryDto>(workExp);
	}

	/// <summary>
	/// Retrieves all work experience records from the database.
	/// </summary>
	public async Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperiencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all work experiences. Time: {Time}", DateTime.UtcNow);

		var workExps = await _repository.CrmWorkExperiences
						.CrmWorkExperiencesAsync(trackChanges, cancellationToken);

		if (!workExps.Any())
		{
			_logger.LogWarning("No work experiences found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<WorkExperienceHistoryDto>();
		}

		var workExpsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmWorkExperience, WorkExperienceHistoryDto>(workExps);

		_logger.LogInformation("Work experiences fetched successfully. Count: {Count}, Time: {Time}",
						workExpsDto.Count(), DateTime.UtcNow);

		return workExpsDto;
	}

	/// <summary>
	/// Retrieves active work experience records from the database.
	/// </summary>
	public async Task<IEnumerable<WorkExperienceHistoryDto>> ActiveWorkExperiencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active work experiences. Time: {Time}", DateTime.UtcNow);

		var workExps = await _repository.CrmWorkExperiences
						.CrmWorkExperiencesAsync(trackChanges, cancellationToken);

		if (!workExps.Any())
		{
			_logger.LogWarning("No active work experiences found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<WorkExperienceHistoryDto>();
		}

		var workExpsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmWorkExperience, WorkExperienceHistoryDto>(workExps);

		_logger.LogInformation("Active work experiences fetched successfully. Count: {Count}, Time: {Time}",
						workExpsDto.Count(), DateTime.UtcNow);

		return workExpsDto;
	}

	/// <summary>
	/// Retrieves work experiences by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperiencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("WorkExperiencesByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching work experiences for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var workExps = await _repository.CrmWorkExperiences
						.CrmWorkExperiencesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!workExps.Any())
		{
			_logger.LogWarning("No work experiences found for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
			return Enumerable.Empty<WorkExperienceHistoryDto>();
		}

		var workExpsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmWorkExperience, WorkExperienceHistoryDto>(workExps);

		_logger.LogInformation("Work experiences fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, workExpsDto.Count(), DateTime.UtcNow);

		return workExpsDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all work experiences suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<WorkExperienceHistoryDto>> WorkExperienceForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching work experiences for dropdown list. Time: {Time}", DateTime.UtcNow);

		var workExps = await _repository.CrmWorkExperiences
						.CrmWorkExperiencesAsync(false, cancellationToken);

		if (!workExps.Any())
		{
			_logger.LogWarning("No work experiences found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<WorkExperienceHistoryDto>();
		}

		var workExpsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmWorkExperience, WorkExperienceHistoryDto>(workExps);

		_logger.LogInformation("Work experiences fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						workExpsDto.Count(), DateTime.UtcNow);

		return workExpsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all work experiences.
	/// </summary>
	public async Task<GridEntity<WorkExperienceHistoryDto>> WorkExperiencesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    we.WorkExperienceId,
                    we.ApplicantId,
                    we.NameOfEmployer,
                    we.Position,
                    we.StartDate,
                    we.EndDate,
                    we.Period,
                    we.MainResponsibility,
                    we.ScannedCopy,
                    we.DocumentName,
                    we.FileThumbnail,
                    we.CreatedDate,
                    we.CreatedBy,
                    we.UpdatedDate,
                    we.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmWorkExperience we
                LEFT JOIN CrmApplication app ON we.ApplicantId = app.ApplicationId";

		const string orderBy = "we.CreatedDate DESC";

		_logger.LogInformation("Fetching work experiences summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmWorkExperiences.AdoGridDataAsync<WorkExperienceHistoryDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmWorkExperience service implementing business logic for CrmWorkExperience management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmWorkExperienceService : ICrmWorkExperienceService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmWorkExperienceService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmWorkExperienceService(IRepositoryManager repository, ILogger<CrmWorkExperienceService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmWorkExperience records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmWorkExperienceDto>> CrmWorkExperienceSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmWorkExperience summary grid");

//        string query = "SELECT * FROM CrmWorkExperience";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmWorkExperiences.AdoGridDataAsync<CrmWorkExperienceDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmWorkExperience records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmWorkExperienceDto>> CrmWorkExperiencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmWorkExperience records");

//        var records = await _repository.CrmWorkExperiences.CrmWorkExperiencesAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmWorkExperience records found");
//            return Enumerable.Empty<CrmWorkExperienceDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmWorkExperience, CrmWorkExperienceDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmWorkExperience record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmWorkExperienceDto> CrmWorkExperienceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmWorkExperienceAsync called with invalid id: {CrmWorkExperienceId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmWorkExperience record with ID: {CrmWorkExperienceId}", id);

//        var record = await _repository.CrmWorkExperiences.CrmWorkExperienceAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmWorkExperience record not found with ID: {CrmWorkExperienceId}", id);
//            throw new NotFoundException("CrmWorkExperience", "CrmWorkExperienceId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmWorkExperience, CrmWorkExperienceDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmWorkExperience record asynchronously.
//    /// </summary>
//    public async Task<CrmWorkExperienceDto> CreateAsync(CrmWorkExperienceDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmWorkExperienceDto));

//        _logger.LogInformation("Creating new CrmWorkExperience record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmWorkExperiences.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmWorkExperience", "Name");

//        // Map and create
//        CrmWorkExperience entity = MyMapper.JsonClone<CrmWorkExperienceDto, CrmWorkExperience>(modelDto);
//        modelDto.CrmWorkExperienceId = await _repository.CrmWorkExperiences.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmWorkExperience record created successfully with ID: {CrmWorkExperienceId}", modelDto.CrmWorkExperienceId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmWorkExperience record asynchronously.
//    /// </summary>
//    public async Task<CrmWorkExperienceDto> UpdateAsync(int key, CrmWorkExperienceDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmWorkExperienceDto));

//        if (key != modelDto.CrmWorkExperienceId)
//            throw new BadRequestException(key.ToString(), nameof(CrmWorkExperienceDto));

//        _logger.LogInformation("Updating CrmWorkExperience record with ID: {CrmWorkExperienceId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmWorkExperiences.ByIdAsync(
//            x => x.CrmWorkExperienceId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmWorkExperience", "CrmWorkExperienceId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmWorkExperiences.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmWorkExperienceId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmWorkExperience", "Name");

//        // Map and update
//        CrmWorkExperience entity = MyMapper.JsonClone<CrmWorkExperienceDto, CrmWorkExperience>(modelDto);
//        _repository.CrmWorkExperiences.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmWorkExperience record updated successfully: {CrmWorkExperienceId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmWorkExperience record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmWorkExperience record with ID: {CrmWorkExperienceId}", key);

//        var record = await _repository.CrmWorkExperiences.ByIdAsync(
//            x => x.CrmWorkExperienceId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmWorkExperience", "CrmWorkExperienceId", key.ToString());

//        await _repository.CrmWorkExperiences.DeleteAsync(x => x.CrmWorkExperienceId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmWorkExperience record deleted successfully: {CrmWorkExperienceId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmWorkExperience records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmWorkExperienceForDDLDto>> CrmWorkExperiencesForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmWorkExperience records for dropdown list");

//        var records = await _repository.CrmWorkExperiences.ListWithSelectAsync(
//            x => new CrmWorkExperience
//            {
//                CrmWorkExperienceId = x.CrmWorkExperienceId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmWorkExperienceForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmWorkExperience, CrmWorkExperienceForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
