using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CrmPaymentMethod service implementing business logic for CrmPaymentMethod management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmPaymentMethodService : ICrmPaymentMethodService
{
	private readonly IRepositoryManager _repository;
	private readonly ILogger<CrmPaymentMethodService> _logger;
	private readonly IConfiguration _configuration;

	public CrmPaymentMethodService(IRepositoryManager repository, ILogger<CrmPaymentMethodService> logger, IConfiguration configuration)
	{
		_repository = repository;
		_logger = logger;
		_configuration = configuration;
	}

	///// <summary>
	///// Retrieves paginated summary grid of CrmPaymentMethod records asynchronously.
	///// </summary>
	//public async Task<GridEntity<CrmPaymentMethodDto>> CrmPaymentMethodSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
	//{
	//	_logger.LogInformation("Fetching CrmPaymentMethod summary grid");

	//	string query = "SELECT * FROM CrmPaymentMethod";
	//	string orderBy = "Name ASC";

	//	var gridEntity = await _repository.CrmPaymentMethods.AdoGridDataAsync<CrmPaymentMethodDto>(query, options, orderBy, "", cancellationToken);
	//	return gridEntity;
	//}

	///// <summary>
	///// Retrieves all CrmPaymentMethod records asynchronously.
	///// </summary>
	//public async Task<IEnumerable<CrmPaymentMethodDto>> CrmPaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	//{
	//	_logger.LogInformation("Fetching all CrmPaymentMethod records");

	//	var records = await _repository.CrmPaymentMethods.CrmPaymentMethodsAsync(trackChanges, cancellationToken);

	//	if (!records.Any())
	//	{
	//		_logger.LogWarning("No CrmPaymentMethod records found");
	//		return Enumerable.Empty<CrmPaymentMethodDto>();
	//	}

	//	var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmPaymentMethod, CrmPaymentMethodDto>(records);
	//	return recordDtos;
	//}

	///// <summary>
	///// Retrieves a CrmPaymentMethod record by ID asynchronously.
	///// </summary>
	//public async Task<CrmPaymentMethodDto> CrmPaymentMethodAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
	//{
	//	if (id <= 0)
	//	{
	//		_logger.LogWarning("CrmPaymentMethodAsync called with invalid id: {CrmPaymentMethodId}", id);
	//		throw new BadRequestException("Invalid request!");
	//	}

	//	_logger.LogInformation("Fetching CrmPaymentMethod record with ID: {CrmPaymentMethodId}", id);

	//	var record = await _repository.CrmPaymentMethods.CrmPaymentMethodAsync(id, trackChanges, cancellationToken);

	//	if (record == null)
	//	{
	//		_logger.LogWarning("CrmPaymentMethod record not found with ID: {CrmPaymentMethodId}", id);
	//		throw new NotFoundException("CrmPaymentMethod", "CrmPaymentMethodId", id.ToString());
	//	}

	//	var recordDto = MyMapper.JsonClone<CrmPaymentMethod, CrmPaymentMethodDto>(record);
	//	return recordDto;
	//}

	///// <summary>
	///// Creates a new CrmPaymentMethod record asynchronously.
	///// </summary>
	//public async Task<CrmPaymentMethodDto> CreateAsync(CrmPaymentMethodDto modelDto)
	//{
	//	if (modelDto == null)
	//		throw new BadRequestException(nameof(CrmPaymentMethodDto));

	//	_logger.LogInformation("Creating new CrmPaymentMethod record");

	//	// Check for duplicate record
	//	bool recordExists = await _repository.CrmPaymentMethods.ExistsAsync(
	//			x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

	//	if (recordExists)
	//		throw new DuplicateRecordException("CrmPaymentMethod", "Name");

	//	// Map and create
	//	CrmPaymentMethod entity = MyMapper.JsonClone<CrmPaymentMethodDto, CrmPaymentMethod>(modelDto);
	//	modelDto.CrmPaymentMethodId = await _repository.CrmPaymentMethods.CreateAndIdAsync(entity);
	//	await _repository.SaveAsync();

	//	_logger.LogInformation("CrmPaymentMethod record created successfully with ID: {CrmPaymentMethodId}", modelDto.CrmPaymentMethodId);

	//	return modelDto;
	//}

	///// <summary>
	///// Updates an existing CrmPaymentMethod record asynchronously.
	///// </summary>
	//public async Task<CrmPaymentMethodDto> UpdateAsync(int key, CrmPaymentMethodDto modelDto)
	//{
	//	if (modelDto == null)
	//		throw new BadRequestException(nameof(CrmPaymentMethodDto));

	//	if (key != modelDto.CrmPaymentMethodId)
	//		throw new BadRequestException(key.ToString(), nameof(CrmPaymentMethodDto));

	//	_logger.LogInformation("Updating CrmPaymentMethod record with ID: {CrmPaymentMethodId}", key);

	//	// Check if record exists
	//	var existingRecord = await _repository.CrmPaymentMethods.ByIdAsync(
	//			x => x.CrmPaymentMethodId == key, trackChanges: false);

	//	if (existingRecord == null)
	//		throw new NotFoundException("CrmPaymentMethod", "CrmPaymentMethodId", key.ToString());

	//	// Check for duplicate name (excluding current record)
	//	bool duplicateExists = await _repository.CrmPaymentMethods.ExistsAsync(
	//			x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower()
	//					 && x.CrmPaymentMethodId != key);

	//	if (duplicateExists)
	//		throw new DuplicateRecordException("CrmPaymentMethod", "Name");

	//	// Map and update
	//	CrmPaymentMethod entity = MyMapper.JsonClone<CrmPaymentMethodDto, CrmPaymentMethod>(modelDto);
	//	_repository.CrmPaymentMethods.UpdateByState(entity);
	//	await _repository.SaveAsync();

	//	_logger.LogInformation("CrmPaymentMethod record updated successfully: {CrmPaymentMethodId}", key);

	//	return modelDto;
	//}

	///// <summary>
	///// Deletes a CrmPaymentMethod record by ID asynchronously.
	///// </summary>
	//public async Task DeleteAsync(int key)
	//{
	//	if (key <= 0)
	//		throw new BadRequestException("Invalid request!");

	//	_logger.LogInformation("Deleting CrmPaymentMethod record with ID: {CrmPaymentMethodId}", key);

	//	var record = await _repository.CrmPaymentMethods.ByIdAsync(
	//			x => x.CrmPaymentMethodId == key, trackChanges: false);

	//	if (record == null)
	//		throw new NotFoundException("CrmPaymentMethod", "CrmPaymentMethodId", key.ToString());

	//	await _repository.CrmPaymentMethods.DeleteAsync(x => x.CrmPaymentMethodId == key, trackChanges: false);
	//	await _repository.SaveAsync();

	//	_logger.LogInformation("CrmPaymentMethod record deleted successfully: {CrmPaymentMethodId}", key);
	//}

	///// <summary>
	///// Retrieves CrmPaymentMethod records for dropdown list asynchronously.
	///// </summary>
	//public async Task<IEnumerable<CrmPaymentMethodForDDLDto>> CrmPaymentMethodsForDDLAsync()
	//{
	//	_logger.LogInformation("Fetching CrmPaymentMethod records for dropdown list");

	//	var records = await _repository.CrmPaymentMethods.ListWithSelectAsync(
	//			x => new CrmPaymentMethod
	//			{
	//				CrmPaymentMethodId = x.CrmPaymentMethodId,
	//				Name = x.Name
	//			},
	//			orderBy: x => x.Name,
	//			trackChanges: false
	//	);

	//	if (!records.Any())
	//		return new List<CrmPaymentMethodForDDLDto>();

	//	var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmPaymentMethod, CrmPaymentMethodForDDLDto>(records);
	//	return recordsForDDLDto;
	//}
}
