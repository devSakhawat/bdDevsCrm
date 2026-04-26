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

internal sealed class CrmLeadStageService : ICrmLeadStageService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmLeadStageService> _logger;
  private readonly IConfiguration _configuration;

  public CrmLeadStageService(IRepositoryManager repository, ILogger<CrmLeadStageService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<CrmLeadStageDto> CreateAsync(CreateCrmLeadStageRecord record, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(CreateCrmLeadStageRecord));

    bool exists = await _repository.CrmLeadStages.ExistsAsync(x => x.LeadStageName.Trim().ToLower() == record.LeadStageName.Trim().ToLower(), cancellationToken: cancellationToken);
    if (exists)
      throw new ConflictException("Lead Stage with this name already exists!");

    CrmLeadStage entity = record.MapTo<CrmLeadStage>();
    int newId = await _repository.CrmLeadStages.CreateAndIdAsync(entity, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Lead Stage created successfully. ID: {LeadStageId}", newId);

    var resultDto = entity.MapTo<CrmLeadStageDto>() with { LeadStageId = newId };
    return resultDto;
  }

  public async Task<CrmLeadStageDto> UpdateAsync(UpdateCrmLeadStageRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(UpdateCrmLeadStageRecord));

    var existing = await _repository.CrmLeadStages.FirstOrDefaultAsync(x => x.LeadStageId == record.LeadStageId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Lead Stage", "LeadStageId", record.LeadStageId.ToString());

    bool duplicateExists = await _repository.CrmLeadStages.ExistsAsync(x => x.LeadStageName.Trim().ToLower() == record.LeadStageName.Trim().ToLower() && x.LeadStageId != record.LeadStageId, cancellationToken: cancellationToken);
    if (duplicateExists)
      throw new ConflictException("Lead Stage with this name already exists!");

    CrmLeadStage entity = record.MapTo<CrmLeadStage>();
    _repository.CrmLeadStages.UpdateByState(entity);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Lead Stage updated successfully. ID: {LeadStageId}", record.LeadStageId);
    return entity.MapTo<CrmLeadStageDto>();
  }

  public async Task DeleteAsync(DeleteCrmLeadStageRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null || record.LeadStageId <= 0)
      throw new BadRequestException("Invalid delete request!");

    var entity = await _repository.CrmLeadStages.FirstOrDefaultAsync(x => x.LeadStageId == record.LeadStageId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Lead Stage", "LeadStageId", record.LeadStageId.ToString());

    await _repository.CrmLeadStages.DeleteAsync(x => x.LeadStageId == record.LeadStageId, trackChanges: false, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogWarning("Lead Stage deleted successfully. ID: {LeadStageId}", record.LeadStageId);
  }

  public async Task<CrmLeadStageDto> LeadStageAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.CrmLeadStages.FirstOrDefaultAsync(x => x.LeadStageId == id, trackChanges, cancellationToken)
      ?? throw new NotFoundException("Lead Stage", "LeadStageId", id.ToString());

    return entity.MapTo<CrmLeadStageDto>();
  }

  public async Task<IEnumerable<CrmLeadStageDto>> LeadStagesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmLeadStages.LeadStagesAsync(trackChanges, cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Lead Stage records found.");
      return Enumerable.Empty<CrmLeadStageDto>();
    }

    return entities.MapToList<CrmLeadStageDto>();
  }

  public async Task<IEnumerable<CrmLeadStageDDLDto>> LeadStageForDDLAsync(CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmLeadStages.ListByConditionAsync(x => true, x => x.LeadStageName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Lead Stage records found for dropdown.");
      return Enumerable.Empty<CrmLeadStageDDLDto>();
    }

    return entities.MapToList<CrmLeadStageDDLDto>();
  }

  public async Task<GridEntity<CrmLeadStageDto>> LeadStageSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    const string query = @"SELECT LeadStageId, LeadStageName, StageType, IsClosedStage, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmLeadStage";
    const string orderBy = "LeadStageName ASC";
    return await _repository.CrmLeadStages.AdoGridDataAsync<CrmLeadStageDto>(query, options, orderBy, string.Empty, cancellationToken);
  }
}
