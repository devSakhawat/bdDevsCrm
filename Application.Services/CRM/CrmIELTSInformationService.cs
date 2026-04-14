// CrmIELTSInformationService.cs
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
/// CRM IELTS Information service implementing business logic for IELTS information management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmIELTSInformationService : ICrmIELTSInformationService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmIELTSInformationService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmIELTSInformationService"/> with required dependencies.
	/// </summary>
	public CrmIELTSInformationService(IRepositoryManager repository, ILogger<CrmIELTSInformationService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new IELTS information record.
	/// </summary>
	public async Task<IELTSInformationDto> CreateIELTSInformationAsync(IELTSInformationDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(IELTSInformationDto));

		if (entityForCreate.IELTSInformationId != 0)
			throw new InvalidCreateOperationException("IELTSInformationId must be 0 for new record.");

		bool applicantExists = await _repository.CrmIELTSInformations.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmIELTSInformation", "ApplicantId");

		_logger.LogInformation("Creating new IELTS information. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var ieltsEntity = MyMapper.JsonClone<IELTSInformationDto, CrmIELTSInformation>(entityForCreate);
		ieltsEntity.CreatedDate = DateTime.UtcNow;
		ieltsEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmIELTSInformations.CreateAsync(ieltsEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("IELTS information could not be saved to the database.");

		_logger.LogInformation("IELTS information created successfully. ID: {IELTSInformationId}, Time: {Time}",
						ieltsEntity.IELTSInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmIELTSInformation, IELTSInformationDto>(ieltsEntity);
	}

	/// <summary>
	/// Updates an existing IELTS information record.
	/// </summary>
	public async Task<IELTSInformationDto> UpdateIELTSInformationAsync(int ieltsInformationId, IELTSInformationDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(IELTSInformationDto));

		if (ieltsInformationId != modelDto.IELTSInformationId)
			throw new BadRequestException(ieltsInformationId.ToString(), nameof(IELTSInformationDto));

		_logger.LogInformation("Updating IELTS information. ID: {IELTSInformationId}, Time: {Time}", ieltsInformationId, DateTime.UtcNow);

		var ieltsEntity = await _repository.CrmIELTSInformations
						.FirstOrDefaultAsync(x => x.IELTSInformationId == ieltsInformationId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmIELTSInformation", "IELTSInformationId", ieltsInformationId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmIELTSInformation, IELTSInformationDto>(ieltsEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmIELTSInformations.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmIELTSInformation", "IELTSInformationId", ieltsInformationId.ToString());

		_logger.LogInformation("IELTS information updated successfully. ID: {IELTSInformationId}, Time: {Time}",
						ieltsInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmIELTSInformation, IELTSInformationDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an IELTS information record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteIELTSInformationAsync(int ieltsInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (ieltsInformationId <= 0)
			throw new BadRequestException(ieltsInformationId.ToString(), nameof(IELTSInformationDto));

		_logger.LogInformation("Deleting IELTS information. ID: {IELTSInformationId}, Time: {Time}", ieltsInformationId, DateTime.UtcNow);

		var ieltsEntity = await _repository.CrmIELTSInformations
						.FirstOrDefaultAsync(x => x.IELTSInformationId == ieltsInformationId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmIELTSInformation", "IELTSInformationId", ieltsInformationId.ToString());

		await _repository.CrmIELTSInformations.DeleteAsync(x => x.IELTSInformationId == ieltsInformationId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmIELTSInformation", "IELTSInformationId", ieltsInformationId.ToString());

		_logger.LogInformation("IELTS information deleted successfully. ID: {IELTSInformationId}, Time: {Time}",
						ieltsInformationId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single IELTS information record by its ID.
	/// </summary>
	public async Task<IELTSInformationDto> IELTSInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching IELTS information. ID: {IELTSInformationId}, Time: {Time}", id, DateTime.UtcNow);

		var ielts = await _repository.CrmIELTSInformations
						.FirstOrDefaultAsync(x => x.IELTSInformationId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmIELTSInformation", "IELTSInformationId", id.ToString());

		_logger.LogInformation("IELTS information fetched successfully. ID: {IELTSInformationId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmIELTSInformation, IELTSInformationDto>(ielts);
	}

	/// <summary>
	/// Retrieves all IELTS information records from the database.
	/// </summary>
	public async Task<IEnumerable<IELTSInformationDto>> IELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all IELTS informations. Time: {Time}", DateTime.UtcNow);

		var ielts = await _repository.CrmIELTSInformations
						.CrmIELTSInformationsAsync(trackChanges, cancellationToken);

		if (!ielts.Any())
		{
			_logger.LogWarning("No IELTS informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<IELTSInformationDto>();
		}

		var ieltsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmIELTSInformation, IELTSInformationDto>(ielts);

		_logger.LogInformation("IELTS informations fetched successfully. Count: {Count}, Time: {Time}",
						ieltsDto.Count(), DateTime.UtcNow);

		return ieltsDto;
	}

	/// <summary>
	/// Retrieves active IELTS information records from the database.
	/// </summary>
	public async Task<IEnumerable<IELTSInformationDto>> ActiveIELTSInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active IELTS informations. Time: {Time}", DateTime.UtcNow);

		var ielts = await _repository.CrmIELTSInformations
						.CrmIELTSInformationsAsync(trackChanges, cancellationToken);

		if (!ielts.Any())
		{
			_logger.LogWarning("No active IELTS informations found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<IELTSInformationDto>();
		}

		var ieltsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmIELTSInformation, IELTSInformationDto>(ielts);

		_logger.LogInformation("Active IELTS informations fetched successfully. Count: {Count}, Time: {Time}",
						ieltsDto.Count(), DateTime.UtcNow);

		return ieltsDto;
	}

	/// <summary>
	/// Retrieves IELTS information by the specified applicant ID.
	/// </summary>
	public async Task<IELTSInformationDto> IELTSInformationByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("IELTSInformationByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching IELTS information for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var ielts = await _repository.CrmIELTSInformations
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmIELTSInformation", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("IELTS information fetched successfully. ID: {IELTSInformationId}, Time: {Time}",
						ielts.IELTSInformationId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmIELTSInformation, IELTSInformationDto>(ielts);
	}

	/// <summary>
	/// Retrieves a lightweight list of all IELTS informations suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<IELTSInformationDto>> IELTSInformationForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching IELTS informations for dropdown list. Time: {Time}", DateTime.UtcNow);

		var ielts = await _repository.CrmIELTSInformations.CrmIELTSInformationsAsync(false, cancellationToken);

		if (!ielts.Any())
		{
			_logger.LogWarning("No IELTS informations found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<IELTSInformationDto>();
		}

		var ieltsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmIELTSInformation, IELTSInformationDto>(ielts);

		_logger.LogInformation("IELTS informations fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						ieltsDto.Count(), DateTime.UtcNow);

		return ieltsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all IELTS informations.
	/// </summary>
	public async Task<GridEntity<IELTSInformationDto>> IELTSInformationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    ii.IELTSInformationId,
                    ii.ApplicantId,
                    ii.IELTSlistening,
                    ii.IELTSreading,
                    ii.IELTSwriting,
                    ii.IELTSspeaking,
                    ii.IELTSoverallScore,
                    ii.IELTSdate,
                    ii.IELTSscannedCopyPath,
                    ii.IELTSadditionalInformation,
                    ii.CreatedDate,
                    ii.CreatedBy,
                    ii.UpdatedDate,
                    ii.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmIELTSInformation ii
                LEFT JOIN CrmApplication app ON ii.ApplicantId = app.ApplicationId";

		const string orderBy = "ii.CreatedDate DESC";

		_logger.LogInformation("Fetching IELTS informations summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmIELTSInformations.AdoGridDataAsync<IELTSInformationDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmIeltsInformation service implementing business logic for CrmIeltsInformation management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmIeltsInformationService : ICrmIeltsInformationService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmIeltsInformationService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmIeltsInformationService(IRepositoryManager repository, ILogger<CrmIeltsInformationService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmIeltsInformation records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmIeltsInformationDto>> CrmIeltsInformationSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmIeltsInformation summary grid");

//        string query = "SELECT * FROM CrmIeltsInformation";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmIeltsInformations.AdoGridDataAsync<CrmIeltsInformationDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmIeltsInformation records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmIeltsInformationDto>> CrmIeltsInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmIeltsInformation records");

//        var records = await _repository.CrmIeltsInformations.CrmIeltsInformationsAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmIeltsInformation records found");
//            return Enumerable.Empty<CrmIeltsInformationDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmIeltsInformation, CrmIeltsInformationDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmIeltsInformation record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmIeltsInformationDto> CrmIeltsInformationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmIeltsInformationAsync called with invalid id: {CrmIeltsInformationId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmIeltsInformation record with ID: {CrmIeltsInformationId}", id);

//        var record = await _repository.CrmIeltsInformations.CrmIeltsInformationAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmIeltsInformation record not found with ID: {CrmIeltsInformationId}", id);
//            throw new NotFoundException("CrmIeltsInformation", "CrmIeltsInformationId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmIeltsInformation, CrmIeltsInformationDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmIeltsInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmIeltsInformationDto> CreateAsync(CrmIeltsInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmIeltsInformationDto));

//        _logger.LogInformation("Creating new CrmIeltsInformation record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmIeltsInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmIeltsInformation", "Name");

//        // Map and create
//        CrmIeltsInformation entity = MyMapper.JsonClone<CrmIeltsInformationDto, CrmIeltsInformation>(modelDto);
//        modelDto.CrmIeltsInformationId = await _repository.CrmIeltsInformations.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmIeltsInformation record created successfully with ID: {CrmIeltsInformationId}", modelDto.CrmIeltsInformationId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmIeltsInformation record asynchronously.
//    /// </summary>
//    public async Task<CrmIeltsInformationDto> UpdateAsync(int key, CrmIeltsInformationDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmIeltsInformationDto));

//        if (key != modelDto.CrmIeltsInformationId)
//            throw new BadRequestException(key.ToString(), nameof(CrmIeltsInformationDto));

//        _logger.LogInformation("Updating CrmIeltsInformation record with ID: {CrmIeltsInformationId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmIeltsInformations.ByIdAsync(
//            x => x.CrmIeltsInformationId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmIeltsInformation", "CrmIeltsInformationId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmIeltsInformations.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmIeltsInformationId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmIeltsInformation", "Name");

//        // Map and update
//        CrmIeltsInformation entity = MyMapper.JsonClone<CrmIeltsInformationDto, CrmIeltsInformation>(modelDto);
//        _repository.CrmIeltsInformations.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmIeltsInformation record updated successfully: {CrmIeltsInformationId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmIeltsInformation record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmIeltsInformation record with ID: {CrmIeltsInformationId}", key);

//        var record = await _repository.CrmIeltsInformations.ByIdAsync(
//            x => x.CrmIeltsInformationId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmIeltsInformation", "CrmIeltsInformationId", key.ToString());

//        await _repository.CrmIeltsInformations.DeleteAsync(x => x.CrmIeltsInformationId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmIeltsInformation record deleted successfully: {CrmIeltsInformationId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmIeltsInformation records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmIeltsInformationForDDLDto>> CrmIeltsInformationsForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmIeltsInformation records for dropdown list");

//        var records = await _repository.CrmIeltsInformations.ListWithSelectAsync(
//            x => new CrmIeltsInformation
//            {
//                CrmIeltsInformationId = x.CrmIeltsInformationId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmIeltsInformationForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmIeltsInformation, CrmIeltsInformationForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
