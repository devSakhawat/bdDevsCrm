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

internal sealed class CrmApplicationDocumentService : ICrmApplicationDocumentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmApplicationDocumentService> _logger;
    private readonly IConfiguration _config;

    public CrmApplicationDocumentService(IRepositoryManager repository, ILogger<CrmApplicationDocumentService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmApplicationDocumentDto> CreateAsync(CreateCrmApplicationDocumentRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmApplicationDocumentRecord));
        var entity = record.MapTo<CrmApplicationDocument>();
        int newId = await _repository.CrmApplicationDocuments.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        var dto = entity.MapTo<CrmApplicationDocumentDto>();
        dto.ApplicationDocumentId = newId;
        return dto;
    }

    public async Task<CrmApplicationDocumentDto> UpdateAsync(UpdateCrmApplicationDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmApplicationDocumentRecord));
        _ = await _repository.CrmApplicationDocuments.ApplicationDocumentAsync(record.ApplicationDocumentId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplicationDocument", "ApplicationDocumentId", record.ApplicationDocumentId.ToString());
        var entity = record.MapTo<CrmApplicationDocument>();
        _repository.CrmApplicationDocuments.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmApplicationDocumentDto>();
    }

    public async Task DeleteAsync(DeleteCrmApplicationDocumentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ApplicationDocumentId <= 0) throw new BadRequestException("Invalid delete request!");
        await _repository.CrmApplicationDocuments.DeleteAsync(x => x.ApplicationDocumentId == record.ApplicationDocumentId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmApplicationDocumentDto> ApplicationDocumentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmApplicationDocuments.ApplicationDocumentAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmApplicationDocument", "ApplicationDocumentId", id.ToString())).MapTo<CrmApplicationDocumentDto>();

    public async Task<IEnumerable<CrmApplicationDocumentDto>> ApplicationDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmApplicationDocuments.ApplicationDocumentsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmApplicationDocumentDto>() : Enumerable.Empty<CrmApplicationDocumentDto>();
    }

    public async Task<IEnumerable<CrmApplicationDocumentDto>> ApplicationDocumentsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmApplicationDocuments.ApplicationDocumentsByApplicationIdAsync(applicationId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmApplicationDocumentDto>() : Enumerable.Empty<CrmApplicationDocumentDto>();
    }

    public async Task<GridEntity<CrmApplicationDocumentDto>> ApplicationDocumentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT ApplicationDocumentId, ApplicationId, DocumentId, IsRequired, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmApplicationDocument";
        return await _repository.CrmApplicationDocuments.AdoGridDataAsync<CrmApplicationDocumentDto>(sql, options, "ApplicationDocumentId DESC", string.Empty, cancellationToken);
    }
}
