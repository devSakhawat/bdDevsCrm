using Application.Services.Mappings;
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

internal sealed class CrmDocumentVerificationHistoryService : ICrmDocumentVerificationHistoryService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmDocumentVerificationHistoryService> _logger;
    private readonly IConfiguration _config;

    public CrmDocumentVerificationHistoryService(IRepositoryManager repository, ILogger<CrmDocumentVerificationHistoryService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmDocumentVerificationHistoryDto> CreateAsync(CreateCrmDocumentVerificationHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmDocumentVerificationHistoryRecord));
        var entity = record.MapTo<CrmDocumentVerificationHistory>();
        int newId = await _repository.CrmDocumentVerificationHistories.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        var dto = entity.MapTo<CrmDocumentVerificationHistoryDto>();
        dto.DocumentVerificationHistoryId = newId;
        return dto;
    }

    public async Task<CrmDocumentVerificationHistoryDto> UpdateAsync(UpdateCrmDocumentVerificationHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmDocumentVerificationHistoryRecord));
        _ = await _repository.CrmDocumentVerificationHistories.DocumentVerificationHistoryAsync(record.DocumentVerificationHistoryId, false, cancellationToken)
            ?? throw new NotFoundException("CrmDocumentVerificationHistory", "DocumentVerificationHistoryId", record.DocumentVerificationHistoryId.ToString());
        var entity = record.MapTo<CrmDocumentVerificationHistory>();
        _repository.CrmDocumentVerificationHistories.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmDocumentVerificationHistoryDto>();
    }

    public async Task DeleteAsync(DeleteCrmDocumentVerificationHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.DocumentVerificationHistoryId <= 0) throw new BadRequestException("Invalid delete request!");
        await _repository.CrmDocumentVerificationHistories.DeleteAsync(x => x.DocumentVerificationHistoryId == record.DocumentVerificationHistoryId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmDocumentVerificationHistoryDto> DocumentVerificationHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmDocumentVerificationHistories.DocumentVerificationHistoryAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmDocumentVerificationHistory", "DocumentVerificationHistoryId", id.ToString())).MapTo<CrmDocumentVerificationHistoryDto>();

    public async Task<IEnumerable<CrmDocumentVerificationHistoryDto>> DocumentVerificationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmDocumentVerificationHistories.DocumentVerificationHistoriesAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmDocumentVerificationHistoryDto>() : Enumerable.Empty<CrmDocumentVerificationHistoryDto>();
    }

    public async Task<IEnumerable<CrmDocumentVerificationHistoryDto>> DocumentVerificationHistoriesByDocumentIdAsync(int documentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmDocumentVerificationHistories.DocumentVerificationHistoriesByDocumentIdAsync(documentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmDocumentVerificationHistoryDto>() : Enumerable.Empty<CrmDocumentVerificationHistoryDto>();
    }

    public async Task<GridEntity<CrmDocumentVerificationHistoryDto>> DocumentVerificationHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT DocumentVerificationHistoryId, DocumentId, OldStatus, NewStatus, ChangedBy, ChangedDate, Notes FROM CrmDocumentVerificationHistory";
        return await _repository.CrmDocumentVerificationHistories.AdoGridDataAsync<CrmDocumentVerificationHistoryDto>(sql, options, "ChangedDate DESC", string.Empty, cancellationToken);
    }
}
