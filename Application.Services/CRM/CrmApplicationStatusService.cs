using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmApplicationStatusService : ICrmApplicationStatusService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmApplicationStatusService> _logger;
  private readonly IConfiguration _configuration;

  public CrmApplicationStatusService(IRepositoryManager repository, ILogger<CrmApplicationStatusService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<CrmApplicationStatusDto> CreateAsync(CreateCrmApplicationStatusRecord record, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(CreateCrmApplicationStatusRecord));

    bool exists = await _repository.CrmApplicationStatuses.ExistsAsync(x => x.ApplicationStatusName.Trim().ToLower() == record.ApplicationStatusName.Trim().ToLower(), cancellationToken: cancellationToken);
    if (exists)
      throw new ConflictException("Application Status with this name already exists!");

    CrmApplicationStatus entity = record.MapTo<CrmApplicationStatus>();
    int newId = await _repository.CrmApplicationStatuses.CreateAndIdAsync(entity, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Application Status created successfully. ID: {ApplicationStatusId}", newId);

    var resultDto = entity.MapTo<CrmApplicationStatusDto>() with { ApplicationStatusId = newId };
    return resultDto;
  }

  public async Task<CrmApplicationStatusDto> UpdateAsync(UpdateCrmApplicationStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(UpdateCrmApplicationStatusRecord));

    var existing = await _repository.CrmApplicationStatuses.FirstOrDefaultAsync(x => x.ApplicationStatusId == record.ApplicationStatusId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Application Status", "ApplicationStatusId", record.ApplicationStatusId.ToString());

    bool duplicateExists = await _repository.CrmApplicationStatuses.ExistsAsync(x => x.ApplicationStatusName.Trim().ToLower() == record.ApplicationStatusName.Trim().ToLower() && x.ApplicationStatusId != record.ApplicationStatusId, cancellationToken: cancellationToken);
    if (duplicateExists)
      throw new ConflictException("Application Status with this name already exists!");

    CrmApplicationStatus entity = record.MapTo<CrmApplicationStatus>();
    _repository.CrmApplicationStatuses.UpdateByState(entity);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Application Status updated successfully. ID: {ApplicationStatusId}", record.ApplicationStatusId);
    return entity.MapTo<CrmApplicationStatusDto>();
  }

  public async Task DeleteAsync(DeleteCrmApplicationStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null || record.ApplicationStatusId <= 0)
      throw new BadRequestException("Invalid delete request!");

    var entity = await _repository.CrmApplicationStatuses.FirstOrDefaultAsync(x => x.ApplicationStatusId == record.ApplicationStatusId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Application Status", "ApplicationStatusId", record.ApplicationStatusId.ToString());

    await _repository.CrmApplicationStatuses.DeleteAsync(x => x.ApplicationStatusId == record.ApplicationStatusId, trackChanges: false, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogWarning("Application Status deleted successfully. ID: {ApplicationStatusId}", record.ApplicationStatusId);
  }

  public async Task<CrmApplicationStatusDto> ApplicationStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.CrmApplicationStatuses.FirstOrDefaultAsync(x => x.ApplicationStatusId == id, trackChanges, cancellationToken)
      ?? throw new NotFoundException("Application Status", "ApplicationStatusId", id.ToString());

    return entity.MapTo<CrmApplicationStatusDto>();
  }

  public async Task<IEnumerable<CrmApplicationStatusDto>> ApplicationStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmApplicationStatuses.ApplicationStatusesAsync(trackChanges, cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Application Status records found.");
      return Enumerable.Empty<CrmApplicationStatusDto>();
    }

    return entities.MapToList<CrmApplicationStatusDto>();
  }

  public async Task<IEnumerable<CrmApplicationStatusDDLDto>> ApplicationStatusForDDLAsync(CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmApplicationStatuses.ListByConditionAsync(x => true, x => x.SequenceNo, trackChanges: false, descending: false, cancellationToken: cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Application Status records found for dropdown.");
      return Enumerable.Empty<CrmApplicationStatusDDLDto>();
    }

    return entities.MapToList<CrmApplicationStatusDDLDto>();
  }

  public async Task<GridEntity<CrmApplicationStatusDto>> ApplicationStatusSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    const string query = @"SELECT ApplicationStatusId, ApplicationStatusName, SequenceNo, IsFinalStatus, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmApplicationStatus";
    const string orderBy = "ApplicationStatusName ASC";
    return await _repository.CrmApplicationStatuses.AdoGridDataAsync<CrmApplicationStatusDto>(query, options, orderBy, string.Empty, cancellationToken);
  }
}
