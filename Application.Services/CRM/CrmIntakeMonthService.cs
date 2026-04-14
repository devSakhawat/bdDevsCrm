using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Contracts.Services.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Application.Services.Mappings;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CrmIntakeMonth service implementing business logic for CrmIntakeMonth management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmIntakeMonthService : ICrmIntakeMonthService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmIntakeMonthService> _logger;
  private readonly IConfiguration _configuration;

  public CrmIntakeMonthService(IRepositoryManager repository, ILogger<CrmIntakeMonthService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  ///// <summary>
  ///// Retrieves paginated summary grid of CrmIntakeMonth records asynchronously.
  ///// </summary>
  //public async Task<GridEntity<CrmIntakeMonthDto>> CrmIntakeMonthSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
  //{
  //  _logger.LogInformation("Fetching CrmIntakeMonth summary grid");

  //  string query = "SELECT * FROM CrmIntakeMonth";
  //  string orderBy = "Name ASC";

  //  var gridEntity = await _repository.CrmIntakeMonths.AdoGridDataAsync<CrmIntakeMonthDto>(query, options, orderBy, "", cancellationToken);
  //  return gridEntity;
  //}

  ///// <summary>
  ///// Retrieves all CrmIntakeMonth records asynchronously.
  ///// </summary>
  //public async Task<IEnumerable<CrmIntakeMonthDto>> CrmIntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  //{
  //  _logger.LogInformation("Fetching all CrmIntakeMonth records");

  //  var records = await _repository.CrmIntakeMonths.CrmIntakeMonthsAsync(trackChanges, cancellationToken);

  //  if (!records.Any())
  //  {
  //    _logger.LogWarning("No CrmIntakeMonth records found");
  //    return Enumerable.Empty<CrmIntakeMonthDto>();
  //  }

  //  var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmIntakeMonth, CrmIntakeMonthDto>(records);
  //  return recordDtos;
  //}

  ///// <summary>
  ///// Retrieves a CrmIntakeMonth record by ID asynchronously.
  ///// </summary>
  //public async Task<CrmIntakeMonthDto> CrmIntakeMonthAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  //{
  //  if (id <= 0)
  //  {
  //    _logger.LogWarning("CrmIntakeMonthAsync called with invalid id: {CrmIntakeMonthId}", id);
  //    throw new BadRequestException("Invalid request!");
  //  }

  //  _logger.LogInformation("Fetching CrmIntakeMonth record with ID: {CrmIntakeMonthId}", id);

  //  var record = await _repository.CrmIntakeMonths.CrmIntakeMonthAsync(id, trackChanges, cancellationToken);

  //  if (record == null)
  //  {
  //    _logger.LogWarning("CrmIntakeMonth record not found with ID: {CrmIntakeMonthId}", id);
  //    throw new NotFoundException("CrmIntakeMonth", "CrmIntakeMonthId", id.ToString());
  //  }

  //  var recordDto = MyMapper.JsonClone<CrmIntakeMonth, CrmIntakeMonthDto>(record);
  //  return recordDto;
  //}

  ///// <summary>
  ///// Creates a new CrmIntakeMonth record asynchronously.
  ///// </summary>
  //public async Task<CrmIntakeMonthDto> CreateAsync(CrmIntakeMonthDto modelDto)
  //{
  //  if (modelDto == null)
  //    throw new BadRequestException(nameof(CrmIntakeMonthDto));

  //  _logger.LogInformation("Creating new CrmIntakeMonth record");

  //  // Check for duplicate record
  //  bool recordExists = await _repository.CrmIntakeMonths.ExistsAsync(
  //      x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

  //  if (recordExists)
  //    throw new DuplicateRecordException("CrmIntakeMonth", "Name");

  //  // Map and create
  //  CrmIntakeMonth entity = MyMapper.JsonClone<CrmIntakeMonthDto, CrmIntakeMonth>(modelDto);
  //  modelDto.CrmIntakeMonthId = await _repository.CrmIntakeMonths.CreateAndIdAsync(entity);
  //  await _repository.SaveAsync();

  //  _logger.LogInformation("CrmIntakeMonth record created successfully with ID: {CrmIntakeMonthId}", modelDto.CrmIntakeMonthId);

  //  return modelDto;
  //}

  ///// <summary>
  ///// Updates an existing CrmIntakeMonth record asynchronously.
  ///// </summary>
  //public async Task<CrmIntakeMonthDto> UpdateAsync(int key, CrmIntakeMonthDto modelDto)
  //{
  //  if (modelDto == null)
  //    throw new BadRequestException(nameof(CrmIntakeMonthDto));

  //  if (key != modelDto.CrmIntakeMonthId)
  //    throw new BadRequestException(key.ToString(), nameof(CrmIntakeMonthDto));

  //  _logger.LogInformation("Updating CrmIntakeMonth record with ID: {CrmIntakeMonthId}", key);

  //  // Check if record exists
  //  var existingRecord = await _repository.CrmIntakeMonths.ByIdAsync(
  //      x => x.CrmIntakeMonthId == key, trackChanges: false);

  //  if (existingRecord == null)
  //    throw new NotFoundException("CrmIntakeMonth", "CrmIntakeMonthId", key.ToString());

  //  // Check for duplicate name (excluding current record)
  //  bool duplicateExists = await _repository.CrmIntakeMonths.ExistsAsync(
  //      x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower()
  //           && x.CrmIntakeMonthId != key);

  //  if (duplicateExists)
  //    throw new DuplicateRecordException("CrmIntakeMonth", "Name");

  //  // Map and update
  //  CrmIntakeMonth entity = MyMapper.JsonClone<CrmIntakeMonthDto, CrmIntakeMonth>(modelDto);
  //  _repository.CrmIntakeMonths.UpdateByState(entity);
  //  await _repository.SaveAsync();

  //  _logger.LogInformation("CrmIntakeMonth record updated successfully: {CrmIntakeMonthId}", key);

  //  return modelDto;
  //}

  ///// <summary>
  ///// Deletes a CrmIntakeMonth record by ID asynchronously.
  ///// </summary>
  //public async Task DeleteAsync(int key)
  //{
  //  if (key <= 0)
  //    throw new BadRequestException("Invalid request!");

  //  _logger.LogInformation("Deleting CrmIntakeMonth record with ID: {CrmIntakeMonthId}", key);

  //  var record = await _repository.CrmIntakeMonths.ByIdAsync(
  //      x => x.CrmIntakeMonthId == key, trackChanges: false);

  //  if (record == null)
  //    throw new NotFoundException("CrmIntakeMonth", "CrmIntakeMonthId", key.ToString());

  //  await _repository.CrmIntakeMonths.DeleteAsync(x => x.CrmIntakeMonthId == key, trackChanges: false);
  //  await _repository.SaveAsync();

  //  _logger.LogInformation("CrmIntakeMonth record deleted successfully: {CrmIntakeMonthId}", key);
  //}

  ///// <summary>
  ///// Retrieves CrmIntakeMonth records for dropdown list asynchronously.
  ///// </summary>
  //public async Task<IEnumerable<CrmIntakeMonthForDDLDto>> CrmIntakeMonthsForDDLAsync()
  //{
  //  _logger.LogInformation("Fetching CrmIntakeMonth records for dropdown list");

  //  var records = await _repository.CrmIntakeMonths.ListWithSelectAsync(
  //      x => new CrmIntakeMonth
  //      {
  //        CrmIntakeMonthId = x.CrmIntakeMonthId,
  //        Name = x.Name
  //      },
  //      orderBy: x => x.Name,
  //      trackChanges: false
  //  );

  //  if (!records.Any())
  //    return new List<CrmIntakeMonthForDDLDto>();

  //  var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmIntakeMonth, CrmIntakeMonthForDDLDto>(records);
  //  return recordsForDDLDto;
  //}
}
