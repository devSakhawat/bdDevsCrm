using bdDevs.Shared.Constants;
// CrmInstituteTypeService.cs
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using Domain.Exceptions;
using Domain.Contracts.Repositories;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CRM Institute Type service implementing business logic for institute type management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmInstituteTypeService : ICrmInstituteTypeService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmInstituteTypeService> _logger;
	private readonly IConfiguration _configuration;

	/// <summary>
	/// Initializes a new instance of <see cref="CrmInstituteTypeService"/> with required dependencies.
	/// </summary>
	public CrmInstituteTypeService(IRepositoryManager repository, ILogger<CrmInstituteTypeService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	/// <summary>
	/// Creates a new institute type record.
	/// </summary>
	public async Task<CrmInstituteTypeDto> CreateInstituteTypeAsync(CrmInstituteTypeDto entityForCreate, UsersDto currentUser, CancellationToken cancellationToken = default)
	{
		if (entityForCreate is null)
			throw new BadRequestException(nameof(CrmInstituteTypeDto));

		if (entityForCreate.InstituteTypeId != 0)
			throw new InvalidCreateOperationException("InstituteTypeId must be 0 for new record.");

		bool instituteTypeExists = await _repository.CrmInstituteTypes.ExistsAsync(
						x => x.InstituteTypeName != null
								&& x.InstituteTypeName.Trim().ToLower() == entityForCreate.InstituteTypeName.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (instituteTypeExists)
			throw new DuplicateRecordException("InstituteType", "InstituteTypeName");

		_logger.LogInformation("Creating new institute type. Name: {InstituteTypeName}, Time: {Time}",
						entityForCreate.InstituteTypeName, DateTime.UtcNow);

		var instituteTypeEntity = MyMapper.JsonClone<CrmInstituteTypeDto, CrmInstituteType>(entityForCreate);
		//instituteTypeEntity.CreatedDate = DateTime.UtcNow;
		//instituteTypeEntity.CreatedBy = currentUser.UserId ?? 0;

		await _repository.CrmInstituteTypes.CreateAsync(instituteTypeEntity, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new InvalidOperationException("Institute type could not be saved to the database.");

		_logger.LogInformation("Institute type created successfully. ID: {InstituteTypeId}, Time: {Time}",
						instituteTypeEntity.InstituteTypeId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmInstituteType, CrmInstituteTypeDto>(instituteTypeEntity);
	}

	/// <summary>
	/// Updates an existing institute type record.
	/// </summary>
	public async Task<CrmInstituteTypeDto> UpdateInstituteTypeAsync(int instituteTypeId, CrmInstituteTypeDto modelDto, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmInstituteTypeDto));

		if (instituteTypeId != modelDto.InstituteTypeId)
			throw new BadRequestException(instituteTypeId.ToString(), nameof(CrmInstituteTypeDto));

		_logger.LogInformation("Updating institute type. ID: {InstituteTypeId}, Time: {Time}", instituteTypeId, DateTime.UtcNow);

		var instituteTypeEntity = await _repository.CrmInstituteTypes
						.FirstOrDefaultAsync(x => x.InstituteTypeId == instituteTypeId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("InstituteType", "InstituteTypeId", instituteTypeId.ToString());

		var updatedEntity = MyMapper.MergeChangedValues<CrmInstituteType, CrmInstituteTypeDto>(instituteTypeEntity, modelDto);
		//updatedEntity.UpdatedDate = DateTime.UtcNow;

		_repository.CrmInstituteTypes.UpdateByState(updatedEntity);

		int affected = await _repository.SaveChangesAsync(cancellationToken);
		if (affected <= 0)
			throw new NotFoundException("InstituteType", "InstituteTypeId", instituteTypeId.ToString());

		_logger.LogInformation("Institute type updated successfully. ID: {InstituteTypeId}, Time: {Time}",
						instituteTypeId, DateTime.UtcNow);

		return MyMapper.JsonClone<CrmInstituteType, CrmInstituteTypeDto>(updatedEntity);
	}

	/// <summary>
	/// Deletes an institute type record identified by the given ID.
	/// </summary>
	public async Task<int> DeleteInstituteTypeAsync(int instituteTypeId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (instituteTypeId <= 0)
			throw new BadRequestException(instituteTypeId.ToString(), nameof(CrmInstituteTypeDto));

		_logger.LogInformation("Deleting institute type. ID: {InstituteTypeId}, Time: {Time}", instituteTypeId, DateTime.UtcNow);

		var instituteTypeEntity = await _repository.CrmInstituteTypes
						.FirstOrDefaultAsync(x => x.InstituteTypeId == instituteTypeId, trackChanges, cancellationToken)
						?? throw new NotFoundException("InstituteType", "InstituteTypeId", instituteTypeId.ToString());

		await _repository.CrmInstituteTypes.DeleteAsync(x => x.InstituteTypeId == instituteTypeId, trackChanges, cancellationToken);
		int affected = await _repository.SaveChangesAsync(cancellationToken);

		if (affected <= 0)
			throw new NotFoundException("InstituteType", "InstituteTypeId", instituteTypeId.ToString());

		_logger.LogInformation("Institute type deleted successfully. ID: {InstituteTypeId}, Time: {Time}",
						instituteTypeId, DateTime.UtcNow);

		return affected;
	}

	/// <summary>
	/// Retrieves all institute type records from the database.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteTypeDto>> InstituteTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching all institute types. Time: {Time}", DateTime.UtcNow);

		var instituteTypes = await _repository.CrmInstituteTypes.CrmInstituteTypesAsync(trackChanges, cancellationToken);

		if (!instituteTypes.Any())
		{
			_logger.LogWarning("No institute types found. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmInstituteTypeDto>();
		}

		var instituteTypesDto = MyMapper.JsonCloneIEnumerableToIEnumerable<CrmInstituteType, CrmInstituteTypeDto>(instituteTypes);

		_logger.LogInformation("Institute types fetched successfully. Count: {Count}, Time: {Time}",
						instituteTypesDto.Count(), DateTime.UtcNow);

		return instituteTypesDto;
	}

	/// <summary>
	/// Retrieves a lightweight list of all institute types suitable for use in dropdown lists.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteTypeDto>> InstituteTypeForDDLAsync(CancellationToken cancellationToken = default)
	{
		_logger.LogInformation("Fetching institute types for dropdown list. Time: {Time}", DateTime.UtcNow);

		var instituteTypes = await _repository.CrmInstituteTypes.ListWithSelectAsync(selector: x => new CrmInstituteTypeDto
		{
			InstituteTypeId = x.InstituteTypeId,
			InstituteTypeName = x.InstituteTypeName
		}
		,orderBy: x => x.InstituteTypeName
		,trackChanges: false, cancellationToken);

		if (!instituteTypes.Any())
		{
			_logger.LogWarning("No institute types found for dropdown list. Time: {Time}", DateTime.UtcNow);
			return Enumerable.Empty<CrmInstituteTypeDto>();
		}

		_logger.LogInformation("Institute types fetched successfully for dropdown list. Count: {Count}, Time: {Time}",
						instituteTypes.Count(), DateTime.UtcNow);

		return instituteTypes;
	}

	/// <summary>
	/// Retrieves a paginated summary grid of all institute types.
	/// </summary>
	public async Task<GridEntity<CrmInstituteTypeDto>> InstituteTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
	{
		const string sql = "SELECT * FROM CrmInstituteType";
		const string orderBy = "InstituteTypeName ASC";

		_logger.LogInformation("Fetching institute types summary grid. Time: {Time}", DateTime.UtcNow);

		return await _repository.CrmInstituteTypes.AdoGridDataAsync<CrmInstituteTypeDto>(sql, options, orderBy, "", cancellationToken);
	}

	/// <summary>
	/// Saves an institute type record (create or update).
	/// </summary>
	public async Task<string> SaveOrUpdateInstituteTypeAsync(int instituteTypeId, CrmInstituteTypeDto modelDto, CancellationToken cancellationToken = default)
	{
		if (modelDto is null)
			throw new BadRequestException(nameof(CrmInstituteTypeDto));

		_logger.LogInformation("Saving institute type. InstituteTypeId: {InstituteTypeId}, Time: {Time}",
						instituteTypeId, DateTime.UtcNow);

		if (instituteTypeId == 0)
		{
			var created = await CreateInstituteTypeAsync(modelDto, new UsersDto(), cancellationToken);
			return created.InstituteTypeId > 0
					? OperationMessage.Success
					: "Failed to create institute type.";
		}
		else
		{
			await UpdateInstituteTypeAsync(instituteTypeId, modelDto, false, cancellationToken);
			return OperationMessage.Success;
		}
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
///// CrmInstituteType service implementing business logic for CrmInstituteType management.
///// Follows enterprise patterns with structured logging and exception handling.
///// </summary>
//internal sealed class CrmInstituteTypeService : ICrmInstituteTypeService
//{
//    private readonly IRepositoryManager _repository;
//    private readonly ILogger<CrmInstituteTypeService> _logger;
//    private readonly IConfiguration _configuration;

//    public CrmInstituteTypeService(IRepositoryManager repository, ILogger<CrmInstituteTypeService> logger, IConfiguration configuration)
//    {
//        _repository = repository;
//        _logger = logger;
//        _configuration = configuration;
//    }

//    /// <summary>
//    /// Retrieves paginated summary grid of CrmInstituteType records asynchronously.
//    /// </summary>
//    public async Task<GridEntity<CrmInstituteTypeDto>> CrmInstituteTypeSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching CrmInstituteType summary grid");

//        string query = "SELECT * FROM CrmInstituteType";
//        string orderBy = "Name ASC";

//        var gridEntity = await _repository.CrmInstituteTypes.AdoGridDataAsync<CrmInstituteTypeDto>(query, options, orderBy, "", cancellationToken);
//        return gridEntity;
//    }

//    /// <summary>
//    /// Retrieves all CrmInstituteType records asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmInstituteTypeDto>> CrmInstituteTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        _logger.LogInformation("Fetching all CrmInstituteType records");

//        var records = await _repository.CrmInstituteTypes.CrmInstituteTypesAsync(trackChanges, cancellationToken);

//        if (!records.Any())
//        {
//            _logger.LogWarning("No CrmInstituteType records found");
//            return Enumerable.Empty<CrmInstituteTypeDto>();
//        }

//        var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmInstituteType, CrmInstituteTypeDto>(records);
//        return recordDtos;
//    }

//    /// <summary>
//    /// Retrieves a CrmInstituteType record by ID asynchronously.
//    /// </summary>
//    public async Task<CrmInstituteTypeDto> CrmInstituteTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
//    {
//        if (id <= 0)
//        {
//            _logger.LogWarning("CrmInstituteTypeAsync called with invalid id: {CrmInstituteTypeId}", id);
//            throw new BadRequestException("Invalid request!");
//        }

//        _logger.LogInformation("Fetching CrmInstituteType record with ID: {CrmInstituteTypeId}", id);

//        var record = await _repository.CrmInstituteTypes.CrmInstituteTypeAsync(id, trackChanges, cancellationToken);

//        if (record == null)
//        {
//            _logger.LogWarning("CrmInstituteType record not found with ID: {CrmInstituteTypeId}", id);
//            throw new NotFoundException("CrmInstituteType", "CrmInstituteTypeId", id.ToString());
//        }

//        var recordDto = MyMapper.JsonClone<CrmInstituteType, CrmInstituteTypeDto>(record);
//        return recordDto;
//    }

//    /// <summary>
//    /// Creates a new CrmInstituteType record asynchronously.
//    /// </summary>
//    public async Task<CrmInstituteTypeDto> CreateAsync(CrmInstituteTypeDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmInstituteTypeDto));

//        _logger.LogInformation("Creating new CrmInstituteType record");

//        // Check for duplicate record
//        bool recordExists = await _repository.CrmInstituteTypes.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

//        if (recordExists)
//            throw new DuplicateRecordException("CrmInstituteType", "Name");

//        // Map and create
//        CrmInstituteType entity = MyMapper.JsonClone<CrmInstituteTypeDto, CrmInstituteType>(modelDto);
//        modelDto.CrmInstituteTypeId = await _repository.CrmInstituteTypes.CreateAndIdAsync(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmInstituteType record created successfully with ID: {CrmInstituteTypeId}", modelDto.CrmInstituteTypeId);

//        return modelDto;
//    }

//    /// <summary>
//    /// Updates an existing CrmInstituteType record asynchronously.
//    /// </summary>
//    public async Task<CrmInstituteTypeDto> UpdateAsync(int key, CrmInstituteTypeDto modelDto)
//    {
//        if (modelDto == null)
//            throw new BadRequestException(nameof(CrmInstituteTypeDto));

//        if (key != modelDto.CrmInstituteTypeId)
//            throw new BadRequestException(key.ToString(), nameof(CrmInstituteTypeDto));

//        _logger.LogInformation("Updating CrmInstituteType record with ID: {CrmInstituteTypeId}", key);

//        // Check if record exists
//        var existingRecord = await _repository.CrmInstituteTypes.ByIdAsync(
//            x => x.CrmInstituteTypeId == key, trackChanges: false);

//        if (existingRecord == null)
//            throw new NotFoundException("CrmInstituteType", "CrmInstituteTypeId", key.ToString());

//        // Check for duplicate name (excluding current record)
//        bool duplicateExists = await _repository.CrmInstituteTypes.ExistsAsync(
//            x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower() 
//                 && x.CrmInstituteTypeId != key);

//        if (duplicateExists)
//            throw new DuplicateRecordException("CrmInstituteType", "Name");

//        // Map and update
//        CrmInstituteType entity = MyMapper.JsonClone<CrmInstituteTypeDto, CrmInstituteType>(modelDto);
//        _repository.CrmInstituteTypes.UpdateByState(entity);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmInstituteType record updated successfully: {CrmInstituteTypeId}", key);

//        return modelDto;
//    }

//    /// <summary>
//    /// Deletes a CrmInstituteType record by ID asynchronously.
//    /// </summary>
//    public async Task DeleteAsync(int key)
//    {
//        if (key <= 0)
//            throw new BadRequestException("Invalid request!");

//        _logger.LogInformation("Deleting CrmInstituteType record with ID: {CrmInstituteTypeId}", key);

//        var record = await _repository.CrmInstituteTypes.ByIdAsync(
//            x => x.CrmInstituteTypeId == key, trackChanges: false);

//        if (record == null)
//            throw new NotFoundException("CrmInstituteType", "CrmInstituteTypeId", key.ToString());

//        await _repository.CrmInstituteTypes.DeleteAsync(x => x.CrmInstituteTypeId == key, trackChanges: false);
//        await _repository.SaveAsync();

//        _logger.LogInformation("CrmInstituteType record deleted successfully: {CrmInstituteTypeId}", key);
//    }

//    /// <summary>
//    /// Retrieves CrmInstituteType records for dropdown list asynchronously.
//    /// </summary>
//    public async Task<IEnumerable<CrmInstituteTypeForDDLDto>> CrmInstituteTypesForDDLAsync()
//    {
//        _logger.LogInformation("Fetching CrmInstituteType records for dropdown list");

//        var records = await _repository.CrmInstituteTypes.ListWithSelectAsync(
//            x => new CrmInstituteType
//            {
//                CrmInstituteTypeId = x.CrmInstituteTypeId,
//                Name = x.Name
//            },
//            orderBy: x => x.Name,
//            trackChanges: false
//        );

//        if (!records.Any())
//            return new List<CrmInstituteTypeForDDLDto>();

//        var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmInstituteType, CrmInstituteTypeForDDLDto>(records);
//        return recordsForDDLDto;
//    }
//}
