using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

internal sealed class CrmFollowUpHistoryService : ICrmFollowUpHistoryService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmFollowUpHistoryService> _logger;
    private readonly IConfiguration _config;

    public CrmFollowUpHistoryService(IRepositoryManager repository, ILogger<CrmFollowUpHistoryService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmFollowUpHistoryDto> CreateAsync(CreateCrmFollowUpHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmFollowUpHistoryRecord));

        var entity = record.MapTo<CrmFollowUpHistory>();
        int newId = await _repository.CrmFollowUpHistories.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogInformation("CrmFollowUpHistory created. ID: {Id}", newId);
        return entity.MapTo<CrmFollowUpHistoryDto>() with { FollowUpHistoryId = newId };
    }

    public async Task<CrmFollowUpHistoryDto> UpdateAsync(UpdateCrmFollowUpHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmFollowUpHistoryRecord));
        _ = await _repository.CrmFollowUpHistories.CrmFollowUpHistoryAsync(record.FollowUpHistoryId, false, cancellationToken)
            ?? throw new NotFoundException("CrmFollowUpHistory", "FollowUpHistoryId", record.FollowUpHistoryId.ToString());

        var entity = record.MapTo<CrmFollowUpHistory>();
        _repository.CrmFollowUpHistories.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmFollowUpHistoryDto>();
    }

    public async Task DeleteAsync(DeleteCrmFollowUpHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.FollowUpHistoryId <= 0) throw new BadRequestException("Invalid delete request!");
        _ = await _repository.CrmFollowUpHistories.CrmFollowUpHistoryAsync(record.FollowUpHistoryId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmFollowUpHistory", "FollowUpHistoryId", record.FollowUpHistoryId.ToString());
        await _repository.CrmFollowUpHistories.DeleteAsync(x => x.FollowUpHistoryId == record.FollowUpHistoryId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmFollowUpHistoryDto> FollowUpHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmFollowUpHistories.CrmFollowUpHistoryAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmFollowUpHistory", "FollowUpHistoryId", id.ToString());
        return entity.MapTo<CrmFollowUpHistoryDto>();
    }

    public async Task<IEnumerable<CrmFollowUpHistoryDto>> FollowUpHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFollowUpHistories.FollowUpHistoriesAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFollowUpHistoryDto>() : Enumerable.Empty<CrmFollowUpHistoryDto>();
    }

    public async Task<IEnumerable<CrmFollowUpHistoryDto>> FollowUpHistoriesByFollowUpIdAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFollowUpHistories.FollowUpHistoriesByFollowUpIdAsync(followUpId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFollowUpHistoryDto>() : Enumerable.Empty<CrmFollowUpHistoryDto>();
    }

    public async Task<GridEntity<CrmFollowUpHistoryDto>> FollowUpHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT FollowUpHistoryId, FollowUpId, OldStatus, NewStatus, ChangedBy, ChangedDate, Remarks FROM CrmFollowUpHistory";
        const string orderBy = "ChangedDate DESC";
        return await _repository.CrmFollowUpHistories.AdoGridDataAsync<CrmFollowUpHistoryDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

}
