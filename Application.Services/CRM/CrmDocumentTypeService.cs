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

internal sealed class CrmDocumentTypeService : ICrmDocumentTypeService
{
  private readonly IRepositoryManager _repository;
  private readonly ILogger<CrmDocumentTypeService> _logger;
  private readonly IConfiguration _configuration;

  public CrmDocumentTypeService(IRepositoryManager repository, ILogger<CrmDocumentTypeService> logger, IConfiguration configuration)
  {
    _repository = repository;
    _logger = logger;
    _configuration = configuration;
  }

  public async Task<CrmDocumentTypeDto> CreateAsync(CreateCrmDocumentTypeRecord record, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(CreateCrmDocumentTypeRecord));

    bool exists = await _repository.CrmDocumentTypes.ExistsAsync(x => x.DocumentTypeName.Trim().ToLower() == record.DocumentTypeName.Trim().ToLower(), cancellationToken: cancellationToken);
    if (exists)
      throw new ConflictException("Document Type with this name already exists!");

    CrmDocumentType entity = record.MapTo<CrmDocumentType>();
    int newId = await _repository.CrmDocumentTypes.CreateAndIdAsync(entity, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Document Type created successfully. ID: {DocumentTypeId}", newId);
    return entity.MapTo<CrmDocumentTypeDto>() with { DocumentTypeId = newId };
  }

  public async Task<CrmDocumentTypeDto> UpdateAsync(UpdateCrmDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null)
      throw new BadRequestException(nameof(UpdateCrmDocumentTypeRecord));

    var existing = await _repository.CrmDocumentTypes.FirstOrDefaultAsync(x => x.DocumentTypeId == record.DocumentTypeId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Document Type", "DocumentTypeId", record.DocumentTypeId.ToString());

    bool duplicateExists = await _repository.CrmDocumentTypes.ExistsAsync(x => x.DocumentTypeName.Trim().ToLower() == record.DocumentTypeName.Trim().ToLower() && x.DocumentTypeId != record.DocumentTypeId, cancellationToken: cancellationToken);
    if (duplicateExists)
      throw new ConflictException("Document Type with this name already exists!");

    CrmDocumentType entity = record.MapTo<CrmDocumentType>();
    _repository.CrmDocumentTypes.UpdateByState(entity);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogInformation("Document Type updated successfully. ID: {DocumentTypeId}", record.DocumentTypeId);
    return entity.MapTo<CrmDocumentTypeDto>();
  }

  public async Task DeleteAsync(DeleteCrmDocumentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
  {
    if (record == null || record.DocumentTypeId <= 0)
      throw new BadRequestException("Invalid delete request!");

    var entity = await _repository.CrmDocumentTypes.FirstOrDefaultAsync(x => x.DocumentTypeId == record.DocumentTypeId, trackChanges: false, cancellationToken)
      ?? throw new NotFoundException("Document Type", "DocumentTypeId", record.DocumentTypeId.ToString());

    await _repository.CrmDocumentTypes.DeleteAsync(x => x.DocumentTypeId == record.DocumentTypeId, trackChanges: false, cancellationToken);
    await _repository.SaveAsync(cancellationToken);

    _logger.LogWarning("Document Type deleted successfully. ID: {DocumentTypeId}", record.DocumentTypeId);
  }

  public async Task<CrmDocumentTypeDto> DocumentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entity = await _repository.CrmDocumentTypes.FirstOrDefaultAsync(x => x.DocumentTypeId == id, trackChanges, cancellationToken)
      ?? throw new NotFoundException("Document Type", "DocumentTypeId", id.ToString());

    return entity.MapTo<CrmDocumentTypeDto>();
  }

  public async Task<IEnumerable<CrmDocumentTypeDto>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmDocumentTypes.DocumentTypesAsync(trackChanges, cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Document Type records found.");
      return Enumerable.Empty<CrmDocumentTypeDto>();
    }

    return entities.MapToList<CrmDocumentTypeDto>();
  }

  public async Task<IEnumerable<CrmDocumentTypeDDLDto>> DocumentTypeForDDLAsync(CancellationToken cancellationToken = default)
  {
    var entities = await _repository.CrmDocumentTypes.ListByConditionAsync(x => x.IsActive, x => x.DocumentTypeName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
    if (!entities.Any())
    {
      _logger.LogWarning("No Document Type records found for dropdown.");
      return Enumerable.Empty<CrmDocumentTypeDDLDto>();
    }

    return entities.MapToList<CrmDocumentTypeDDLDto>();
  }

  public async Task<GridEntity<CrmDocumentTypeDto>> DocumentTypeSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
  {
    const string query = @"SELECT * FROM CrmDocumentType";
    const string orderBy = "DocumentTypeName ASC";
    return await _repository.CrmDocumentTypes.AdoGridDataAsync<CrmDocumentTypeDto>(query, options, orderBy, string.Empty, cancellationToken);
  }
}
