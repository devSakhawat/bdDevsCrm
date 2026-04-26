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

internal sealed class CrmCommunicationTypeService : ICrmCommunicationTypeService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmCommunicationTypeService> _logger;
  private readonly IConfiguration _configuration;

  public CrmCommunicationTypeService(IRepositoryManager repository, ILogger<CrmCommunicationTypeService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<CrmCommunicationTypeDto> CreateAsync(CreateCrmCommunicationTypeRecord record, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(CreateCrmCommunicationTypeRecord));

    bool exists = await _repository.CrmCommunicationTypes.ExistsAsync(x => x.CommunicationTypeName.Trim().ToLower() == record.CommunicationTypeName.Trim().ToLower(), cancellationToken: cancellationToken);
    if (exists)
      throw new ConflictException("Communication Type with this name already exists!");

    CrmCommunicationType entity = record.MapTo<CrmCommunicationType>();
    int newId = await _repository.CrmCommunicationTypes.CreateAndIdAsync(entity, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Communication Type created successfully. ID: {CommunicationTypeId}", newId);

    var resultDto = entity.MapTo<CrmCommunicationTypeDto>() with { CommunicationTypeId = newId };
    return resultDto;
  }

  public async Task<CrmCommunicationTypeDto> UpdateAsync(UpdateCrmCommunicationTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(UpdateCrmCommunicationTypeRecord));

    var existing = await _repository.CrmCommunicationTypes.FirstOrDefaultAsync(x => x.CommunicationTypeId == record.CommunicationTypeId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Communication Type", "CommunicationTypeId", record.CommunicationTypeId.ToString());

    bool duplicateExists = await _repository.CrmCommunicationTypes.ExistsAsync(x => x.CommunicationTypeName.Trim().ToLower() == record.CommunicationTypeName.Trim().ToLower() && x.CommunicationTypeId != record.CommunicationTypeId, cancellationToken: cancellationToken);
    if (duplicateExists)
      throw new ConflictException("Communication Type with this name already exists!");

    CrmCommunicationType entity = record.MapTo<CrmCommunicationType>();
    _repository.CrmCommunicationTypes.UpdateByState(entity);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Communication Type updated successfully. ID: {CommunicationTypeId}", record.CommunicationTypeId);
    return entity.MapTo<CrmCommunicationTypeDto>();
  }

  public async Task DeleteAsync(DeleteCrmCommunicationTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null || record.CommunicationTypeId <= 0)
      throw new BadRequestException("Invalid delete request!");

    var entity = await _repository.CrmCommunicationTypes.FirstOrDefaultAsync(x => x.CommunicationTypeId == record.CommunicationTypeId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Communication Type", "CommunicationTypeId", record.CommunicationTypeId.ToString());

    await _repository.CrmCommunicationTypes.DeleteAsync(x => x.CommunicationTypeId == record.CommunicationTypeId, trackChanges: false, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogWarning("Communication Type deleted successfully. ID: {CommunicationTypeId}", record.CommunicationTypeId);
  }

  public async Task<CrmCommunicationTypeDto> CommunicationTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.CrmCommunicationTypes.FirstOrDefaultAsync(x => x.CommunicationTypeId == id, trackChanges, cancellationToken)
      ?? throw new NotFoundException("Communication Type", "CommunicationTypeId", id.ToString());

    return entity.MapTo<CrmCommunicationTypeDto>();
  }

  public async Task<IEnumerable<CrmCommunicationTypeDto>> CommunicationTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmCommunicationTypes.CommunicationTypesAsync(trackChanges, cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Communication Type records found.");
      return Enumerable.Empty<CrmCommunicationTypeDto>();
    }

    return entities.MapToList<CrmCommunicationTypeDto>();
  }

  public async Task<IEnumerable<CrmCommunicationTypeDDLDto>> CommunicationTypeForDDLAsync(CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmCommunicationTypes.ListByConditionAsync(x => true, x => x.CommunicationTypeName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Communication Type records found for dropdown.");
      return Enumerable.Empty<CrmCommunicationTypeDDLDto>();
    }

    return entities.MapToList<CrmCommunicationTypeDDLDto>();
  }

  public async Task<GridEntity<CrmCommunicationTypeDto>> CommunicationTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    const string query = @"SELECT CommunicationTypeId, CommunicationTypeName, IsDigitalChannel, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmCommunicationType";
    const string orderBy = "CommunicationTypeName ASC";
    return await _repository.CrmCommunicationTypes.AdoGridDataAsync<CrmCommunicationTypeDto>(query, options, orderBy, string.Empty, cancellationToken);
  }
}
