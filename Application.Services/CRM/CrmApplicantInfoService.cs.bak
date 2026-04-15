// CrmApplicantInfoService.cs
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
/// CRM Applicant Info service implementing business logic for applicant info management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmApplicantInfoService : ICrmApplicantInfoService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmApplicantInfoService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmApplicantInfoService"/> with required dependencies.
	/// </summary>
	public CrmApplicantInfoService(IRepositoryManager repository, ILogger<CrmApplicantInfoService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new applicant info record.
	/// </summary>
	public async Task<ApplicantInfoDto> CreateApplicantInfoAsync(ApplicantInfoDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(ApplicantInfoDto));

		if (entityForCreate.ApplicantId != 0)
			throw new InvalidCreateOperationException("ApplicantId must be 0 for new record.");

		if (!string.IsNullOrEmpty(entityForCreate.EmailAddress))
		{
			bool emailExists = await _repository.CrmApplicantInfoes.ExistsAsync(
							x => x.EmailAddress != null && x.EmailAddress.ToLower() == entityForCreate.EmailAddress.ToLower(),
							cancellationToken: cancellationToken);

			if (emailExists)
				throw new DuplicateRecordException("CrmApplicantInfo", "EmailAddress");
		}

		bool appExists = await _repository.CrmApplicantInfoes.ExistsAsync(
						x => x.ApplicationId == entityForCreate.ApplicationId,
						cancellationToken: cancellationToken);

		if (appExists)
			throw new DuplicateRecordException("CrmApplicantInfo", "ApplicationId");

		_logger.LogInformation("Creating new applicant info. Email: {EmailAddress}, ApplicationId: {ApplicationId}, Time: {Time}",
						entityForCreate.EmailAddress, entityForCreate.ApplicationId, DateTime.UtcNow);

		var infoEntity = MyMapper.JsonClone<ApplicantInfoDto, CrmApplicantInfo>(entityForCreate);
		infoEntity.CreatedDate = DateTime.UtcNow;
		infoEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmApplicantInfoes.CreateAsync(infoEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Applicant info could not be saved to the database.");

		_logger.LogInformation("Applicant info created successfully. ID: {ApplicantId}, Time: {Time}",
						infoEntity.ApplicantId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantInfo, ApplicantInfoDto>(infoEntity);
	}

	/// <summary>
	/// Updates an existing applicant info record.
	/// </summary>
	public async Task<ApplicantInfoDto> UpdateApplicantInfoAsync(int applicantId, ApplicantInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(ApplicantInfoDto));

		if (applicantId != modelDto.ApplicantId)
			throw new BadRequestException(applicantId.ToString(), nameof(ApplicantInfoDto));

		_logger.LogInformation("Updating applicant info. ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var applicantInfo = await _repository.CrmApplicantInfoes
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmApplicantInfo", "ApplicantId", applicantId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmApplicantInfo, ApplicantInfoDto>(applicantInfo, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmApplicantInfoes.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmApplicantInfo", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("Applicant info updated successfully. ID: {ApplicantId}, Time: {Time}",
						applicantId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantInfo, ApplicantInfoDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an applicant info record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteApplicantInfoAsync(int applicantId, ApplicantInfoDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(ApplicantInfoDto));

		if (applicantId != modelDto.ApplicantId)
			throw new BadRequestException(applicantId.ToString(), nameof(ApplicantInfoDto));

		_logger.LogInformation("Deleting applicant info. ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var infoEntity = await _repository.CrmApplicantInfoes
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmApplicantInfo", "ApplicantId", applicantId.ToString());

		await _repository.CrmApplicantInfoes.DeleteAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmApplicantInfo", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("Applicant info deleted successfully. ID: {ApplicantId}, Time: {Time}",
						applicantId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single applicant info record by its ID.
	/// </summary>
	public async Task<ApplicantInfoDto> ApplicantInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant info. ID: {ApplicantId}, Time: {Time}", id, DateTime.UtcNow);

		var info = await _repository.CrmApplicantInfoes
						.FirstOrDefaultAsync(x => x.ApplicantId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmApplicantInfo", "ApplicantId", id.ToString());

		_logger.LogInformation("Applicant info fetched successfully. ID: {ApplicantId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantInfo, ApplicantInfoDto>(info);
	}

	/// <summary>
	/// Retrieves a single applicant info record by application ID.
	/// </summary>
	public async Task<ApplicantInfoDto> ApplicantInfoByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant info by application ID: {ApplicationId}, Time: {Time}", applicationId, DateTime.UtcNow);

		var info = await _repository.CrmApplicantInfoes
						.FirstOrDefaultAsync(x => x.ApplicationId == applicationId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmApplicantInfo", "ApplicationId", applicationId.ToString());

		_logger.LogInformation("Applicant info fetched successfully. ID: {ApplicantId}, Time: {Time}",
						info.ApplicantId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantInfo, ApplicantInfoDto>(info);
	}

	/// <summary>
	/// Retrieves a single applicant info record by email address.
	/// </summary>
	public async Task<ApplicantInfoDto> ApplicantInfoByEmailAsync(string email, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			_logger.LogWarning("ApplicantInfoByEmailAsync called with null or whitespace email");
			throw new BadRequestException(nameof(email));
		}

		_logger.LogInformation("Fetching applicant info by email: {Email}, Time: {Time}", email, DateTime.UtcNow);

		var info = await _repository.CrmApplicantInfoes
						.FirstOrDefaultAsync(x => x.EmailAddress != null && x.EmailAddress.ToLower() == email.ToLower(), trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmApplicantInfo", "EmailAddress", email);

		_logger.LogInformation("Applicant info fetched successfully. ID: {ApplicantId}, Time: {Time}",
						info.ApplicantId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmApplicantInfo, ApplicantInfoDto>(info);
	}

	/// <summary>
	/// Retrieves all applicant info records from the database.
	/// </summary>
	public async Task<IEnumerable<ApplicantInfoDto>> ApplicantInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all applicant infos. Time: {Time}", DateTime.UtcNow);

		var infos = await _repository.CrmApplicantInfoes.CrmApplicantInfosAsync(trackChanges, cancellationToken);

		if (!infos.Any())
		{
			_logger.LogWarning("No applicant infos found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantInfoDto>();
		}

		var infosDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantInfo, ApplicantInfoDto>(infos);

		_logger.LogInformation("Applicant infos fetched successfully. Count: {Count}, Time: {Time}",
						infosDto.Count(), DateTime.UtcNow);

		return infosDto;
	}

	/// <summary>
	/// Retrieves active applicant info records from the database.
	/// </summary>
	public async Task<IEnumerable<ApplicantInfoDto>> ActiveApplicantInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active applicant infos. Time: {Time}", DateTime.UtcNow);

		var infos = await _repository.CrmApplicantInfoes.CrmApplicantInfosAsync(trackChanges, cancellationToken);

		if (!infos.Any())
		{
			_logger.LogWarning("No active applicant infos found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantInfoDto>();
		}

		var infosDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantInfo, ApplicantInfoDto>(infos);

		_logger.LogInformation("Active applicant infos fetched successfully. Count: {Count}, Time: {Time}",
						infosDto.Count(), DateTime.UtcNow);

		return infosDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all applicant infos suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<ApplicantInfoDto>> ApplicantInfoForDDLAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching applicant infos for dropdown list. Time: {Time}", DateTime.UtcNow);

		var infos = await _repository.CrmApplicantInfoes.CrmApplicantInfosAsync(trackChanges, cancellationToken);

		if (!infos.Any())
		{
			_logger.LogWarning("No applicant infos found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<ApplicantInfoDto>();
		}

		var infosDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmApplicantInfo, ApplicantInfoDto>(infos);

		_logger.LogInformation("Applicant infos fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						infosDto.Count(), DateTime.UtcNow);

		return infosDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all applicant infos.
	/// </summary>
	public async Task<GridEntity<ApplicantInfoDto>> ApplicantInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    ai.ApplicantId,
                    ai.ApplicationId,
                    ai.GenderId,
                    ai.GenderName,
                    ai.TitleValue,
                    ai.TitleText,
                    ai.FirstName,
                    ai.LastName,
                    ai.DateOfBirth,
                    ai.MaritalStatusId,
                    ai.MaritalStatusName,
                    ai.Nationality,
                    ai.HasValidPassport,
                    ai.PassportNumber,
                    ai.PassportIssueDate,
                    ai.PassportExpiryDate,
                    ai.PhoneCountryCode,
                    ai.PhoneAreaCode,
                    ai.PhoneNumber,
                    ai.Mobile,
                    ai.EmailAddress,
                    ai.SkypeId,
                    ai.ApplicantImagePath,
                    ai.CreatedDate,
                    ai.CreatedBy,
                    ai.UpdatedDate,
                    ai.UpdatedBy,
                    app.ApplicationStatus,
                    CONCAT(ai.TitleText, ' ', ai.FirstName, ' ', ai.LastName) AS FullName
                FROM CrmApplicantInfo ai
                LEFT JOIN CrmApplication app ON ai.ApplicationId = app.ApplicationId";

		const string orderBy = "ai.CreatedDate DESC";

		_logger.LogInformation("Fetching applicant infos summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmApplicantInfoes.AdoGridDataAsync<ApplicantInfoDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmApplicantInfo service implementing business logic for CrmApplicantInfo management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmApplicantInfoService : ICrmApplicantInfoService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmApplicantInfoService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmApplicantInfoService(IRepositoryManager repository, ILogger<CrmApplicantInfoService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmApplicantInfo records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmApplicantInfoDto>> CrmApplicantInfoSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmApplicantInfo summary grid");

//        string query = "SELECT * FROM CrmApplicantInfo";
//        string orderBy = "Title ASC";

//        var gridEntity = await _repository.CrmApplicantInfos.AdoGridDataAsync<CrmApplicantInfoDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmApplicantInfo records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmApplicantInfoDto>> CrmApplicantInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmApplicantInfo records");

//        var records = await _repository.CrmApplicantInfos.CrmApplicantInfosAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmApplicantInfo records found");
//            return Enumerable.Empty<CrmApplicantInfoDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmApplicantInfo, CrmApplicantInfoDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmApplicantInfo record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantInfoDto> CrmApplicantInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmApplicantInfoAsync called with invalid id: {CrmApplicantInfoId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmApplicantInfo record with ID: {CrmApplicantInfoId}", id);

//        var record = await _repository.CrmApplicantInfos.CrmApplicantInfoAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmApplicantInfo record not found with ID: {CrmApplicantInfoId}", id);
//            throw new NotFoundException("CrmApplicantInfo", "CrmApplicantInfoId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmApplicantInfo, CrmApplicantInfoDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmApplicantInfo record asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantInfoDto> CreateAsync(CrmApplicantInfoDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmApplicantInfoDto));

//        _logger.LogInformation("Creating new CrmApplicantInfo record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmApplicantInfos.ExistsAsync(
//            x => x.Title.Trim().ToLower() == modelDto.Title.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmApplicantInfo", "Title");

//        // Map and create
//        CrmApplicantInfo entity = MyMapper.JsonClone<CrmApplicantInfoDto, CrmApplicantInfo>(modelDto);
//        modelDto.CrmApplicantInfoId = await _repository.CrmApplicantInfos.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantInfo record created successfully with ID: {CrmApplicantInfoId}", modelDto.CrmApplicantInfoId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmApplicantInfo record asynchronously.
//    /// </summary>
//    public async Task<CrmApplicantInfoDto> UpdateAsync(int key, CrmApplicantInfoDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmApplicantInfoDto));

//        if (key != modelDto.CrmApplicantInfoId)
//            throw new BadRequestException(key.ToString(), nameof(CrmApplicantInfoDto));

//        _logger.LogInformation("Updating CrmApplicantInfo record with ID: {CrmApplicantInfoId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmApplicantInfos.ByIdAsync(
//            x => x.CrmApplicantInfoId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmApplicantInfo", "CrmApplicantInfoId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmApplicantInfos.ExistsAsync(
//            x => x.Title.Trim().ToLower() == modelDto.Title.Trim().ToLower() 
//                 && x.CrmApplicantInfoId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmApplicantInfo", "Title");

//        // Map and update
//        CrmApplicantInfo entity = MyMapper.JsonClone<CrmApplicantInfoDto, CrmApplicantInfo>(modelDto);
//        _repository.CrmApplicantInfos.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantInfo record updated successfully: {CrmApplicantInfoId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmApplicantInfo record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmApplicantInfo record with ID: {CrmApplicantInfoId}", key);

//        var record = await _repository.CrmApplicantInfos.ByIdAsync(
//            x => x.CrmApplicantInfoId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmApplicantInfo", "CrmApplicantInfoId", key.ToString());

//        await _repository.CrmApplicantInfos.DeleteAsync(x => x.CrmApplicantInfoId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmApplicantInfo record deleted successfully: {CrmApplicantInfoId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmApplicantInfo records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmApplicantInfoForDDLDto>> CrmApplicantInfosForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmApplicantInfo records for dropdown list");

//        var records = await _repository.CrmApplicantInfos.ListWithSelectAsync(
//            x => new CrmApplicantInfo
//            {
//                CrmApplicantInfoId = x.CrmApplicantInfoId,
//                Title = x.Title
//            },
//            orderBy: x => x.Title,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmApplicantInfoForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmApplicantInfo, CrmApplicantInfoForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
