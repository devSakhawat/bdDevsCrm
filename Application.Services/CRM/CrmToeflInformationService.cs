// CrmToeflInformationService.cs
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
/// CRM TOEFL Information service implementing business logic for TOEFL information management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmToeflInformationService : ICrmToeflInformationService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmToeflInformationService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmToeflInformationService"/> with required dependencies.
	/// </summary>
	public CrmToeflInformationService(IRepositoryManager repository, ILogger<CrmToeflInformationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new TOEFL information record.
	/// </summary>
	public async Task<ToeflInformationDto> CreateTOEFLInformationAsync(ToeflInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(ToeflInformationDto));

		if (entityForCreate.TOEFLInformationId != 0)
			throw new InvalidCreateOperationException("TOEFLInformationId must be 0 for new record.");

		bool applicantExists = await _repository.CrmToeflInformations.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmToeflInformation", "ApplicantId");

		_logger.LogInformation("Creating new TOEFL information. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var toeflEntity = MyMapper.JsonClone<ToeflInformationDto, CrmToeflInformation>(entityForCreate);
		toeflEntity.CreatedDate = DateTime.UtcNow;
		toeflEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmToeflInformations.CreateAsync(toeflEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("TOEFL information could not be saved to the database.");

		_logger.LogInformation("TOEFL information created successfully. ID: {TOEFLInformationId}, Time: {Time}",
						toeflEntity.TOEFLInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmToeflInformation, ToeflInformationDto>(toeflEntity);
	}

	/// <summary>
	/// Updates an existing TOEFL information record.
	/// </summary>
	public async Task<ToeflInformationDto> UpdateTOEFLInformationAsync(int toeflInformationId, ToeflInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(ToeflInformationDto));

		if (toeflInformationId != modelDto.TOEFLInformationId)
			throw new BadRequestException(toeflInformationId.ToString(), nameof(ToeflInformationDto));

		_logger.LogInformation("Updating TOEFL information. ID: {TOEFLInformationId}, Time: {Time}", toeflInformationId, DateTime.UtcNow);

		var toeflEntity = await _repository.CrmToeflInformations
						.FirstOrDefaultAsync(x => x.TOEFLInformationId == toeflInformationId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmToeflInformation", "TOEFLInformationId", toeflInformationId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmToeflInformation, ToeflInformationDto>(toeflEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmToeflInformations.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmToeflInformation", "TOEFLInformationId", toeflInformationId.ToString());

		_logger.LogInformation("TOEFL information updated successfully. ID: {TOEFLInformationId}, Time: {Time}",
						toeflInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmToeflInformation, ToeflInformationDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a TOEFL information record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteTOEFLInformationAsync(int toeflInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (toeflInformationId <= 0)
			throw new BadRequestException(toeflInformationId.ToString(), nameof(ToeflInformationDto));

		_logger.LogInformation("Deleting TOEFL information. ID: {TOEFLInformationId}, Time: {Time}", toeflInformationId, DateTime.UtcNow);

		var toeflEntity = await _repository.CrmToeflInformations
						.FirstOrDefaultAsync(x => x.TOEFLInformationId == toeflInformationId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmToeflInformation", "TOEFLInformationId", toeflInformationId.ToString());

		await _repository.CrmToeflInformations.DeleteAsync(x => x.TOEFLInformationId == toeflInformationId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmToeflInformation", "TOEFLInformationId", toeflInformationId.ToString());

		_logger.LogInformation("TOEFL information deleted successfully. ID: {TOEFLInformationId}, Time: {Time}",
						toeflInformationId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single TOEFL information record by its ID.
	/// </summary>
	public async Task<ToeflInformationDto> TOEFLInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching TOEFL information. ID: {TOEFLInformationId}, Time: {Time}", id, DateTime.UtcNow);

		var toefl = await _repository.CrmToeflInformations
						.FirstOrDefaultAsync(x => x.TOEFLInformationId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmToeflInformation", "TOEFLInformationId", id.ToString());

		_logger.LogInformation("TOEFL information fetched successfully. ID: {TOEFLInformationId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmToeflInformation, ToeflInformationDto>(toefl);
	}

	/// <summary>
	/// Retrieves all TOEFL information records from the database.
	/// </summary>
	public async Task<IEnumerable<ToeflInformationDto>> TOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all TOEFL informations. Time: {Time}", DateTime.UtcNow);

		var toefls = await _repository.CrmToeflInformations
						.CrmToeflInformationsAsync(trackChanges, cancellationToken);

		if (!toefls.Any())
		{
			_logger.LogWarning("No TOEFL informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ToeflInformationDto>();
		}

		var toeflsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmToeflInformation, ToeflInformationDto>(toefls);

		_logger.LogInformation("TOEFL informations fetched successfully. Count: {Count}, Time: {Time}",
						toeflsDto.Count(), DateTime.UtcNow);

		return toeflsDto;
	}

	/// <summary>
	/// Retrieves active TOEFL information records from the database.
	/// </summary>
	public async Task<IEnumerable<ToeflInformationDto>> ActiveTOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active TOEFL informations. Time: {Time}", DateTime.UtcNow);

		var toefls = await _repository.CrmToeflInformations
						.CrmToeflInformationsAsync(trackChanges, cancellationToken);

		if (!toefls.Any())
		{
			_logger.LogWarning("No active TOEFL informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ToeflInformationDto>();
		}

		var toeflsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmToeflInformation, ToeflInformationDto>(toefls);

		_logger.LogInformation("Active TOEFL informations fetched successfully. Count: {Count}, Time: {Time}",
						toeflsDto.Count(), DateTime.UtcNow);

		return toeflsDto;
	}

	/// <summary>
	/// Retrieves TOEFL information by the specified applicant ID.
	/// </summary>
	public async Task<ToeflInformationDto> TOEFLInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("TOEFLInformationByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching TOEFL information for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var toefl = await _repository.CrmToeflInformations
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmToeflInformation", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("TOEFL information fetched successfully. ID: {TOEFLInformationId}, Time: {Time}",
						toefl.TOEFLInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmToeflInformation, ToeflInformationDto>(toefl);
	}

	/// <summary>
	/// Retrieves a lightweight list of all TOEFL informations suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<ToeflInformationDto>> TOEFLInformationForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching TOEFL informations for dropdown list. Time: {Time}", DateTime.UtcNow);

		var toefls = await _repository.CrmToeflInformations
						.CrmToeflInformationsAsync(false, cancellationToken);

		if (!toefls.Any())
		{
			_logger.LogWarning("No TOEFL informations found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ToeflInformationDto>();
		}

		var toeflsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmToeflInformation, ToeflInformationDto>(toefls);

		_logger.LogInformation("TOEFL informations fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						toeflsDto.Count(), DateTime.UtcNow);

		return toeflsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all TOEFL informations.
	/// </summary>
	public async Task<GridEntity<ToeflInformationDto>> TOEFLInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    ti.TOEFLInformationId,
                    ti.ApplicantId,
                    ti.TOEFLListening,
                    ti.TOEFLReading,
                    ti.TOEFLWriting,
                    ti.TOEFLSpeaking,
                    ti.TOEFLOverallScore,
                    ti.TOEFLDate,
                    ti.TOEFLScannedCopyPath,
                    ti.TOEFLAdditionalInformation,
                    ti.CreatedDate,
                    ti.CreatedBy,
                    ti.UpdatedDate,
                    ti.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmToeflInformation ti
                LEFT JOIN CrmApplication app ON ti.ApplicantId = app.ApplicationId";

		const string orderBy = "ti.CreatedDate DESC";

		_logger.LogInformation("Fetching TOEFL informations summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmToeflInformations.AdoGridDataAsync<ToeflInformationDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmToeflInformation service implementing business logic for CrmToeflInformation management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmToeflInformationService : ICrmToeflInformationService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmToeflInformationService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmToeflInformationService(IRepositoryManager repository, ILogger<CrmToeflInformationService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmToeflInformation records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmToeflInformationDto>> CrmToeflInformationSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmToeflInformation summary grid");

//        string query = "SELECT * FROM CrmToeflInformation";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmToeflInformations.AdoGridDataAsync<CrmToeflInformationDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmToeflInformation records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmToeflInformationDto>> CrmToeflInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmToeflInformation records");

//        var records = await _repository.CrmToeflInformations.CrmToeflInformationsAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmToeflInformation records found");
//            return Enumerable.Empty<CrmToeflInformationDto>();
//        }

//        var recordDtos = records.MapToList<CrmToeflInformationDto>();
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmToeflInformation record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmToeflInformationDto> CrmToeflInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmToeflInformationAsync called with invalid id: {CrmToeflInformationId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmToeflInformation record with ID: {CrmToeflInformationId}", id);

//        var record = await _repository.CrmToeflInformations.CrmToeflInformationAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmToeflInformation record not found with ID: {CrmToeflInformationId}", id);
//            throw new NotFoundException("CrmToeflInformation", "CrmToeflInformationId", id.ToString());
//        }

//        var recordDto = record.MapTo<CrmToeflInformationDto>();
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmToeflInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmToeflInformationDto> CreateAsync(CrmToeflInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmToeflInformationDto));

//        _logger.LogInformation("Creating new CrmToeflInformation record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmToeflInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmToeflInformation", "Name");

//        // Map and create
//        CrmToeflInformation entity = modelDto.MapTo<CrmToeflInformation>();
//        modelDto.CrmToeflInformationId = await _repository.CrmToeflInformations.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmToeflInformation record created successfully with ID: {CrmToeflInformationId}", modelDto.CrmToeflInformationId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmToeflInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmToeflInformationDto> UpdateAsync(int key, CrmToeflInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmToeflInformationDto));

//        if (key != modelDto.CrmToeflInformationId)
//            throw new BadRequestException(key.ToString(), nameof(CrmToeflInformationDto));

//        _logger.LogInformation("Updating CrmToeflInformation record with ID: {CrmToeflInformationId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmToeflInformations.ByIdAsync(
//            x => x.CrmToeflInformationId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmToeflInformation", "CrmToeflInformationId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmToeflInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmToeflInformationId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmToeflInformation", "Name");

//        // Map and update
//        CrmToeflInformation entity = modelDto.MapTo<CrmToeflInformation>();
//        _repository.CrmToeflInformations.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmToeflInformation record updated successfully: {CrmToeflInformationId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmToeflInformation record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmToeflInformation record with ID: {CrmToeflInformationId}", key);

//        var record = await _repository.CrmToeflInformations.ByIdAsync(
//            x => x.CrmToeflInformationId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmToeflInformation", "CrmToeflInformationId", key.ToString());

//        await _repository.CrmToeflInformations.DeleteAsync(x => x.CrmToeflInformationId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmToeflInformation record deleted successfully: {CrmToeflInformationId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmToeflInformation records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmToeflInformationForDDLDto>> CrmToeflInformationsForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmToeflInformation records for dropdown list");

//        var records = await _repository.CrmToeflInformations.ListWithSelectAsync(
//            x => new CrmToeflInformation
//            {
//                CrmToeflInformationId = x.CrmToeflInformationId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmToeflInformationForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmToeflInformation, CrmToeflInformationForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
