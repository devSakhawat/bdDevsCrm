using bdDevCRM.Entities.Entities.CRM;
using Domain.Contracts.Services.Core.SystemAdmin;
using bdDevCRM.s.CRM;
using bdDevCRM.ServiceContract.CRM;
using bdDevCRM.ServicesContract.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevCRM.Shared.Exceptions;
using Application.Shared.Grid;
using bdDevCRM.Utilities.OthersLibrary;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

/// <summary>
/// CrmIntakeYear service implementing business logic for CrmIntakeYear management.
/// Follows enterprise patterns with structured logging and exception handling.
/// </summary>
internal sealed class CrmIntakeYearService : ICrmIntakeYearService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmIntakeYearService> _logger;
  private readonly IConfiguration _configuration;

  public CrmIntakeYearService(IRepositoryManager repository, ILogger<CrmIntakeYearService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  ///// <summary>
  ///// Retrieves paginated summary grid of CrmIntakeYear records asynchronously.
  ///// </summary>
  //public async Task<GridEntity<CrmIntakeYearDto>> CrmIntakeYearSummaryAsync(bool trackChanges, GridOptions options, CancellationToken cancellationToken = default)
  //{
  //  _logger.LogInformation("Fetching CrmIntakeYear summary grid");

  //  string query = "SELECT * FROM CrmIntakeYear";
  //  string orderBy = "Name ASC";

  //  var gridEntity = await _repository.CrmIntakeYears.AdoGridDataAsync<CrmIntakeYearDto>(query, options, orderBy, "", cancellationToken);
  //  return gridEntity;
  //}

  ///// <summary>
  ///// Retrieves all CrmIntakeYear records asynchronously.
  ///// </summary>
  //public async Task<IEnumerable<CrmIntakeYearDto>> CrmIntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
  //{
  //  _logger.LogInformation("Fetching all CrmIntakeYear records");

  //  var records = await _repository.CrmIntakeYears.CrmIntakeYearsAsync(trackChanges, cancellationToken);

  //  if (!records.Any())
  //  {
  //    _logger.LogWarning("No CrmIntakeYear records found");
  //    return Enumerable.Empty<CrmIntakeYearDto>();
  //  }

  //  var recordDtos = MyMapper.JsonCloneIEnumerableToList<CrmIntakeYear, CrmIntakeYearDto>(records);
  //  return recordDtos;
  //}

  ///// <summary>
  ///// Retrieves a CrmIntakeYear record by ID asynchronously.
  ///// </summary>
  //public async Task<CrmIntakeYearDto> CrmIntakeYearAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  //{
  //  if (id <= 0)
  //  {
  //    _logger.LogWarning("CrmIntakeYearAsync called with invalid id: {CrmIntakeYearId}", id);
  //    throw new BadRequestException("Invalid request!");
  //  }

  //  _logger.LogInformation("Fetching CrmIntakeYear record with ID: {CrmIntakeYearId}", id);

  //  var record = await _repository.CrmIntakeYears.CrmIntakeYearAsync(id, trackChanges, cancellationToken);

  //  if (record == null)
  //  {
  //    _logger.LogWarning("CrmIntakeYear record not found with ID: {CrmIntakeYearId}", id);
  //    throw new NotFoundException("CrmIntakeYear", "CrmIntakeYearId", id.ToString());
  //  }

  //  var recordDto = MyMapper.JsonClone<CrmIntakeYear, CrmIntakeYearDto>(record);
  //  return recordDto;
  //}

  ///// <summary>
  ///// Creates a new CrmIntakeYear record asynchronously.
  ///// </summary>
  //public async Task<CrmIntakeYearDto> CreateAsync(CrmIntakeYearDto modelDto)
  //{
  //  if (modelDto == null)
  //    throw new BadRequestException(nameof(CrmIntakeYearDto));

  //  _logger.LogInformation("Creating new CrmIntakeYear record");

  //  // Check for duplicate record
  //  bool recordExists = await _repository.CrmIntakeYears.ExistsAsync(
  //      x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower());

  //  if (recordExists)
  //    throw new DuplicateRecordException("CrmIntakeYear", "Name");

  //  // Map and create
  //  CrmIntakeYear entity = MyMapper.JsonClone<CrmIntakeYearDto, CrmIntakeYear>(modelDto);
  //  modelDto.CrmIntakeYearId = await _repository.CrmIntakeYears.CreateAndIdAsync(entity);
  //  await _repository.SaveAsync();

  //  _logger.LogInformation("CrmIntakeYear record created successfully with ID: {CrmIntakeYearId}", modelDto.CrmIntakeYearId);

  //  return modelDto;
  //}

  ///// <summary>
  ///// Updates an existing CrmIntakeYear record asynchronously.
  ///// </summary>
  //public async Task<CrmIntakeYearDto> UpdateAsync(int key, CrmIntakeYearDto modelDto)
  //{
  //  if (modelDto == null)
  //    throw new BadRequestException(nameof(CrmIntakeYearDto));

  //  if (key != modelDto.CrmIntakeYearId)
  //    throw new BadRequestException(key.ToString(), nameof(CrmIntakeYearDto));

  //  _logger.LogInformation("Updating CrmIntakeYear record with ID: {CrmIntakeYearId}", key);

  //  // Check if record exists
  //  var existingRecord = await _repository.CrmIntakeYears.ByIdAsync(
  //      x => x.CrmIntakeYearId == key, trackChanges: false);

  //  if (existingRecord == null)
  //    throw new NotFoundException("CrmIntakeYear", "CrmIntakeYearId", key.ToString());

  //  // Check for duplicate name (excluding current record)
  //  bool duplicateExists = await _repository.CrmIntakeYears.ExistsAsync(
  //      x => x.Name.Trim().ToLower() == modelDto.Name.Trim().ToLower()
  //           && x.CrmIntakeYearId != key);

  //  if (duplicateExists)
  //    throw new DuplicateRecordException("CrmIntakeYear", "Name");

  //  // Map and update
  //  CrmIntakeYear entity = MyMapper.JsonClone<CrmIntakeYearDto, CrmIntakeYear>(modelDto);
  //  _repository.CrmIntakeYears.UpdateByState(entity);
  //  await _repository.SaveAsync();

  //  _logger.LogInformation("CrmIntakeYear record updated successfully: {CrmIntakeYearId}", key);

  //  return modelDto;
  //}

  ///// <summary>
  ///// Deletes a CrmIntakeYear record by ID asynchronously.
  ///// </summary>
  //public async Task DeleteAsync(int key)
  //{
  //  if (key <= 0)
  //    throw new BadRequestException("Invalid request!");

  //  _logger.LogInformation("Deleting CrmIntakeYear record with ID: {CrmIntakeYearId}", key);

  //  var record = await _repository.CrmIntakeYears.ByIdAsync(
  //      x => x.CrmIntakeYearId == key, trackChanges: false);

  //  if (record == null)
  //    throw new NotFoundException("CrmIntakeYear", "CrmIntakeYearId", key.ToString());

  //  await _repository.CrmIntakeYears.DeleteAsync(x => x.CrmIntakeYearId == key, trackChanges: false);
  //  await _repository.SaveAsync();

  //  _logger.LogInformation("CrmIntakeYear record deleted successfully: {CrmIntakeYearId}", key);
  //}

  ///// <summary>
  ///// Retrieves CrmIntakeYear records for dropdown list asynchronously.
  ///// </summary>
  //public async Task<IEnumerable<CrmIntakeYearForDDLDto>> CrmIntakeYearsForDDLAsync()
  //{
  //  _logger.LogInformation("Fetching CrmIntakeYear records for dropdown list");

  //  var records = await _repository.CrmIntakeYears.ListWithSelectAsync(
  //      x => new CrmIntakeYear
  //      {
  //        CrmIntakeYearId = x.CrmIntakeYearId,
  //        Name = x.Name
  //      },
  //      orderBy: x => x.Name,
  //      trackChanges: false
  //  );

  //  if (!records.Any())
  //    return new List<CrmIntakeYearForDDLDto>();

  //  var recordsForDDLDto = MyMapper.JsonCloneIEnumerableToList<CrmIntakeYear, CrmIntakeYearForDDLDto>(records);
  //  return recordsForDDLDto;
  //}
}
