// CrmEducationHistoryService.cs
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

namespace Application.Services.CRM;

/// <summary>
/// CRM Education History service implementing business logic for education history management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmEducationHistoryService : ICrmEducationHistoryService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmEducationHistoryService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmEducationHistoryService"/> with required dependencies.
	/// </summary>
	public CrmEducationHistoryService(IRepositoryManager repository, ILogger<CrmEducationHistoryService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new education history record.
	/// </summary>
	public async Task<EducationHistoryDto> CreateEducationHistoryAsync(EducationHistoryDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(EducationHistoryDto));

		if (entityForCreate.EducationHistoryId != 0)
			throw new InvalidCreateOperationException("EducationHistoryId must be 0 for new record.");

		_logger.LogInformation("Creating new education history. ApplicantId: {ApplicantId}, Institution: {Institution}, Time: {Time}",
						entityForCreate.ApplicantId, entityForCreate.Institution, DateTime.UtcNow);

		var educationEntity = MyMapper.JsonClone<EducationHistoryDto, CrmEducationHistory>(entityForCreate);
		educationEntity.CreatedDate = DateTime.UtcNow;
		educationEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmEducationHistories.CreateAsync(educationEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Education history could not be saved to the database.");

		_logger.LogInformation("Education history created successfully. ID: {EducationHistoryId}, Time: {Time}",
						educationEntity.EducationHistoryId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmEducationHistory, EducationHistoryDto>(educationEntity);
	}

	/// <summary>
	/// Updates an existing education history record.
	/// </summary>
	public async Task<EducationHistoryDto> UpdateEducationHistoryAsync(int educationHistoryId, EducationHistoryDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(EducationHistoryDto));

		if (educationHistoryId != modelDto.EducationHistoryId)
			throw new BadRequestException(educationHistoryId.ToString(), nameof(EducationHistoryDto));

		_logger.LogInformation("Updating education history. ID: {EducationHistoryId}, Time: {Time}", educationHistoryId, DateTime.UtcNow);

		var educationEntity = await _repository.CrmEducationHistories
						.FirstOrDefaultAsync(x => x.EducationHistoryId == educationHistoryId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmEducationHistory", "EducationHistoryId", educationHistoryId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmEducationHistory, EducationHistoryDto>(educationEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmEducationHistories.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmEducationHistory", "EducationHistoryId", educationHistoryId.ToString());

		_logger.LogInformation("Education history updated successfully. ID: {EducationHistoryId}, Time: {Time}",
						educationHistoryId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmEducationHistory, EducationHistoryDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an education history record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteEducationHistoryAsync(int educationHistoryId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (educationHistoryId <= 0)
			throw new BadRequestException(educationHistoryId.ToString(), nameof(EducationHistoryDto));

		_logger.LogInformation("Deleting education history. ID: {EducationHistoryId}, Time: {Time}", educationHistoryId, DateTime.UtcNow);

		var educationEntity = await _repository.CrmEducationHistories
						.FirstOrDefaultAsync(x => x.EducationHistoryId == educationHistoryId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmEducationHistory", "EducationHistoryId", educationHistoryId.ToString());

		await _repository.CrmEducationHistories.DeleteAsync(x => x.EducationHistoryId == educationHistoryId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmEducationHistory", "EducationHistoryId", educationHistoryId.ToString());

		_logger.LogInformation("Education history deleted successfully. ID: {EducationHistoryId}, Time: {Time}",
						educationHistoryId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single education history record by its ID.
	/// </summary>
	public async Task<EducationHistoryDto> EducationHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching education history. ID: {EducationHistoryId}, Time: {Time}", id, DateTime.UtcNow);

		var education = await _repository.CrmEducationHistories
						.FirstOrDefaultAsync(x => x.EducationHistoryId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmEducationHistory", "EducationHistoryId", id.ToString());

		_logger.LogInformation("Education history fetched successfully. ID: {EducationHistoryId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmEducationHistory, EducationHistoryDto>(education);
	}

	/// <summary>
	/// Retrieves all education history records from the database.
	/// </summary>
	public async Task<IEnumerable<EducationHistoryDto>> EducationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all education histories. Time: {Time}", DateTime.UtcNow);

		var educations = await _repository.CrmEducationHistories.CrmEducationHistoriesAsync(trackChanges, cancellationToken);

		if (!educations.Any())
		{
			_logger.LogWarning("No education histories found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<EducationHistoryDto>();
		}

		var educationsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmEducationHistory, EducationHistoryDto>(educations);

		_logger.LogInformation("Education histories fetched successfully. Count: {Count}, Time: {Time}",
						educationsDto.Count(), DateTime.UtcNow);

		return educationsDto;
	}

	/// <summary>
	/// Retrieves active education history records from the database.
	/// </summary>
	public async Task<IEnumerable<EducationHistoryDto>> ActiveEducationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active education histories. Time: {Time}", DateTime.UtcNow);

		var educations = await _repository.CrmEducationHistories.CrmEducationHistoriesAsync(trackChanges, cancellationToken);

		if (!educations.Any())
		{
			_logger.LogWarning("No active education histories found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<EducationHistoryDto>();
		}

		var educationsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmEducationHistory, EducationHistoryDto>(educations);

		_logger.LogInformation("Active education histories fetched successfully. Count: {Count}, Time: {Time}",
						educationsDto.Count(), DateTime.UtcNow);

		return educationsDto;
	}

	/// <summary>
	/// Retrieves education histories by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<EducationHistoryDto>> EducationHistoriesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("EducationHistoriesByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching education histories for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var educations = await _repository.CrmEducationHistories.CrmEducationHistorysByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!educations.Any())
		{
			_logger.LogWarning("No education histories found for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
			return Enumerable.Empty<EducationHistoryDto>();
		}

		var educationsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmEducationHistory, EducationHistoryDto>(educations);

		_logger.LogInformation("Education histories fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, educationsDto.Count(), DateTime.UtcNow);

		return educationsDto;
	}

	/// <summary>
	/// Retrieves an education history by institution name.
	/// </summary>
	public async Task<EducationHistoryDto> EducationHistoryByInstitutionAsync(string institution, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(institution))
		{
			_logger.LogWarning("EducationHistoryByInstitutionAsync called with null or whitespace institution");
			throw new BadRequestException(nameof(institution));
		}

		_logger.LogInformation("Fetching education history by institution: {Institution}, Time: {Time}", institution, DateTime.UtcNow);

		var education = await _repository.CrmEducationHistories
						.FirstOrDefaultAsync(x => x.Institution == institution, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmEducationHistory", "Institution", institution);

		_logger.LogInformation("Education history fetched successfully. ID: {EducationHistoryId}, Time: {Time}",
						education.EducationHistoryId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmEducationHistory, EducationHistoryDto>(education);
	}

	/// <summary>
	/// Retrieves a lightweight list of all education histories suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<EducationHistoryDto>> EducationHistoryForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching education histories for dropdown list. Time: {Time}", DateTime.UtcNow);

		var educations = await _repository.CrmEducationHistories.CrmEducationHistoriesAsync(false, cancellationToken);

		if (!educations.Any())
		{
			_logger.LogWarning("No education histories found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<EducationHistoryDto>();
		}

		var educationsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmEducationHistory, EducationHistoryDto>(educations);

		_logger.LogInformation("Education histories fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						educationsDto.Count(), DateTime.UtcNow);

		return educationsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all education histories.
	/// </summary>
	public async Task<GridEntity<EducationHistoryDto>> EducationHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    eh.EducationHistoryId,
                    eh.ApplicantId,
                    eh.Institution,
                    eh.Qualification,
                    eh.PassingYear,
                    eh.Grade,
                    eh.AttachedDocument,
                    eh.DocumentName,
                    eh.PdfThumbnail,
                    eh.CreatedDate,
                    eh.CreatedBy,
                    eh.UpdatedDate,
                    eh.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmEducationHistory eh
                LEFT JOIN CrmApplication app ON eh.ApplicantId = app.ApplicationId";

		const string orderBy = "eh.CreatedDate DESC";

		_logger.LogInformation("Fetching education histories summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmEducationHistories.AdoGridDataAsync<EducationHistoryDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmEducationHistory service implementing business logic for CrmEducationHistory management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmEducationHistoryService : ICrmEducationHistoryService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmEducationHistoryService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmEducationHistoryService(IRepositoryManager repository, ILogger<CrmEducationHistoryService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmEducationHistory records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmEducationHistoryDto>> CrmEducationHistorySummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmEducationHistory summary grid");

//        string query = "SELECT * FROM CrmEducationHistory";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmEducationHistorys.AdoGridDataAsync<CrmEducationHistoryDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmEducationHistory records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmEducationHistoryDto>> CrmEducationHistorysAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmEducationHistory records");

//        var records = await _repository.CrmEducationHistorys.CrmEducationHistorysAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmEducationHistory records found");
//            return Enumerable.Empty<CrmEducationHistoryDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmEducationHistory, CrmEducationHistoryDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmEducationHistory record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmEducationHistoryDto> CrmEducationHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmEducationHistoryAsync called with invalid id: {CrmEducationHistoryId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmEducationHistory record with ID: {CrmEducationHistoryId}", id);

//        var record = await _repository.CrmEducationHistorys.CrmEducationHistoryAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmEducationHistory record not found with ID: {CrmEducationHistoryId}", id);
//            throw new NotFoundException("CrmEducationHistory", "CrmEducationHistoryId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmEducationHistory, CrmEducationHistoryDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmEducationHistory record asynchronously.
//    /// </summary>
//    public async Task<CrmEducationHistoryDto> CreateAsync(CrmEducationHistoryDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmEducationHistoryDto));

//        _logger.LogInformation("Creating new CrmEducationHistory record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmEducationHistorys.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmEducationHistory", "Name");

//        // Map and create
//        CrmEducationHistory entity = MyMapper.JsonClone<CrmEducationHistoryDto, CrmEducationHistory>(modelDto);
//        modelDto.CrmEducationHistoryId = await _repository.CrmEducationHistorys.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmEducationHistory record created successfully with ID: {CrmEducationHistoryId}", modelDto.CrmEducationHistoryId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmEducationHistory record asynchronously.
//    /// </summary>
//    public async Task<CrmEducationHistoryDto> UpdateAsync(int key, CrmEducationHistoryDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmEducationHistoryDto));

//        if (key != modelDto.CrmEducationHistoryId)
//            throw new BadRequestException(key.ToString(), nameof(CrmEducationHistoryDto));

//        _logger.LogInformation("Updating CrmEducationHistory record with ID: {CrmEducationHistoryId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmEducationHistorys.ByIdAsync(
//            x => x.CrmEducationHistoryId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmEducationHistory", "CrmEducationHistoryId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmEducationHistorys.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmEducationHistoryId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmEducationHistory", "Name");

//        // Map and update
//        CrmEducationHistory entity = MyMapper.JsonClone<CrmEducationHistoryDto, CrmEducationHistory>(modelDto);
//        _repository.CrmEducationHistorys.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmEducationHistory record updated successfully: {CrmEducationHistoryId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmEducationHistory record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmEducationHistory record with ID: {CrmEducationHistoryId}", key);

//        var record = await _repository.CrmEducationHistorys.ByIdAsync(
//            x => x.CrmEducationHistoryId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmEducationHistory", "CrmEducationHistoryId", key.ToString());

//        await _repository.CrmEducationHistorys.DeleteAsync(x => x.CrmEducationHistoryId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmEducationHistory record deleted successfully: {CrmEducationHistoryId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmEducationHistory records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmEducationHistoryForDDLDto>> CrmEducationHistorysForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmEducationHistory records for dropdown list");

//        var records = await _repository.CrmEducationHistorys.ListWithSelectAsync(
//            x => new CrmEducationHistory
//            {
//                CrmEducationHistoryId = x.CrmEducationHistoryId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmEducationHistoryForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmEducationHistory, CrmEducationHistoryForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
