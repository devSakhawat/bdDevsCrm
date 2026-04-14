// CrmApplicantReferenceService.cs
using bdDevCRM.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.ServiceContract.CRM;
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
/// CRM Applicant Reference service implementing business logic for applicant reference management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmApplicantReferenceService : ICrmApplicantReferenceService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmApplicantReferenceService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmApplicantReferenceService"/> with required dependencies.
	/// </summary>
	public CrmApplicantReferenceService(IRepositoryManager repository, ILogger<CrmApplicantReferenceService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new applicant reference record.
	/// </summary>
	public async Task<ApplicantReferenceDto> CreateApplicantReferenceAsync(ApplicantReferenceDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(ApplicantReferenceDto));

		if (entityForCreate.ApplicantReferenceId != 0)
			throw new InvalidCreateOperationException("ApplicantReferenceId must be 0 for new record.");

		_logger.LogInformation("Creating new applicant reference. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var referenceEntity = MyMapper.JsonClone<ApplicantReferenceDto, CrmApplicantReference>(entityForCreate);
		referenceEntity.CreatedDate = DateTime.UtcNow;
		referenceEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmApplicantReferences.CreateAsync(referenceEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Applicant reference could not be saved to the database.");

		_logger.LogInformation("Applicant reference created successfully. ID: {ApplicantReferenceId}, Time: {Time}",
						referenceEntity.ApplicantReferenceId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantReference, ApplicantReferenceDto>(referenceEntity);
	}

	/// <summary>
	/// Updates an existing applicant reference record.
	/// </summary>
	public async Task<ApplicantReferenceDto> UpdateApplicantReferenceAsync(int applicantReferenceId, ApplicantReferenceDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(ApplicantReferenceDto));

		if (applicantReferenceId != modelDto.ApplicantReferenceId)
			throw new BadRequestException(applicantReferenceId.ToString(), nameof(ApplicantReferenceDto));

		_logger.LogInformation("Updating applicant reference. ID: {ApplicantReferenceId}, Time: {Time}", applicantReferenceId, DateTime.UtcNow);

		var referenceEntity = await _repository.CrmApplicantReferences
						.FirstOrDefaultAsync(x => x.ApplicantReferenceId == applicantReferenceId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("ApplicantReference", "ApplicantReferenceId", applicantReferenceId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmApplicantReference, ApplicantReferenceDto>(referenceEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmApplicantReferences.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("ApplicantReference", "ApplicantReferenceId", applicantReferenceId.ToString());

		_logger.LogInformation("Applicant reference updated successfully. ID: {ApplicantReferenceId}, Time: {Time}",
						applicantReferenceId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantReference, ApplicantReferenceDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an applicant reference record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteApplicantReferenceAsync(int applicantReferenceId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantReferenceId <= 0)
			throw new BadRequestException(applicantReferenceId.ToString(), nameof(ApplicantReferenceDto));

		_logger.LogInformation("Deleting applicant reference. ID: {ApplicantReferenceId}, Time: {Time}", applicantReferenceId, DateTime.UtcNow);

		var referenceEntity = await _repository.CrmApplicantReferences
						.FirstOrDefaultAsync(x => x.ApplicantReferenceId == applicantReferenceId, trackChanges, cancellationToken)
						?? throw new NotFoundException("ApplicantReference", "ApplicantReferenceId", applicantReferenceId.ToString());

		await _repository.CrmApplicantReferences.DeleteAsync(x => x.ApplicantReferenceId == applicantReferenceId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("ApplicantReference", "ApplicantReferenceId", applicantReferenceId.ToString());

		_logger.LogInformation("Applicant reference deleted successfully. ID: {ApplicantReferenceId}, Time: {Time}",
						applicantReferenceId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single applicant reference record by its ID.
	/// </summary>
	public async Task<ApplicantReferenceDto> ApplicantReferenceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant reference. ID: {ApplicantReferenceId}, Time: {Time}", id, DateTime.UtcNow);

		var reference = await _repository.CrmApplicantReferences
						.FirstOrDefaultAsync(x => x.ApplicantReferenceId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("ApplicantReference", "ApplicantReferenceId", id.ToString());

		_logger.LogInformation("Applicant reference fetched successfully. ID: {ApplicantReferenceId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantReference, ApplicantReferenceDto>(reference);
	}

	/// <summary>
	/// Retrieves all applicant reference records from the database.
	/// </summary>
	public async Task<IEnumerable<ApplicantReferenceDto>> ApplicantReferencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all applicant references. Time: {Time}", DateTime.UtcNow);

		var references = await _repository.CrmApplicantReferences.CrmApplicantReferencesAsync(trackChanges, cancellationToken);

		if (!references.Any())
		{
			_logger.LogWarning("No applicant references found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantReferenceDto>();
		}

		var referencesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantReference, ApplicantReferenceDto>(references);

		_logger.LogInformation("Applicant references fetched successfully. Count: {Count}, Time: {Time}",
						referencesDto.Count(), DateTime.UtcNow);

		return referencesDto;
	}

	/// <summary>
	/// Retrieves active applicant reference records from the database.
	/// </summary>
	public async Task<IEnumerable<ApplicantReferenceDto>> ActiveApplicantReferencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active applicant references. Time: {Time}", DateTime.UtcNow);

		var references = await _repository.CrmApplicantReferences.CrmApplicantReferencesAsync(trackChanges, cancellationToken);

		if (!references.Any())
		{
			_logger.LogWarning("No active applicant references found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantReferenceDto>();
		}

		var referencesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantReference, ApplicantReferenceDto>(references);

		_logger.LogInformation("Active applicant references fetched successfully. Count: {Count}, Time: {Time}",
						referencesDto.Count(), DateTime.UtcNow);

		return referencesDto;
	}

	/// <summary>
	/// Retrieves applicant references by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<ApplicantReferenceDto>> ApplicantReferencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("ApplicantReferencesByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching applicant references for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var references = await _repository.CrmApplicantReferences.CrmApplicantReferencesByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!references.Any())
		{
			_logger.LogWarning("No applicant references found for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
			return Enumerable.Empty<ApplicantReferenceDto>();
		}

		var referencesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantReference, ApplicantReferenceDto>(references);

		_logger.LogInformation("Applicant references fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, referencesDto.Count(), DateTime.UtcNow);

		return referencesDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all applicant references suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<ApplicantReferenceDDLDto>> ApplicantReferenceForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant references for dropdown list. Time: {Time}", DateTime.UtcNow);

		var referenceDDL = await _repository.CrmApplicantReferences.ListWithSelectAsync(selector: x => new ApplicantReferenceDDLDto
		{
			ApplicantReferenceId = x.ApplicantReferenceId,
			Name = x.Name
		}
		, orderBy: x => x.ApplicantId
		, trackChanges: false
		, cancellationToken: cancellationToken);


		//var references = await _repository.CrmApplicantReferences.GetActiveApplicantReferencesAsync(false, cancellationToken);

		if (!referenceDDL.Any())
		{
			_logger.LogWarning("No applicant references found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantReferenceDDLDto>();
		}

		_logger.LogInformation("Applicant references fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						referenceDDL.Count(), DateTime.UtcNow);

		return referenceDDL;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all applicant references.
	/// </summary>
	public async Task<GridEntity<ApplicantReferenceDto>> ApplicantReferencesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    ar.ApplicantReferenceId,
                    ar.ApplicantId,
                    ar.Name,
                    ar.Designation,
                    ar.Institution,
                    ar.EmailId,
                    ar.PhoneNo,
                    ar.FaxNo,
                    ar.Address,
                    ar.City,
                    ar.State,
                    ar.Country,
                    ar.PostOrZipCode,
                    ar.CreatedDate,
                    ar.CreatedBy,
                    ar.UpdatedDate,
                    ar.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmApplicantReference ar
                LEFT JOIN CrmApplication app ON ar.ApplicantId = app.ApplicationId";

		const string orderBy = "ar.CreatedDate DESC";

		_logger.LogInformation("Fetching applicant references summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmApplicantReferences.AdoGridDataAsync<ApplicantReferenceDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmApplicantReference service implementing business logic for CrmApplicantReference management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmApplicantReferenceService : ICrmApplicantReferenceService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmApplicantReferenceService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmApplicantReferenceService(IRepositoryManager repository, ILogger<CrmApplicantReferenceService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmApplicantReference records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmApplicantReferenceDto>> CrmApplicantReferenceSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmApplicantReference summary grid");

//        string query = "SELECT * FROM CrmApplicantReference";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmApplicantReferences.AdoGridDataAsync<CrmApplicantReferenceDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmApplicantReference records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmApplicantReferenceDto>> CrmApplicantReferencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmApplicantReference records");

//        var records = await _repository.CrmApplicantReferences.CrmApplicantReferencesAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmApplicantReference records found");
//            return Enumerable.Empty<CrmApplicantReferenceDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmApplicantReference, CrmApplicantReferenceDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmApplicantReference record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantReferenceDto> CrmApplicantReferenceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmApplicantReferenceAsync called with invalid id: {CrmApplicantReferenceId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmApplicantReference record with ID: {CrmApplicantReferenceId}", id);

//        var record = await _repository.CrmApplicantReferences.CrmApplicantReferenceAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmApplicantReference record not found with ID: {CrmApplicantReferenceId}", id);
//            throw new NotFoundException("CrmApplicantReference", "CrmApplicantReferenceId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmApplicantReference, CrmApplicantReferenceDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmApplicantReference record asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantReferenceDto> CreateAsync(CrmApplicantReferenceDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmApplicantReferenceDto));

//        _logger.LogInformation("Creating new CrmApplicantReference record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmApplicantReferences.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmApplicantReference", "Name");

//        // Map and create
//        CrmApplicantReference entity = MyMapper.JsonClone<CrmApplicantReferenceDto, CrmApplicantReference>(modelDto);
//        modelDto.CrmApplicantReferenceId = await _repository.CrmApplicantReferences.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantReference record created successfully with ID: {CrmApplicantReferenceId}", modelDto.CrmApplicantReferenceId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmApplicantReference record asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantReferenceDto> UpdateAsync(int key, CrmApplicantReferenceDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmApplicantReferenceDto));

//        if (key != modelDto.CrmApplicantReferenceId)
//            throw new BadRequestException(key.ToString(), nameof(CrmApplicantReferenceDto));

//        _logger.LogInformation("Updating CrmApplicantReference record with ID: {CrmApplicantReferenceId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmApplicantReferences.ByIdAsync(
//            x => x.CrmApplicantReferenceId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmApplicantReference", "CrmApplicantReferenceId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmApplicantReferences.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmApplicantReferenceId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmApplicantReference", "Name");

//        // Map and update
//        CrmApplicantReference entity = MyMapper.JsonClone<CrmApplicantReferenceDto, CrmApplicantReference>(modelDto);
//        _repository.CrmApplicantReferences.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantReference record updated successfully: {CrmApplicantReferenceId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmApplicantReference record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmApplicantReference record with ID: {CrmApplicantReferenceId}", key);

//        var record = await _repository.CrmApplicantReferences.ByIdAsync(
//            x => x.CrmApplicantReferenceId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmApplicantReference", "CrmApplicantReferenceId", key.ToString());

//        await _repository.CrmApplicantReferences.DeleteAsync(x => x.CrmApplicantReferenceId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantReference record deleted successfully: {CrmApplicantReferenceId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmApplicantReference records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmApplicantReferenceForDDLDto>> CrmApplicantReferencesForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmApplicantReference records for dropdown list");

//        var records = await _repository.CrmApplicantReferences.ListWithSelectAsync(
//            x => new CrmApplicantReference
//            {
//                CrmApplicantReferenceId = x.CrmApplicantReferenceId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmApplicantReferenceForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmApplicantReference, CrmApplicantReferenceForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
