// CrmStatementOfPurposeService.cs
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
/// CRM Statement of Purpose service implementing business logic for statement of purpose management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmStatementOfPurposeService : ICrmStatementOfPurposeService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmStatementOfPurposeService> _logger;
	private readonly IConfiguration _config;
	private readonly IHttpContextAccessor _httpContextAccessor;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmStatementOfPurposeService"/> with required dependencies.
	/// </summary>
	public CrmStatementOfPurposeService(IRepositoryManager repository, ILogger<CrmStatementOfPurposeService> logger, IConfiguration config, IHttpContextAccessor httpContextAccessor)
	{
		_repository = repository;
		_logger = logger;
		_config = config;
		_httpContextAccessor = httpContextAccessor;
	}

	/// <summary>
	/// Creates a new statement of purpose record.
	/// </summary>
	public async Task<StatementOfPurposeDto> CreateStatementOfPurposeAsync(StatementOfPurposeDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(StatementOfPurposeDto));

		if (entityForCreate.StatementOfPurposeId != 0)
			throw new InvalidCreateOperationException("StatementOfPurposeId must be 0 for new record.");

		bool applicantExists = await _repository.CrmStatementOfPurposes.ExistsAsync(
						x => x.ApplicantId == entityForCreate.ApplicantId,
						cancellationToken: cancellationToken);

		if (applicantExists)
			throw new DuplicateRecordException("CrmStatementOfPurpose", "ApplicantId");

		_logger.LogInformation("Creating new statement of purpose. ApplicantId: {ApplicantId}, Time: {Time}",
						entityForCreate.ApplicantId, DateTime.UtcNow);

		var sopEntity = MyMapper.JsonClone<StatementOfPurposeDto, CrmStatementOfPurpose>(entityForCreate);
		sopEntity.CreatedDate = DateTime.UtcNow;
		sopEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmStatementOfPurposes.CreateAsync(sopEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Statement of purpose could not be saved to the database.");

		_logger.LogInformation("Statement of purpose created successfully. ID: {StatementOfPurposeId}, Time: {Time}",
						sopEntity.StatementOfPurposeId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmStatementOfPurpose, StatementOfPurposeDto>(sopEntity);
	}

	/// <summary>
	/// Updates an existing statement of purpose record.
	/// </summary>
	public async Task<StatementOfPurposeDto> UpdateStatementOfPurposeAsync(int statementOfPurposeId, StatementOfPurposeDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(StatementOfPurposeDto));

		if (statementOfPurposeId != modelDto.StatementOfPurposeId)
			throw new BadRequestException(statementOfPurposeId.ToString(), nameof(StatementOfPurposeDto));

		_logger.LogInformation("Updating statement of purpose. ID: {StatementOfPurposeId}, Time: {Time}", statementOfPurposeId, DateTime.UtcNow);

		var sopEntity = await _repository.CrmStatementOfPurposes
						.FirstOrDefaultAsync(x => x.StatementOfPurposeId == statementOfPurposeId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("CrmStatementOfPurpose", "StatementOfPurposeId", statementOfPurposeId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmStatementOfPurpose, StatementOfPurposeDto>(sopEntity, modelDto);
		updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmStatementOfPurposes.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("CrmStatementOfPurpose", "StatementOfPurposeId", statementOfPurposeId.ToString());

		_logger.LogInformation("Statement of purpose updated successfully. ID: {StatementOfPurposeId}, Time: {Time}",
						statementOfPurposeId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmStatementOfPurpose, StatementOfPurposeDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes a statement of purpose record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteStatementOfPurposeAsync(int statementOfPurposeId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (statementOfPurposeId <= 0)
			throw new BadRequestException(statementOfPurposeId.ToString(), nameof(StatementOfPurposeDto));

		_logger.LogInformation("Deleting statement of purpose. ID: {StatementOfPurposeId}, Time: {Time}", statementOfPurposeId, DateTime.UtcNow);

		var sopEntity = await _repository.CrmStatementOfPurposes
						.FirstOrDefaultAsync(x => x.StatementOfPurposeId == statementOfPurposeId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmStatementOfPurpose", "StatementOfPurposeId", statementOfPurposeId.ToString());

		await _repository.CrmStatementOfPurposes.DeleteAsync(x => x.StatementOfPurposeId == statementOfPurposeId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("CrmStatementOfPurpose", "StatementOfPurposeId", statementOfPurposeId.ToString());

		_logger.LogInformation("Statement of purpose deleted successfully. ID: {StatementOfPurposeId}, Time: {Time}",
						statementOfPurposeId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves a single statement of purpose record by its ID.
	/// </summary>
	public async Task<StatementOfPurposeDto> StatementOfPurposeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching statement of purpose. ID: {StatementOfPurposeId}, Time: {Time}", id, DateTime.UtcNow);

		var sop = await _repository.CrmStatementOfPurposes
						.FirstOrDefaultAsync(x => x.StatementOfPurposeId == id, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmStatementOfPurpose", "StatementOfPurposeId", id.ToString());

		_logger.LogInformation("Statement of purpose fetched successfully. ID: {StatementOfPurposeId}, Time: {Time}",
						id, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmStatementOfPurpose, StatementOfPurposeDto>(sop);
	}

	/// <summary>
	/// Retrieves all statement of purpose records from the database.
	/// </summary>
	public async Task<IEnumerable<StatementOfPurposeDto>> StatementOfPurposesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all statement of purposes. Time: {Time}", DateTime.UtcNow);

		var sops = await _repository.CrmStatementOfPurposes
						.CrmStatementOfPurposesAsync(trackChanges, cancellationToken);

		if (!sops.Any())
		{
			_logger.LogWarning("No statement of purposes found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<StatementOfPurposeDto>();
		}

		var sopsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmStatementOfPurpose, StatementOfPurposeDto>(sops);

		_logger.LogInformation("Statement of purposes fetched successfully. Count: {Count}, Time: {Time}",
						sopsDto.Count(), DateTime.UtcNow);

		return sopsDto;
	}

	/// <summary>
	/// Retrieves active statement of purpose records from the database.
	/// </summary>
	public async Task<IEnumerable<StatementOfPurposeDto>> ActiveStatementOfPurposesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching active statement of purposes. Time: {Time}", DateTime.UtcNow);

		var sops = await _repository.CrmStatementOfPurposes
						.CrmStatementOfPurposesAsync(trackChanges, cancellationToken);

		if (!sops.Any())
		{
			_logger.LogWarning("No active statement of purposes found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<StatementOfPurposeDto>();
		}

		var sopsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmStatementOfPurpose, StatementOfPurposeDto>(sops);

		_logger.LogInformation("Active statement of purposes fetched successfully. Count: {Count}, Time: {Time}",
						sopsDto.Count(), DateTime.UtcNow);

		return sopsDto;
	}

	/// <summary>
	/// Retrieves statement of purpose by the specified applicant ID.
	/// </summary>
	public async Task<StatementOfPurposeDto> StatementOfPurposeByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (applicantId <= 0)
		{
			_logger.LogWarning("StatementOfPurposeByApplicantIdAsync called with invalid applicantId: {ApplicantId}", applicantId);
			throw new BadRequestException("Invalid request!");
		}

		_logger.LogInformation("Fetching statement of purpose for applicant ID: {ApplicantId}, Time: {Time}", applicantId, DateTime.UtcNow);

		var sop = await _repository.CrmStatementOfPurposes
						.FirstOrDefaultAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken)
						?? throw new NotFoundException("CrmStatementOfPurpose", "ApplicantId", applicantId.ToString());

		_logger.LogInformation("Statement of purpose fetched successfully. ID: {StatementOfPurposeId}, Time: {Time}",
						sop.StatementOfPurposeId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmStatementOfPurpose, StatementOfPurposeDto>(sop);
	}

	/// <summary>
	/// Retrieves a lightweight list of all statement of purposes suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<StatementOfPurposeDto>> StatementOfPurposeForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching statement of purposes for dropdown list. Time: {Time}", DateTime.UtcNow);

		var sops = await _repository.CrmStatementOfPurposes
						.CrmStatementOfPurposesAsync(false, cancellationToken);

		if (!sops.Any())
		{
			_logger.LogWarning("No statement of purposes found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<StatementOfPurposeDto>();
		}

		var sopsDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmStatementOfPurpose, StatementOfPurposeDto>(sops);

		_logger.LogInformation("Statement of purposes fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						sopsDto.Count(), DateTime.UtcNow);

		return sopsDto;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all statement of purposes.
	/// </summary>
	public async Task<GridEntity<StatementOfPurposeDto>> StatementOfPurposesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql =
						@"SELECT 
                    sop.StatementOfPurposeId,
                    sop.ApplicantId,
                    sop.StatementOfPurposeRemarks,
                    sop.StatementOfPurposeFilePath,
                    sop.CreatedDate,
                    sop.CreatedBy,
                    sop.UpdatedDate,
                    sop.UpdatedBy,
                    app.ApplicationStatus
                FROM CrmStatementOfPurpose sop
                LEFT JOIN CrmApplication app ON sop.ApplicantId = app.ApplicationId";

		const string orderBy = "sop.CreatedDate DESC";

		_logger.LogInformation("Fetching statement of purposes summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmStatementOfPurposes.AdoGridDataAsync<StatementOfPurposeDto>(sql, options, orderBy, "", cancellationToken);
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
///// CrmStatementOfPurpose service implementing business logic for CrmStatementOfPurpose management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmStatementOfPurposeService : ICrmStatementOfPurposeService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmStatementOfPurposeService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmStatementOfPurposeService(IRepositoryManager repository, ILogger<CrmStatementOfPurposeService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmStatementOfPurpose records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmStatementOfPurposeDto>> CrmStatementOfPurposeSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmStatementOfPurpose summary grid");

//        string query = "SELECT * FROM CrmStatementOfPurpose";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmStatementOfPurposes.AdoGridDataAsync<CrmStatementOfPurposeDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmStatementOfPurpose records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmStatementOfPurposeDto>> CrmStatementOfPurposesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmStatementOfPurpose records");

//        var records = await _repository.CrmStatementOfPurposes.CrmStatementOfPurposesAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmStatementOfPurpose records found");
//            return Enumerable.Empty<CrmStatementOfPurposeDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmStatementOfPurpose, CrmStatementOfPurposeDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmStatementOfPurpose record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmStatementOfPurposeDto> CrmStatementOfPurposeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmStatementOfPurposeAsync called with invalid id: {CrmStatementOfPurposeId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmStatementOfPurpose record with ID: {CrmStatementOfPurposeId}", id);

//        var record = await _repository.CrmStatementOfPurposes.CrmStatementOfPurposeAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmStatementOfPurpose record not found with ID: {CrmStatementOfPurposeId}", id);
//            throw new NotFoundException("CrmStatementOfPurpose", "CrmStatementOfPurposeId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmStatementOfPurpose, CrmStatementOfPurposeDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmStatementOfPurpose record asynchronously.
//    /// </summary>
//    public async Task<CrmStatementOfPurposeDto> CreateAsync(CrmStatementOfPurposeDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmStatementOfPurposeDto));

//        _logger.LogInformation("Creating new CrmStatementOfPurpose record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmStatementOfPurposes.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmStatementOfPurpose", "Name");

//        // Map and create
//        CrmStatementOfPurpose entity = MyMapper.JsonClone<CrmStatementOfPurposeDto, CrmStatementOfPurpose>(modelDto);
//        modelDto.CrmStatementOfPurposeId = await _repository.CrmStatementOfPurposes.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmStatementOfPurpose record created successfully with ID: {CrmStatementOfPurposeId}", modelDto.CrmStatementOfPurposeId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmStatementOfPurpose record asynchronously.
//    /// </summary>
//    public async Task<CrmStatementOfPurposeDto> UpdateAsync(int key, CrmStatementOfPurposeDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmStatementOfPurposeDto));

//        if (key != modelDto.CrmStatementOfPurposeId)
//            throw new BadRequestException(key.ToString(), nameof(CrmStatementOfPurposeDto));

//        _logger.LogInformation("Updating CrmStatementOfPurpose record with ID: {CrmStatementOfPurposeId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmStatementOfPurposes.ByIdAsync(
//            x => x.CrmStatementOfPurposeId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmStatementOfPurpose", "CrmStatementOfPurposeId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmStatementOfPurposes.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmStatementOfPurposeId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmStatementOfPurpose", "Name");

//        // Map and update
//        CrmStatementOfPurpose entity = MyMapper.JsonClone<CrmStatementOfPurposeDto, CrmStatementOfPurpose>(modelDto);
//        _repository.CrmStatementOfPurposes.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmStatementOfPurpose record updated successfully: {CrmStatementOfPurposeId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmStatementOfPurpose record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmStatementOfPurpose record with ID: {CrmStatementOfPurposeId}", key);

//        var record = await _repository.CrmStatementOfPurposes.ByIdAsync(
//            x => x.CrmStatementOfPurposeId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmStatementOfPurpose", "CrmStatementOfPurposeId", key.ToString());

//        await _repository.CrmStatementOfPurposes.DeleteAsync(x => x.CrmStatementOfPurposeId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmStatementOfPurpose record deleted successfully: {CrmStatementOfPurposeId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmStatementOfPurpose records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmStatementOfPurposeForDDLDto>> CrmStatementOfPurposesForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmStatementOfPurpose records for dropdown list");

//        var records = await _repository.CrmStatementOfPurposes.ListWithSelectAsync(
//            x => new CrmStatementOfPurpose
//            {
//                CrmStatementOfPurposeId = x.CrmStatementOfPurposeId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmStatementOfPurposeForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmStatementOfPurpose, CrmStatementOfPurposeForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
