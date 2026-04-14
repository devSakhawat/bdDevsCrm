// CrmPTEInformationService.cs
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
/// CRM PTE Information service implementing business logic for PTE information management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmPTEInformationService : ICrmPTEInformationService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmPTEInformationService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmPTEInformationService"/> with required dependencies.
	/// </summary>
	public CrmPTEInformationService(IRepositoryManager repository, ILogger<CrmPTEInformationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new PTE information record.
	/// </summary>
	public async Task<PTEInformationDto> CreatePTEInformationAsync(PTEInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(PTEInformationDto));

		if (entityForCreate.PTEInformationId != 0)
			throw new InvalidCreateOperationException("PTEInformationId must be 0 for new record.");

		bool applicantExists = await _repository.CrmPTEInformations.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmPTEInformation", "ApplicantId");

		_logger.LogInformation("Creating new PTE information. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var pteEntity = MyMapper.JsonClone<PTEInformationDto, CrmPTEInformation>(entityForCreate);
		pteEntity.CreatedDate = DateTime.UtcNow;
		pteEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmPTEInformations.CreateAsync(pteEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("PTE information could not be saved to the database.");

		_logger.LogInformation("PTE information created successfully. ID: {PTEInformationId}, Time: {Time}",
						pteEntity.PTEInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPTEInformation, PTEInformationDto>(pteEntity);
	}

	/// <summary>
	/// Updates an existing PTE information record.
	/// </summary>
	public async Task<PTEInformationDto> UpdatePTEInformationAsync(int pteInformationId, PTEInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(PTEInformationDto));

		if (pteInformationId != modelDto.PTEInformationId)
			throw new BadRequestException(pteInformationId.ToString(), nameof(PTEInformationDto));

		_logger.LogInformation("Updating PTE information. ID: {PTEInformationId}, Time: {Time}", pteInformationId, DateTime.UtcNow);

		var pteEntity = await _repository.CrmPTEInformations
						.FirstOrDefaultAsync(x => x.PTEInformationId == pteInformationId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmPTEInformation", "PTEInformationId", pteInformationId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmPTEInformation, PTEInformationDto>(pteEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmPTEInformations.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmPTEInformation", "PTEInformationId", pteInformationId.ToString());

		_logger.LogInformation("PTE information updated successfully. ID: {PTEInformationId}, Time: {Time}",
						pteInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPTEInformation, PTEInformationDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a PTE information record identified by the given ID.
	/// </summary>
	public async Task<int> DeletePTEInformationAsync(int pteInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (pteInformationId <= 0)
			throw new BadRequestException(pteInformationId.ToString(), nameof(PTEInformationDto));

		_logger.LogInformation("Deleting PTE information. ID: {PTEInformationId}, Time: {Time}", pteInformationId, DateTime.UtcNow);

		var pteEntity = await _repository.CrmPTEInformations
						.FirstOrDefaultAsync(x => x.PTEInformationId == pteInformationId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPTEInformation", "PTEInformationId", pteInformationId.ToString());

		await _repository.CrmPTEInformations.DeleteAsync(x => x.PTEInformationId == pteInformationId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmPTEInformation", "PTEInformationId", pteInformationId.ToString());

		_logger.LogInformation("PTE information deleted successfully. ID: {PTEInformationId}, Time: {Time}",
						pteInformationId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single PTE information record by its ID.
	/// </summary>
	public async Task<PTEInformationDto> PTEInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching PTE information. ID: {PTEInformationId}, Time: {Time}", id, DateTime.UtcNow);

		var pte = await _repository.CrmPTEInformations
						.FirstOrDefaultAsync(x => x.PTEInformationId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPTEInformation", "PTEInformationId", id.ToString());

		_logger.LogInformation("PTE information fetched successfully. ID: {PTEInformationId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPTEInformation, PTEInformationDto>(pte);
	}

	/// <summary>
	/// Retrieves all PTE information records from the database.
	/// </summary>
	public async Task<IEnumerable<PTEInformationDto>> PTEInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all PTE informations. Time: {Time}", DateTime.UtcNow);

		var ptes = await _repository.CrmPTEInformations
						.CrmPTEInformationsAsync(trackChanges, cancellationToken);

		if (!ptes.Any())
		{
			_logger.LogWarning("No PTE informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PTEInformationDto>();
		}

		var ptesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPTEInformation, PTEInformationDto>(ptes);

		_logger.LogInformation("PTE informations fetched successfully. Count: {Count}, Time: {Time}",
						ptesDto.Count(), DateTime.UtcNow);

		return ptesDto;
	}

	/// <summary>
	/// Retrieves active PTE information records from the database.
	/// </summary>
	public async Task<IEnumerable<PTEInformationDto>> ActivePTEInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active PTE informations. Time: {Time}", DateTime.UtcNow);

		var ptes = await _repository.CrmPTEInformations
						.CrmPTEInformationsAsync(trackChanges, cancellationToken);

		if (!ptes.Any())
		{
			_logger.LogWarning("No active PTE informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PTEInformationDto>();
		}

		var ptesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPTEInformation, PTEInformationDto>(ptes);

		_logger.LogInformation("Active PTE informations fetched successfully. Count: {Count}, Time: {Time}",
						ptesDto.Count(), DateTime.UtcNow);

		return ptesDto;
	}

	/// <summary>
	/// Retrieves PTE information by the specified applicant ID.
	/// </summary>
	public async Task<PTEInformationDto> PTEInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("PTEInformationByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching PTE information for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var pte = await _repository.CrmPTEInformations
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmPTEInformation", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("PTE information fetched successfully. ID: {PTEInformationId}, Time: {Time}",
						pte.PTEInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmPTEInformation, PTEInformationDto>(pte);
	}

	/// <summary>
	/// Retrieves a lightweight list of all PTE informations suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<PTEInformationDto>> PTEInformationForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching PTE informations for dropdown list. Time: {Time}", DateTime.UtcNow);

		var ptes = await _repository.CrmPTEInformations
						.CrmPTEInformationsAsync(false, cancellationToken);

		if (!ptes.Any())
		{
			_logger.LogWarning("No PTE informations found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<PTEInformationDto>();
		}

		var ptesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmPTEInformation, PTEInformationDto>(ptes);

		_logger.LogInformation("PTE informations fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						ptesDto.Count(), DateTime.UtcNow);

		return ptesDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all PTE informations.
	/// </summary>
	public async Task<GridEntity<PTEInformationDto>> PTEInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    pi.PTEInformationId,
                    pi.ApplicantId,
                    pi.PTEListening,
                    pi.PTEReading,
                    pi.PTEWriting,
                    pi.PTESpeaking,
                    pi.PTEOverallScore,
                    pi.PTEDate,
                    pi.PTEScannedCopyPath,
                    pi.PTEAdditionalInformation,
                    pi.CreatedDate,
                    pi.CreatedBy,
                    pi.UpdatedDate,
                    pi.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmPTEInformation pi
                LEFT JOIN CrmApplication app ON pi.ApplicantId = app.ApplicationId";

		const string orderBy = "pi.CreatedDate DESC";

		_logger.LogInformation("Fetching PTE informations summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmPTEInformations.AdoGridDataAsync<PTEInformationDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmPteInformation service implementing business logic for CrmPteInformation management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmPteInformationService : ICrmPteInformationService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmPteInformationService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmPteInformationService(IRepositoryManager repository, ILogger<CrmPteInformationService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmPteInformation records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmPteInformationDto>> CrmPteInformationSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmPteInformation summary grid");

//        string query = "SELECT * FROM CrmPteInformation";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmPteInformations.AdoGridDataAsync<CrmPteInformationDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmPteInformation records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmPteInformationDto>> CrmPteInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmPteInformation records");

//        var records = await _repository.CrmPteInformations.CrmPteInformationsAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmPteInformation records found");
//            return Enumerable.Empty<CrmPteInformationDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmPteInformation, CrmPteInformationDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmPteInformation record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmPteInformationDto> CrmPteInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmPteInformationAsync called with invalid id: {CrmPteInformationId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmPteInformation record with ID: {CrmPteInformationId}", id);

//        var record = await _repository.CrmPteInformations.CrmPteInformationAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmPteInformation record not found with ID: {CrmPteInformationId}", id);
//            throw new NotFoundException("CrmPteInformation", "CrmPteInformationId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmPteInformation, CrmPteInformationDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmPteInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmPteInformationDto> CreateAsync(CrmPteInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmPteInformationDto));

//        _logger.LogInformation("Creating new CrmPteInformation record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmPteInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmPteInformation", "Name");

//        // Map and create
//        CrmPteInformation entity = MyMapper.JsonClone<CrmPteInformationDto, CrmPteInformation>(modelDto);
//        modelDto.CrmPteInformationId = await _repository.CrmPteInformations.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPteInformation record created successfully with ID: {CrmPteInformationId}", modelDto.CrmPteInformationId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmPteInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmPteInformationDto> UpdateAsync(int key, CrmPteInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmPteInformationDto));

//        if (key != modelDto.CrmPteInformationId)
//            throw new BadRequestException(key.ToString(), nameof(CrmPteInformationDto));

//        _logger.LogInformation("Updating CrmPteInformation record with ID: {CrmPteInformationId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmPteInformations.ByIdAsync(
//            x => x.CrmPteInformationId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmPteInformation", "CrmPteInformationId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmPteInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmPteInformationId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmPteInformation", "Name");

//        // Map and update
//        CrmPteInformation entity = MyMapper.JsonClone<CrmPteInformationDto, CrmPteInformation>(modelDto);
//        _repository.CrmPteInformations.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPteInformation record updated successfully: {CrmPteInformationId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmPteInformation record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmPteInformation record with ID: {CrmPteInformationId}", key);

//        var record = await _repository.CrmPteInformations.ByIdAsync(
//            x => x.CrmPteInformationId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmPteInformation", "CrmPteInformationId", key.ToString());

//        await _repository.CrmPteInformations.DeleteAsync(x => x.CrmPteInformationId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmPteInformation record deleted successfully: {CrmPteInformationId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmPteInformation records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmPteInformationForDDLDto>> CrmPteInformationsForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmPteInformation records for dropdown list");

//        var records = await _repository.CrmPteInformations.ListWithSelectAsync(
//            x => new CrmPteInformation
//            {
//                CrmPteInformationId = x.CrmPteInformationId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmPteInformationForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmPteInformation, CrmPteInformationForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
