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

internal sealed class CrmVisaStatusService : ICrmVisaStatusService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmVisaStatusService> _logger;
  private readonly IConfiguration _configuration;

  public CrmVisaStatusService(IRepositoryManager repository, ILogger<CrmVisaStatusService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<CrmVisaStatusDto> CreateAsync(CreateCrmVisaStatusRecord record, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(CreateCrmVisaStatusRecord));

    bool exists = await _repository.CrmVisaStatuses.ExistsAsync(x => x.VisaStatusName.Trim().ToLower() == record.VisaStatusName.Trim().ToLower(), cancellationToken: cancellationToken);
    if (exists)
      throw new ConflictException("Visa Status with this name already exists!");

    CrmVisaStatus entity = record.MapTo<CrmVisaStatus>();
    int newId = await _repository.CrmVisaStatuses.CreateAndIdAsync(entity, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Visa Status created successfully. ID: {VisaStatusId}", newId);

    var resultDto = entity.MapTo<CrmVisaStatusDto>() with { VisaStatusId = newId };
    return resultDto;
  }

  public async Task<CrmVisaStatusDto> UpdateAsync(UpdateCrmVisaStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(UpdateCrmVisaStatusRecord));

    var existing = await _repository.CrmVisaStatuses.FirstOrDefaultAsync(x => x.VisaStatusId == record.VisaStatusId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Visa Status", "VisaStatusId", record.VisaStatusId.ToString());

    bool duplicateExists = await _repository.CrmVisaStatuses.ExistsAsync(x => x.VisaStatusName.Trim().ToLower() == record.VisaStatusName.Trim().ToLower() && x.VisaStatusId != record.VisaStatusId, cancellationToken: cancellationToken);
    if (duplicateExists)
      throw new ConflictException("Visa Status with this name already exists!");

    CrmVisaStatus entity = record.MapTo<CrmVisaStatus>();
    _repository.CrmVisaStatuses.UpdateByState(entity);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Visa Status updated successfully. ID: {VisaStatusId}", record.VisaStatusId);
    return entity.MapTo<CrmVisaStatusDto>();
  }

  public async Task DeleteAsync(DeleteCrmVisaStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null || record.VisaStatusId <= 0)
      throw new BadRequestException("Invalid delete request!");

    var entity = await _repository.CrmVisaStatuses.FirstOrDefaultAsync(x => x.VisaStatusId == record.VisaStatusId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Visa Status", "VisaStatusId", record.VisaStatusId.ToString());

    await _repository.CrmVisaStatuses.DeleteAsync(x => x.VisaStatusId == record.VisaStatusId, trackChanges: false, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogWarning("Visa Status deleted successfully. ID: {VisaStatusId}", record.VisaStatusId);
  }

  public async Task<CrmVisaStatusDto> VisaStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.CrmVisaStatuses.FirstOrDefaultAsync(x => x.VisaStatusId == id, trackChanges, cancellationToken)
      ?? throw new NotFoundException("Visa Status", "VisaStatusId", id.ToString());

    return entity.MapTo<CrmVisaStatusDto>();
  }

  public async Task<IEnumerable<CrmVisaStatusDto>> VisaStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmVisaStatuses.VisaStatusesAsync(trackChanges, cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Visa Status records found.");
      return Enumerable.Empty<CrmVisaStatusDto>();
    }

    return entities.MapToList<CrmVisaStatusDto>();
  }

  public async Task<IEnumerable<CrmVisaStatusDDLDto>> VisaStatusForDDLAsync(CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmVisaStatuses.ListByConditionAsync(x => true, x => x.SequenceNo, trackChanges: false, descending: false, cancellationToken: cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Visa Status records found for dropdown.");
      return Enumerable.Empty<CrmVisaStatusDDLDto>();
    }

    return entities.MapToList<CrmVisaStatusDDLDto>();
  }

  public async Task<GridEntity<CrmVisaStatusDto>> VisaStatusSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    const string query = @"SELECT VisaStatusId, VisaStatusName, SequenceNo, IsFinalStatus, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmVisaStatus";
    const string orderBy = "VisaStatusName ASC";
    return await _repository.CrmVisaStatuses.AdoGridDataAsync<CrmVisaStatusDto>(query, options, orderBy, string.Empty, cancellationToken);
  }
}
