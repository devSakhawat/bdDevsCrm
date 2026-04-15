using Domain.Contracts.Repositories;
// CrmGmatInformationService.cs
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CRM GMAT Information service implementing business logic for GMAT information management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmGmatInformationService : ICrmGmatInformationService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmGmatInformationService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmGmatInformationService"/> with required dependencies.
	/// </summary>
	public CrmGmatInformationService(IRepositoryManager repository, ILogger<CrmGmatInformationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new GMAT information record.
	/// </summary>
	public async Task<GmatInformationDto> CreateGMATInformationAsync(GmatInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(GmatInformationDto));

		if (entityForCreate.GMATInformationId != 0)
			throw new InvalidCreateOperationException("GMATInformationId must be 0 for new record.");

		bool applicantExists = await _repository.CrmGmatInformations.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmGmatInformation", "ApplicantId");

		_logger.LogInformation("Creating new GMAT information. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var gmatEntity = MyMapper.JsonClone<GmatInformationDto, CrmGmatInformation>(entityForCreate);
		gmatEntity.CreatedDate = DateTime.UtcNow;
		gmatEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmGmatInformations.CreateAsync(gmatEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("GMAT information could not be saved to the database.");

		_logger.LogInformation("GMAT information created successfully. ID: {GMATInformationId}, Time: {Time}",
						gmatEntity.GMATInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmGmatInformation, GmatInformationDto>(gmatEntity);
	}

	/// <summary>
	/// Updates an existing GMAT information record.
	/// </summary>
	public async Task<GmatInformationDto> UpdateGMATInformationAsync(int gmatInformationId, GmatInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(GmatInformationDto));

		if (gmatInformationId != modelDto.GMATInformationId)
			throw new BadRequestException(gmatInformationId.ToString(), nameof(GmatInformationDto));

		_logger.LogInformation("Updating GMAT information. ID: {GMATInformationId}, Time: {Time}", gmatInformationId, DateTime.UtcNow);

		var gmatEntity = await _repository.CrmGmatInformations
						.FirstOrDefaultAsync(x => x.GMATInformationId == gmatInformationId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmGmatInformation", "GMATInformationId", gmatInformationId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmGmatInformation, GmatInformationDto>(gmatEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmGmatInformations.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmGmatInformation", "GMATInformationId", gmatInformationId.ToString());

		_logger.LogInformation("GMAT information updated successfully. ID: {GMATInformationId}, Time: {Time}",
						gmatInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmGmatInformation, GmatInformationDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a GMAT information record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteGMATInformationAsync(int gmatInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (gmatInformationId <= 0)
			throw new BadRequestException(gmatInformationId.ToString(), nameof(GmatInformationDto));

		_logger.LogInformation("Deleting GMAT information. ID: {GMATInformationId}, Time: {Time}", gmatInformationId, DateTime.UtcNow);

		var gmatEntity = await _repository.CrmGmatInformations
						.FirstOrDefaultAsync(x => x.GMATInformationId == gmatInformationId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmGmatInformation", "GMATInformationId", gmatInformationId.ToString());

		await _repository.CrmGmatInformations.DeleteAsync(x => x.GMATInformationId == gmatInformationId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmGmatInformation", "GMATInformationId", gmatInformationId.ToString());

		_logger.LogInformation("GMAT information deleted successfully. ID: {GMATInformationId}, Time: {Time}",
						gmatInformationId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single GMAT information record by its ID.
	/// </summary>
	public async Task<GmatInformationDto> GMATInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching GMAT information. ID: {GMATInformationId}, Time: {Time}", id, DateTime.UtcNow);

		var gmat = await _repository.CrmGmatInformations
						.FirstOrDefaultAsync(x => x.GMATInformationId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmGmatInformation", "GMATInformationId", id.ToString());

		_logger.LogInformation("GMAT information fetched successfully. ID: {GMATInformationId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmGmatInformation, GmatInformationDto>(gmat);
	}

	/// <summary>
	/// Retrieves all GMAT information records from the database.
	/// </summary>
	public async Task<IEnumerable<GmatInformationDto>> GMATInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all GMAT informations. Time: {Time}", DateTime.UtcNow);

		var gmats = await _repository.CrmGmatInformations
						.CrmGmatInformationsAsync(trackChanges, cancellationToken);

		if (!gmats.Any())
		{
			_logger.LogWarning("No GMAT informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<GmatInformationDto>();
		}

		var gmatsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmGmatInformation, GmatInformationDto>(gmats);

		_logger.LogInformation("GMAT informations fetched successfully. Count: {Count}, Time: {Time}",
						gmatsDto.Count(), DateTime.UtcNow);

		return gmatsDto;
	}

	/// <summary>
	/// Retrieves active GMAT information records from the database.
	/// </summary>
	public async Task<IEnumerable<GmatInformationDto>> ActiveGMATInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active GMAT informations. Time: {Time}", DateTime.UtcNow);

		var gmats = await _repository.CrmGmatInformations
						.CrmGmatInformationsAsync(trackChanges, cancellationToken);

		if (!gmats.Any())
		{
			_logger.LogWarning("No active GMAT informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<GmatInformationDto>();
		}

		var gmatsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmGmatInformation, GmatInformationDto>(gmats);

		_logger.LogInformation("Active GMAT informations fetched successfully. Count: {Count}, Time: {Time}",
						gmatsDto.Count(), DateTime.UtcNow);

		return gmatsDto;
	}

	/// <summary>
	/// Retrieves GMAT information by the specified applicant ID.
	/// </summary>
	public async Task<GmatInformationDto> GMATInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("GMATInformationByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching GMAT information for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var gmat = await _repository.CrmGmatInformations
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmGmatInformation", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("GMAT information fetched successfully. ID: {GMATInformationId}, Time: {Time}",
						gmat.GMATInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmGmatInformation, GmatInformationDto>(gmat);
	}

	/// <summary>
	/// Retrieves a lightweight list of all GMAT informations suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<GmatInformationDto>> GMATInformationForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching GMAT informations for dropdown list. Time: {Time}", DateTime.UtcNow);

		var gmats = await _repository.CrmGmatInformations.CrmGmatInformationsAsync(false, cancellationToken);

		if (!gmats.Any())
		{
			_logger.LogWarning("No GMAT informations found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<GmatInformationDto>();
		}

		var gmatsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmGmatInformation, GmatInformationDto>(gmats);

		_logger.LogInformation("GMAT informations fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						gmatsDto.Count(), DateTime.UtcNow);

		return gmatsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all GMAT informations.
	/// </summary>
	public async Task<GridEntity<GmatInformationDto>> GMATInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    gi.GMATInformationId,
                    gi.ApplicantId,
                    gi.Gmatlistening,
                    gi.Gmatreading,
                    gi.Gmatwriting,
                    gi.Gmatspeaking,
                    gi.GmatoverallScore,
                    gi.Gmatdate,
                    gi.GmatscannedCopyPath,
                    gi.GmatadditionalInformation,
                    gi.CreatedDate,
                    gi.CreatedBy,
                    gi.UpdatedDate,
                    gi.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmGmatInformation gi
                LEFT JOIN CrmApplication app ON gi.ApplicantId = app.ApplicationId";

		const string orderBy = "gi.CreatedDate DESC";

		_logger.LogInformation("Fetching GMAT informations summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmGmatInformations.AdoGridDataAsync<GmatInformationDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmGmatInformation service implementing business logic for CrmGmatInformation management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmGmatInformationService : ICrmGmatInformationService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmGmatInformationService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmGmatInformationService(IRepositoryManager repository, ILogger<CrmGmatInformationService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmGmatInformation records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmGmatInformationDto>> CrmGmatInformationSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmGmatInformation summary grid");

//        string query = "SELECT * FROM CrmGmatInformation";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmGmatInformations.AdoGridDataAsync<CrmGmatInformationDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmGmatInformation records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmGmatInformationDto>> CrmGmatInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmGmatInformation records");

//        var records = await _repository.CrmGmatInformations.CrmGmatInformationsAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmGmatInformation records found");
//            return Enumerable.Empty<CrmGmatInformationDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmGmatInformation, CrmGmatInformationDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmGmatInformation record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmGmatInformationDto> CrmGmatInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmGmatInformationAsync called with invalid id: {CrmGmatInformationId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmGmatInformation record with ID: {CrmGmatInformationId}", id);

//        var record = await _repository.CrmGmatInformations.CrmGmatInformationAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmGmatInformation record not found with ID: {CrmGmatInformationId}", id);
//            throw new NotFoundException("CrmGmatInformation", "CrmGmatInformationId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmGmatInformation, CrmGmatInformationDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmGmatInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmGmatInformationDto> CreateAsync(CrmGmatInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmGmatInformationDto));

//        _logger.LogInformation("Creating new CrmGmatInformation record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmGmatInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmGmatInformation", "Name");

//        // Map and create
//        CrmGmatInformation entity = MyMapper.JsonClone<CrmGmatInformationDto, CrmGmatInformation>(modelDto);
//        modelDto.CrmGmatInformationId = await _repository.CrmGmatInformations.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmGmatInformation record created successfully with ID: {CrmGmatInformationId}", modelDto.CrmGmatInformationId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmGmatInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmGmatInformationDto> UpdateAsync(int key, CrmGmatInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmGmatInformationDto));

//        if (key != modelDto.CrmGmatInformationId)
//            throw new BadRequestException(key.ToString(), nameof(CrmGmatInformationDto));

//        _logger.LogInformation("Updating CrmGmatInformation record with ID: {CrmGmatInformationId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmGmatInformations.ByIdAsync(
//            x => x.CrmGmatInformationId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmGmatInformation", "CrmGmatInformationId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmGmatInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmGmatInformationId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmGmatInformation", "Name");

//        // Map and update
//        CrmGmatInformation entity = MyMapper.JsonClone<CrmGmatInformationDto, CrmGmatInformation>(modelDto);
//        _repository.CrmGmatInformations.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmGmatInformation record updated successfully: {CrmGmatInformationId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmGmatInformation record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmGmatInformation record with ID: {CrmGmatInformationId}", key);

//        var record = await _repository.CrmGmatInformations.ByIdAsync(
//            x => x.CrmGmatInformationId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmGmatInformation", "CrmGmatInformationId", key.ToString());

//        await _repository.CrmGmatInformations.DeleteAsync(x => x.CrmGmatInformationId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmGmatInformation record deleted successfully: {CrmGmatInformationId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmGmatInformation records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmGmatInformationForDDLDto>> CrmGmatInformationsForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmGmatInformation records for dropdown list");

//        var records = await _repository.CrmGmatInformations.ListWithSelectAsync(
//            x => new CrmGmatInformation
//            {
//                CrmGmatInformationId = x.CrmGmatInformationId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmGmatInformationForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmGmatInformation, CrmGmatInformationForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
