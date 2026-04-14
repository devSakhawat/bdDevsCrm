// CrmOthersInformationService.cs
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
/// CRM Others Information service implementing business logic for others information management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmOthersInformationService : ICrmOthersInformationService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmOthersInformationService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmOthersInformationService"/> with required dependencies.
	/// </summary>
	public CrmOthersInformationService(IRepositoryManager repository, ILogger<CrmOthersInformationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new others information record.
	/// </summary>
	public async Task<OthersInformationDto> CreateOthersInformationAsync(OthersInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(OthersInformationDto));

		if (entityForCreate.OthersInformationId != 0)
			throw new InvalidCreateOperationException("OthersInformationId must be 0 for new record.");

		bool applicantExists = await _repository.CrmOthersInformations.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmOthersInformation", "ApplicantId");

		_logger.LogInformation("Creating new others information. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var othersEntity = MyMapper.JsonClone<OthersInformationDto, CrmOthersInformation>(entityForCreate);
		othersEntity.CreatedDate = DateTime.UtcNow;
		othersEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmOthersInformations.CreateAsync(othersEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Others information could not be saved to the database.");

		_logger.LogInformation("Others information created successfully. ID: {OthersInformationId}, Time: {Time}",
						othersEntity.OthersInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmOthersInformation, OthersInformationDto>(othersEntity);
	}

	/// <summary>
	/// Updates an existing others information record.
	/// </summary>
	public async Task<OthersInformationDto> UpdateOthersInformationAsync(int othersInformationId, OthersInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(OthersInformationDto));

		if (othersInformationId != modelDto.OthersInformationId)
			throw new BadRequestException(othersInformationId.ToString(), nameof(OthersInformationDto));

		_logger.LogInformation("Updating others information. ID: {OthersInformationId}, Time: {Time}", othersInformationId, DateTime.UtcNow);

		var othersEntity = await _repository.CrmOthersInformations
						.FirstOrDefaultAsync(x => x.OthersInformationId == othersInformationId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmOthersInformation", "OthersInformationId", othersInformationId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmOthersInformation, OthersInformationDto>(othersEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmOthersInformations.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmOthersInformation", "OthersInformationId", othersInformationId.ToString());

		_logger.LogInformation("Others information updated successfully. ID: {OthersInformationId}, Time: {Time}",
						othersInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmOthersInformation, OthersInformationDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an others information record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteOthersInformationAsync(int othersInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (othersInformationId <= 0)
			throw new BadRequestException(othersInformationId.ToString(), nameof(OthersInformationDto));

		_logger.LogInformation("Deleting others information. ID: {OthersInformationId}, Time: {Time}", othersInformationId, DateTime.UtcNow);

		var othersEntity = await _repository.CrmOthersInformations
						.FirstOrDefaultAsync(x => x.OthersInformationId == othersInformationId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmOthersInformation", "OthersInformationId", othersInformationId.ToString());

		await _repository.CrmOthersInformations.DeleteAsync(x => x.OthersInformationId == othersInformationId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmOthersInformation", "OthersInformationId", othersInformationId.ToString());

		_logger.LogInformation("Others information deleted successfully. ID: {OthersInformationId}, Time: {Time}",
						othersInformationId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single others information record by its ID.
	/// </summary>
	public async Task<OthersInformationDto> OthersInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching others information. ID: {OthersInformationId}, Time: {Time}", id, DateTime.UtcNow);

		var others = await _repository.CrmOthersInformations
						.FirstOrDefaultAsync(x => x.OthersInformationId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmOthersInformation", "OthersInformationId", id.ToString());

		_logger.LogInformation("Others information fetched successfully. ID: {OthersInformationId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmOthersInformation, OthersInformationDto>(others);
	}

	/// <summary>
	/// Retrieves all others information records from the database.
	/// </summary>
	public async Task<IEnumerable<OthersInformationDto>> OthersInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all others informations. Time: {Time}", DateTime.UtcNow);

		var others = await _repository.CrmOthersInformations
						.CrmOthersInformationsAsync(trackChanges, cancellationToken);

		if (!others.Any())
		{
			_logger.LogWarning("No others informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<OthersInformationDto>();
		}

		var othersDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmOthersInformation, OthersInformationDto>(others);

		_logger.LogInformation("Others informations fetched successfully. Count: {Count}, Time: {Time}",
						othersDto.Count(), DateTime.UtcNow);

		return othersDto;
	}

	/// <summary>
	/// Retrieves active others information records from the database.
	/// </summary>
	public async Task<IEnumerable<OthersInformationDto>> ActiveOthersInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active others informations. Time: {Time}", DateTime.UtcNow);

		var others = await _repository.CrmOthersInformations
						.CrmOthersInformationsAsync(trackChanges, cancellationToken);

		if (!others.Any())
		{
			_logger.LogWarning("No active others informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<OthersInformationDto>();
		}

		var othersDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmOthersInformation, OthersInformationDto>(others);

		_logger.LogInformation("Active others informations fetched successfully. Count: {Count}, Time: {Time}",
						othersDto.Count(), DateTime.UtcNow);

		return othersDto;
	}

	/// <summary>
	/// Retrieves others information by the specified applicant ID.
	/// </summary>
	public async Task<OthersInformationDto> OthersInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("OthersInformationByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching others information for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var others = await _repository.CrmOthersInformations
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmOthersInformation", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("Others information fetched successfully. ID: {OthersInformationId}, Time: {Time}",
						others.OthersInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmOthersInformation, OthersInformationDto>(others);
	}

	/// <summary>
	/// Retrieves a lightweight list of all others informations suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<OthersInformationDto>> OthersInformationForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching others informations for dropdown list. Time: {Time}", DateTime.UtcNow);

		var others = await _repository.CrmOthersInformations.CrmOthersInformationsAsync(false, cancellationToken);

		if (!others.Any())
		{
			_logger.LogWarning("No others informations found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<OthersInformationDto>();
		}

		var othersDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmOthersInformation, OthersInformationDto>(others);

		_logger.LogInformation("Others informations fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						othersDto.Count(), DateTime.UtcNow);

		return othersDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all others informations.
	/// </summary>
	public async Task<GridEntity<OthersInformationDto>> OthersInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    oi.OthersInformationId,
                    oi.ApplicantId,
                    oi.OthersAdditionalInformation,
                    oi.OthersScannedCopyPath,
                    oi.CreatedDate,
                    oi.CreatedBy,
                    oi.UpdatedDate,
                    oi.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmOthersInformation oi
                LEFT JOIN CrmApplication app ON oi.ApplicantId = app.ApplicationId";

		const string orderBy = "oi.CreatedDate DESC";

		_logger.LogInformation("Fetching others informations summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmOthersInformations.AdoGridDataAsync<OthersInformationDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmOthersInformation service implementing business logic for CrmOthersInformation management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmOthersInformationService : ICrmOthersInformationService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmOthersInformationService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmOthersInformationService(IRepositoryManager repository, ILogger<CrmOthersInformationService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmOthersInformation records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmOthersInformationDto>> CrmOthersInformationSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmOthersInformation summary grid");

//        string query = "SELECT * FROM CrmOthersInformation";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmOthersInformations.AdoGridDataAsync<CrmOthersInformationDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmOthersInformation records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmOthersInformationDto>> CrmOthersInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmOthersInformation records");

//        var records = await _repository.CrmOthersInformations.CrmOthersInformationsAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmOthersInformation records found");
//            return Enumerable.Empty<CrmOthersInformationDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmOthersInformation, CrmOthersInformationDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmOthersInformation record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmOthersInformationDto> CrmOthersInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmOthersInformationAsync called with invalid id: {CrmOthersInformationId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmOthersInformation record with ID: {CrmOthersInformationId}", id);

//        var record = await _repository.CrmOthersInformations.CrmOthersInformationAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmOthersInformation record not found with ID: {CrmOthersInformationId}", id);
//            throw new NotFoundException("CrmOthersInformation", "CrmOthersInformationId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmOthersInformation, CrmOthersInformationDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmOthersInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmOthersInformationDto> CreateAsync(CrmOthersInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmOthersInformationDto));

//        _logger.LogInformation("Creating new CrmOthersInformation record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmOthersInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmOthersInformation", "Name");

//        // Map and create
//        CrmOthersInformation entity = MyMapper.JsonClone<CrmOthersInformationDto, CrmOthersInformation>(modelDto);
//        modelDto.CrmOthersInformationId = await _repository.CrmOthersInformations.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmOthersInformation record created successfully with ID: {CrmOthersInformationId}", modelDto.CrmOthersInformationId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmOthersInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmOthersInformationDto> UpdateAsync(int key, CrmOthersInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmOthersInformationDto));

//        if (key != modelDto.CrmOthersInformationId)
//            throw new BadRequestException(key.ToString(), nameof(CrmOthersInformationDto));

//        _logger.LogInformation("Updating CrmOthersInformation record with ID: {CrmOthersInformationId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmOthersInformations.ByIdAsync(
//            x => x.CrmOthersInformationId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmOthersInformation", "CrmOthersInformationId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmOthersInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmOthersInformationId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmOthersInformation", "Name");

//        // Map and update
//        CrmOthersInformation entity = MyMapper.JsonClone<CrmOthersInformationDto, CrmOthersInformation>(modelDto);
//        _repository.CrmOthersInformations.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmOthersInformation record updated successfully: {CrmOthersInformationId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmOthersInformation record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmOthersInformation record with ID: {CrmOthersInformationId}", key);

//        var record = await _repository.CrmOthersInformations.ByIdAsync(
//            x => x.CrmOthersInformationId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmOthersInformation", "CrmOthersInformationId", key.ToString());

//        await _repository.CrmOthersInformations.DeleteAsync(x => x.CrmOthersInformationId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmOthersInformation record deleted successfully: {CrmOthersInformationId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmOthersInformation records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmOthersInformationForDDLDto>> CrmOthersInformationsForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmOthersInformation records for dropdown list");

//        var records = await _repository.CrmOthersInformations.ListWithSelectAsync(
//            x => new CrmOthersInformation
//            {
//                CrmOthersInformationId = x.CrmOthersInformationId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmOthersInformationForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmOthersInformation, CrmOthersInformationForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
