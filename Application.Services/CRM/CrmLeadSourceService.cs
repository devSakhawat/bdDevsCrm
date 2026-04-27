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

internal sealed class CrmLeadSourceService : ICrmLeadSourceService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmLeadSourceService> _logger;
  private readonly IConfiguration _configuration;

  public CrmLeadSourceService(IRepositoryManager repository, ILogger<CrmLeadSourceService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<CrmLeadSourceDto> CreateAsync(CreateCrmLeadSourceRecord record, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(CreateCrmLeadSourceRecord));

    bool exists = await _repository.CrmLeadSources.ExistsAsync(x => x.LeadSourceName.Trim().ToLower() == record.LeadSourceName.Trim().ToLower(), cancellationToken: cancellationToken);
    if (exists)
      throw new ConflictException("Lead Source with this name already exists!");

    CrmLeadSource entity = record.MapTo<CrmLeadSource>();
    int newId = await _repository.CrmLeadSources.CreateAndIdAsync(entity, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Lead Source created successfully. ID: {LeadSourceId}", newId);

    var resultDto = entity.MapTo<CrmLeadSourceDto>() with { LeadSourceId = newId };
    return resultDto;
  }

  public async Task<CrmLeadSourceDto> UpdateAsync(UpdateCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(UpdateCrmLeadSourceRecord));

    var existing = await _repository.CrmLeadSources.FirstOrDefaultAsync(x => x.LeadSourceId == record.LeadSourceId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Lead Source", "LeadSourceId", record.LeadSourceId.ToString());

    bool duplicateExists = await _repository.CrmLeadSources.ExistsAsync(x => x.LeadSourceName.Trim().ToLower() == record.LeadSourceName.Trim().ToLower() && x.LeadSourceId != record.LeadSourceId, cancellationToken: cancellationToken);
    if (duplicateExists)
      throw new ConflictException("Lead Source with this name already exists!");

    CrmLeadSource entity = record.MapTo<CrmLeadSource>();
    _repository.CrmLeadSources.UpdateByState(entity);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Lead Source updated successfully. ID: {LeadSourceId}", record.LeadSourceId);
    return entity.MapTo<CrmLeadSourceDto>();
  }

  public async Task DeleteAsync(DeleteCrmLeadSourceRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null || record.LeadSourceId <= 0)
      throw new BadRequestException("Invalid delete request!");

    var entity = await _repository.CrmLeadSources.FirstOrDefaultAsync(x => x.LeadSourceId == record.LeadSourceId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Lead Source", "LeadSourceId", record.LeadSourceId.ToString());

    await _repository.CrmLeadSources.DeleteAsync(x => x.LeadSourceId == record.LeadSourceId, trackChanges: false, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogWarning("Lead Source deleted successfully. ID: {LeadSourceId}", record.LeadSourceId);
  }

  public async Task<CrmLeadSourceDto> LeadSourceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.CrmLeadSources.FirstOrDefaultAsync(x => x.LeadSourceId == id, trackChanges, cancellationToken)
      ?? throw new NotFoundException("Lead Source", "LeadSourceId", id.ToString());

    return entity.MapTo<CrmLeadSourceDto>();
  }

  public async Task<IEnumerable<CrmLeadSourceDto>> LeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmLeadSources.LeadSourcesAsync(trackChanges, cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Lead Source records found.");
      return Enumerable.Empty<CrmLeadSourceDto>();
    }

    return entities.MapToList<CrmLeadSourceDto>();
  }

  public async Task<IEnumerable<CrmLeadSourceDDLDto>> LeadSourceForDDLAsync(CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmLeadSources.ListByConditionAsync(x => x.IsActive, x => x.LeadSourceName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Lead Source records found for dropdown.");
      return Enumerable.Empty<CrmLeadSourceDDLDto>();
    }

    return entities.MapToList<CrmLeadSourceDDLDto>();
  }

  public async Task<GridEntity<CrmLeadSourceDto>> LeadSourceSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    const string query = @"SELECT LeadSourceId, LeadSourceName, SortOrder, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmLeadSource";
    const string orderBy = "LeadSourceName ASC";
    return await _repository.CrmLeadSources.AdoGridDataAsync<CrmLeadSourceDto>(query, options, orderBy, string.Empty, cancellationToken);
  }
}
