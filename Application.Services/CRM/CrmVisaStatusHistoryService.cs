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

internal sealed class CrmVisaStatusHistoryService : ICrmVisaStatusHistoryService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmVisaStatusHistoryService> _logger;
    private readonly IConfiguration _configuration;

    public CrmVisaStatusHistoryService(IRepositoryManager repository, ILogger<CrmVisaStatusHistoryService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmVisaStatusHistoryDto> CreateAsync(CreateCrmVisaStatusHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmVisaStatusHistoryRecord));
        var entity = record.MapTo<CrmVisaStatusHistory>();
        int newId = await _repository.CrmVisaStatusHistories.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.VisaStatusHistoryId = newId;
        return entity.MapTo<CrmVisaStatusHistoryDto>();
    }

    public async Task<CrmVisaStatusHistoryDto> UpdateAsync(UpdateCrmVisaStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmVisaStatusHistoryRecord));
        _ = await _repository.CrmVisaStatusHistories.VisaStatusHistoryAsync(record.VisaStatusHistoryId, false, cancellationToken)
            ?? throw new NotFoundException("CrmVisaStatusHistory", "VisaStatusHistoryId", record.VisaStatusHistoryId.ToString());
        var entity = record.MapTo<CrmVisaStatusHistory>();
        _repository.CrmVisaStatusHistories.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmVisaStatusHistoryDto>();
    }

    public async Task DeleteAsync(DeleteCrmVisaStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.VisaStatusHistoryId <= 0) throw new BadRequestException("Invalid delete request!");
        await _repository.CrmVisaStatusHistories.DeleteAsync(x => x.VisaStatusHistoryId == record.VisaStatusHistoryId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmVisaStatusHistoryDto> VisaStatusHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmVisaStatusHistories.VisaStatusHistoryAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmVisaStatusHistory", "VisaStatusHistoryId", id.ToString())).MapTo<CrmVisaStatusHistoryDto>();

    public async Task<IEnumerable<CrmVisaStatusHistoryDto>> VisaStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmVisaStatusHistories.VisaStatusHistoriesAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmVisaStatusHistoryDto>() : Enumerable.Empty<CrmVisaStatusHistoryDto>();
    }

    public async Task<IEnumerable<CrmVisaStatusHistoryDto>> VisaStatusHistoriesByVisaApplicationIdAsync(int visaApplicationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmVisaStatusHistories.VisaStatusHistoriesByVisaApplicationIdAsync(visaApplicationId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmVisaStatusHistoryDto>() : Enumerable.Empty<CrmVisaStatusHistoryDto>();
    }

    public async Task<GridEntity<CrmVisaStatusHistoryDto>> VisaStatusHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT VisaStatusHistoryId, VisaApplicationId, OldStatus, NewStatus, ChangedBy, ChangedDate, Notes FROM CrmVisaStatusHistory";
        return await _repository.CrmVisaStatusHistories.AdoGridDataAsync<CrmVisaStatusHistoryDto>(sql, options, "VisaStatusHistoryId DESC", string.Empty, cancellationToken);
    }
}
