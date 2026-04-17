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
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

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
	/// Creates a new institute type record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmInstituteTypeDto> CreateAsync(CreateCrmInstituteTypeRecord record, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(CreateCrmInstituteTypeRecord));

		_logger.LogInformation("Creating new institute type. Name: {InstituteTypeName}, Time: {Time}",
						record.InstituteTypeName, DateTime.UtcNow);

		bool instituteTypeExists = await _repository.CrmInstituteTypes.ExistsAsync(
						x => x.InstituteTypeName != null
								&& x.InstituteTypeName.Trim().ToLower() == record.InstituteTypeName.Trim().ToLower(),
						cancellationToken: cancellationToken);

		if (instituteTypeExists)
			throw new DuplicateRecordException("InstituteType", "InstituteTypeName");

		// Map Record to Entity using Mapster
		CrmInstituteType instituteType = record.MapTo<CrmInstituteType>();
		int instituteTypeId = await _repository.CrmInstituteTypes.CreateAndIdAsync(instituteType, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Institute type created successfully. ID: {InstituteTypeId}, Time: {Time}",
						instituteTypeId, DateTime.UtcNow);

		// Return as DTO
		var resultDto = instituteType.MapTo<CrmInstituteTypeDto>();
		resultDto.InstituteTypeId = instituteTypeId;
		return resultDto;
	}

	/// <summary>
	/// Updates an existing institute type record using CRUD Record pattern.
	/// </summary>
	public async Task<CrmInstituteTypeDto> UpdateAsync(UpdateCrmInstituteTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null)
			throw new BadRequestException(nameof(UpdateCrmInstituteTypeRecord));

		_logger.LogInformation("Updating institute type. ID: {InstituteTypeId}, Time: {Time}", record.InstituteTypeId, DateTime.UtcNow);

		// Check if institute type exists
		var existingInstituteType = await _repository.CrmInstituteTypes
						.FirstOrDefaultAsync(x => x.InstituteTypeId == record.InstituteTypeId, trackChanges: false, cancellationToken)
						?? throw new NotFoundException("InstituteType", "InstituteTypeId", record.InstituteTypeId.ToString());

		// Check for duplicate name (excluding current record)
		bool duplicateExists = await _repository.CrmInstituteTypes.ExistsAsync(
						x => x.InstituteTypeName != null
								&& x.InstituteTypeName.Trim().ToLower() == record.InstituteTypeName.Trim().ToLower()
								&& x.InstituteTypeId != record.InstituteTypeId,
						cancellationToken: cancellationToken);

		if (duplicateExists)
			throw new DuplicateRecordException("InstituteType", "InstituteTypeName");

		// Map Record to Entity using Mapster
		CrmInstituteType instituteType = record.MapTo<CrmInstituteType>();
		_repository.CrmInstituteTypes.UpdateByState(instituteType);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Institute type updated successfully. ID: {InstituteTypeId}, Time: {Time}",
						record.InstituteTypeId, DateTime.UtcNow);

		return instituteType.MapTo<CrmInstituteTypeDto>();
	}

	/// <summary>
	/// Deletes an institute type record using CRUD Record pattern.
	/// </summary>
	public async Task DeleteAsync(DeleteCrmInstituteTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
	{
		if (record == null || record.InstituteTypeId <= 0)
			throw new BadRequestException("Invalid delete request!");

		_logger.LogInformation("Deleting institute type. ID: {InstituteTypeId}, Time: {Time}", record.InstituteTypeId, DateTime.UtcNow);

		var instituteTypeEntity = await _repository.CrmInstituteTypes
						.FirstOrDefaultAsync(x => x.InstituteTypeId == record.InstituteTypeId, trackChanges, cancellationToken)
						?? throw new NotFoundException("InstituteType", "InstituteTypeId", record.InstituteTypeId.ToString());

		await _repository.CrmInstituteTypes.DeleteAsync(x => x.InstituteTypeId == record.InstituteTypeId, trackChanges: false, cancellationToken);
		await _repository.SaveAsync(cancellationToken);

		_logger.LogInformation("Institute type deleted successfully. ID: {InstituteTypeId}, Time: {Time}",
						record.InstituteTypeId, DateTime.UtcNow);
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

		var instituteTypesDto = instituteTypes.MapToList<CrmInstituteTypeDto>();

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
			var record = new CreateCrmInstituteTypeRecord(modelDto.InstituteTypeName);
			var created = await CreateAsync(record, cancellationToken);
			return created.InstituteTypeId > 0
					? OperationMessage.Success
					: "Failed to create institute type.";
		}
		else
		{
			var record = new UpdateCrmInstituteTypeRecord(modelDto.InstituteTypeId, modelDto.InstituteTypeName);
			await UpdateAsync(record, false, cancellationToken);
			return OperationMessage.Success;
		}
	}
}
