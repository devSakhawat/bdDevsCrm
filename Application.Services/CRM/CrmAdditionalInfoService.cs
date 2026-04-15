// CrmAdditionalInfoService.cs
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
/// CRM Additional Info service implementing business logic for additional info management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmAdditionalInfoService : ICrmAdditionalInfoService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmAdditionalInfoService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmAdditionalInfoService"/> with required dependencies.
	/// </summary>
	public CrmAdditionalInfoService(IRepositoryManager repository, ILogger<CrmAdditionalInfoService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new additional info record.
	/// </summary>
	public async Task<AdditionalInfoDto> CreateAdditionalInfoAsync(AdditionalInfoDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(AdditionalInfoDto));

		if (entityForCreate.AdditionalInfoId != 0)
			throw new InvalidCreateOperationException("AdditionalInfoId must be 0 for new record.");

		_logger.LogInformation("Creating new additional info. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var infoEntity = MyMapper.JsonClone<AdditionalInfoDto, CrmAdditionalInfo>(entityForCreate);
		infoEntity.CreatedDate = DateTime.UtcNow;
		infoEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmAdditionalInfoes.CreateAsync(infoEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Additional info could not be saved to the database.");

		_logger.LogInformation("Additional info created successfully. ID: {AdditionalInfoId}, Time: {Time}",
						infoEntity.AdditionalInfoId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmAdditionalInfo, AdditionalInfoDto>(infoEntity);
	}

	/// <summary>
	/// Updates an existing additional info record.
	/// </summary>
	public async Task<AdditionalInfoDto> UpdateAdditionalInfoAsync(int additionalInfoId, AdditionalInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(AdditionalInfoDto));

		if (additionalInfoId != modelDto.AdditionalInfoId)
			throw new BadRequestException(additionalInfoId.ToString(), nameof(AdditionalInfoDto));

		_logger.LogInformation("Updating additional info. ID: {AdditionalInfoId}, Time: {Time}", additionalInfoId, DateTime.UtcNow);

		var infoEntity = await _repository.CrmAdditionalInfoes
						.FirstOrDefaultAsync(x => x.AdditionalInfoId == additionalInfoId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmAdditionalInfo", "AdditionalInfoId", additionalInfoId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmAdditionalInfo, AdditionalInfoDto>(infoEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmAdditionalInfoes.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmAdditionalInfo", "AdditionalInfoId", additionalInfoId.ToString());

		_logger.LogInformation("Additional info updated successfully. ID: {AdditionalInfoId}, Time: {Time}",
						additionalInfoId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmAdditionalInfo, AdditionalInfoDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an additional info record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteAdditionalInfoAsync(int additionalInfoId, AdditionalInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(AdditionalInfoDto));

		if (additionalInfoId != modelDto.AdditionalInfoId)
			throw new BadRequestException(additionalInfoId.ToString(), nameof(AdditionalInfoDto));

		_logger.LogInformation("Deleting additional info. ID: {AdditionalInfoId}, Time: {Time}", additionalInfoId, DateTime.UtcNow);

		var infoEntity = await _repository.CrmAdditionalInfoes
						.FirstOrDefaultAsync(x => x.AdditionalInfoId == additionalInfoId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmAdditionalInfo", "AdditionalInfoId", additionalInfoId.ToString());

		await _repository.CrmAdditionalInfoes.DeleteAsync(x => x.AdditionalInfoId == additionalInfoId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmAdditionalInfo", "AdditionalInfoId", additionalInfoId.ToString());

		_logger.LogInformation("Additional info deleted successfully. ID: {AdditionalInfoId}, Time: {Time}",
						additionalInfoId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single additional info record by its ID.
	/// </summary>
	public async Task<AdditionalInfoDto> AdditionalInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching additional info. ID: {AdditionalInfoId}, Time: {Time}", id, DateTime.UtcNow);

		var info = await _repository.CrmAdditionalInfoes
						.FirstOrDefaultAsync(x => x.AdditionalInfoId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmAdditionalInfo", "AdditionalInfoId", id.ToString());

		_logger.LogInformation("Additional info fetched successfully. ID: {AdditionalInfoId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmAdditionalInfo, AdditionalInfoDto>(info);
	}

	/// <summary>
	/// Retrieves all additional info records from the database.
	/// </summary>
	public async Task<IEnumerable<AdditionalInfoDto>> AdditionalInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all additional infos. Time: {Time}", DateTime.UtcNow);

		var infos = await _repository.CrmAdditionalInfoes.CrmAdditionalInfosAsync(trackChanges, cancellationToken);

		if (!infos.Any())
		{
			_logger.LogWarning("No additional infos found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<AdditionalInfoDto>();
		}

		var infosDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalInfo, AdditionalInfoDto>(infos);

		_logger.LogInformation("Additional infos fetched successfully. Count: {Count}, Time: {Time}",
						infosDto.Count(), DateTime.UtcNow);

		return infosDto;
	}

	/// <summary>
	/// Retrieves active additional info records from the database.
	/// </summary>
	public async Task<IEnumerable<AdditionalInfoDto>> ActiveAdditionalInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active additional infos. Time: {Time}", DateTime.UtcNow);

		var infos = await _repository.CrmAdditionalInfoes.CrmAdditionalInfosAsync(trackChanges, cancellationToken);

		if (!infos.Any())
		{
			_logger.LogWarning("No active additional infos found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<AdditionalInfoDto>();
		}

		var infosDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalInfo, AdditionalInfoDto>(infos);

		_logger.LogInformation("Active additional infos fetched successfully. Count: {Count}, Time: {Time}",
						infosDto.Count(), DateTime.UtcNow);

		return infosDto;
	}

	/// <summary>
	/// Retrieves additional infos by the specified applicant ID.
	/// </summary>
	public async Task<IEnumerable<AdditionalInfoDto>> AdditionalInfosByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("AdditionalInfosByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching additional infos for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var infos = await _repository.CrmAdditionalInfoes.CrmAdditionalInfosByApplicantIdAsync(applicantId, trackChanges, cancellationToken);

		if (!infos.Any())
		{
			_logger.LogWarning("No additional infos found for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);
			return Enumerable.Empty<AdditionalInfoDto>();
		}

		var infosDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalInfo, AdditionalInfoDto>(infos);

		_logger.LogInformation("Additional infos fetched successfully for applicant ID: {ApplicantId}. Count: {Count}, Time: {Time}",
						applicantId, infosDto.Count(), DateTime.UtcNow);

		return infosDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all additional infos suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<AdditionalInfoDto>> AdditionalInfoForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching additional infos for dropdown list. Time: {Time}", DateTime.UtcNow);

		var infos = await _repository.CrmAdditionalInfoes.CrmAdditionalInfosAsync(trackChanges, cancellationToken);

		if (!infos.Any())
		{
			_logger.LogWarning("No additional infos found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<AdditionalInfoDto>();
		}

		var infosDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmAdditionalInfo, AdditionalInfoDto>(infos);

		_logger.LogInformation("Additional infos fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						infosDto.Count(), DateTime.UtcNow);

		return infosDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all additional infos.
	/// </summary>
	public async Task<GridEntity<AdditionalInfoDto>> AdditionalInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    ai.AdditionalInfoId,
                    ai.ApplicantId,
                    ai.RequireAccommodation,
                    ai.HealthNmedicalNeeds,
                    ai.HealthNmedicalNeedsRemarks,
                    ai.AdditionalInformationRemarks,
                    ai.DocumentTitle,
                    ai.UploadFile,
                    ai.DocumentName,
                    ai.FileThumbnail,
                    ai.RecordType,
                    ai.CreatedDate,
                    ai.CreatedBy,
                    ai.UpdatedDate,
                    ai.UpdatedBy,
                    app.ApplicationStatus
                FROM AdditionalInfo ai
                LEFT JOIN CrmApplication app ON ai.ApplicantId = app.ApplicationId";

		const string orderBy = "ai.CreatedDate DESC";

		_logger.LogInformation("Fetching additional infos summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmAdditionalInfoes.AdoGridDataAsync<AdditionalInfoDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmAdditionalInfo service implementing business logic for CrmAdditionalInfo management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmAdditionalInfoService : ICrmAdditionalInfoService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmAdditionalInfoService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmAdditionalInfoService(IRepositoryManager repository, ILogger<CrmAdditionalInfoService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmAdditionalInfo records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmAdditionalInfoDto>> CrmAdditionalInfoSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmAdditionalInfo summary grid");

//        string query = "SELECT * FROM CrmAdditionalInfo";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmAdditionalInfos.AdoGridDataAsync<CrmAdditionalInfoDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmAdditionalInfo records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmAdditionalInfoDto>> CrmAdditionalInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmAdditionalInfo records");

//        var records = await _repository.CrmAdditionalInfos.CrmAdditionalInfosAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmAdditionalInfo records found");
//            return Enumerable.Empty<CrmAdditionalInfoDto>();
//        }

//        var recordDtos = records.MapToList<CrmAdditionalInfoDto>();
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmAdditionalInfo record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmAdditionalInfoDto> CrmAdditionalInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmAdditionalInfoAsync called with invalid id: {CrmAdditionalInfoId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmAdditionalInfo record with ID: {CrmAdditionalInfoId}", id);

//        var record = await _repository.CrmAdditionalInfos.CrmAdditionalInfoAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmAdditionalInfo record not found with ID: {CrmAdditionalInfoId}", id);
//            throw new NotFoundException("CrmAdditionalInfo", "CrmAdditionalInfoId", id.ToString());
//        }

//        var recordDto = record.MapTo<CrmAdditionalInfoDto>();
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmAdditionalInfo record asynchronously.
//    /// </summary>
//    public async Task<CrmAdditionalInfoDto> CreateAsync(CrmAdditionalInfoDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmAdditionalInfoDto));

//        _logger.LogInformation("Creating new CrmAdditionalInfo record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmAdditionalInfos.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmAdditionalInfo", "Name");

//        // Map and create
//        CrmAdditionalInfo entity = modelDto.MapTo<CrmAdditionalInfo>();
//        modelDto.CrmAdditionalInfoId = await _repository.CrmAdditionalInfos.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmAdditionalInfo record created successfully with ID: {CrmAdditionalInfoId}", modelDto.CrmAdditionalInfoId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmAdditionalInfo record asynchronously.
//    /// </summary>
//    public async Task<CrmAdditionalInfoDto> UpdateAsync(int key, CrmAdditionalInfoDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmAdditionalInfoDto));

//        if (key != modelDto.CrmAdditionalInfoId)
//            throw new BadRequestException(key.ToString(), nameof(CrmAdditionalInfoDto));

//        _logger.LogInformation("Updating CrmAdditionalInfo record with ID: {CrmAdditionalInfoId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmAdditionalInfos.ByIdAsync(
//            x => x.CrmAdditionalInfoId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmAdditionalInfo", "CrmAdditionalInfoId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmAdditionalInfos.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmAdditionalInfoId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmAdditionalInfo", "Name");

//        // Map and update
//        CrmAdditionalInfo entity = modelDto.MapTo<CrmAdditionalInfo>();
//        _repository.CrmAdditionalInfos.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmAdditionalInfo record updated successfully: {CrmAdditionalInfoId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmAdditionalInfo record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmAdditionalInfo record with ID: {CrmAdditionalInfoId}", key);

//        var record = await _repository.CrmAdditionalInfos.ByIdAsync(
//            x => x.CrmAdditionalInfoId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmAdditionalInfo", "CrmAdditionalInfoId", key.ToString());

//        await _repository.CrmAdditionalInfos.DeleteAsync(x => x.CrmAdditionalInfoId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmAdditionalInfo record deleted successfully: {CrmAdditionalInfoId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmAdditionalInfo records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmAdditionalInfoForDDLDto>> CrmAdditionalInfosForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmAdditionalInfo records for dropdown list");

//        var records = await _repository.CrmAdditionalInfos.ListWithSelectAsync(
//            x => new CrmAdditionalInfo
//            {
//                CrmAdditionalInfoId = x.CrmAdditionalInfoId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmAdditionalInfoForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmAdditionalInfo, CrmAdditionalInfoForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
