using Application.Services.Caching;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.DMS;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.DMS;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.DMS;
using Domain.Entities.Entities.DMS;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.DMS;

internal sealed class DmsFileUpdateHistoryService : IDmsFileUpdateHistoryService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<DmsFileUpdateHistoryService> _logger;
    private readonly IConfiguration _configuration;

    public DmsFileUpdateHistoryService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<DmsFileUpdateHistoryService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<DmsFileUpdateHistoryDto> CreateAsync(CreateDmsFileUpdateHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateDmsFileUpdateHistoryRecord));

        _logger.LogInformation("Creating new file update history. EntityId: {EntityId}, Time: {Time}",
            record.EntityId, DateTime.UtcNow);

        DmsFileUpdateHistory fileUpdateHistory = record.MapTo<DmsFileUpdateHistory>();
        fileUpdateHistory.UpdatedDate = DateTime.UtcNow;
        
        int id = await _repository.DmsFileUpdateHistories.CreateAndIdAsync(fileUpdateHistory, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("File update history created successfully. ID: {Id}, Time: {Time}",
            id, DateTime.UtcNow);

        await _cache.RemoveAsync("FileUpdateHistory:All");
        await _cache.RemoveAsync($"FileUpdateHistory:Entity:{record.EntityId}");

        var resultDto = fileUpdateHistory.MapTo<DmsFileUpdateHistoryDto>();
        resultDto.Id = id;
        return resultDto;
    }

    public async Task<DmsFileUpdateHistoryDto> UpdateAsync(UpdateDmsFileUpdateHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateDmsFileUpdateHistoryRecord));

        _logger.LogInformation("Updating file update history. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        var existing = await _repository.DmsFileUpdateHistories.FileUpdateHistoryAsync(
            record.Id, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("DmsFileUpdateHistory", "Id", record.Id.ToString());

        DmsFileUpdateHistory fileUpdateHistory = record.MapTo<DmsFileUpdateHistory>();
        _repository.DmsFileUpdateHistories.UpdateByState(fileUpdateHistory);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("File update history updated successfully. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        await _cache.RemoveAsync("FileUpdateHistory:All");
        await _cache.RemoveAsync($"FileUpdateHistory:{record.Id}");
        await _cache.RemoveAsync($"FileUpdateHistory:Entity:{record.EntityId}");

        return fileUpdateHistory.MapTo<DmsFileUpdateHistoryDto>();
    }

    public async Task DeleteAsync(DeleteDmsFileUpdateHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.Id <= 0)
            throw new BadRequestException("Invalid delete request!");

        _logger.LogInformation("Deleting file update history. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        var entity = await _repository.DmsFileUpdateHistories.FileUpdateHistoryAsync(
            record.Id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DmsFileUpdateHistory", "Id", record.Id.ToString());

        await _repository.DmsFileUpdateHistories.DeleteAsync(f => f.Id == record.Id, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("File update history deleted successfully. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        await _cache.RemoveAsync("FileUpdateHistory:All");
        await _cache.RemoveAsync($"FileUpdateHistory:{record.Id}");
    }

    public async Task<DmsFileUpdateHistoryDto> DmsFileUpdateHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching file update history. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"FileUpdateHistory:{id}",
            factory: async () =>
            {
                var fileUpdateHistory = await _repository.DmsFileUpdateHistories.FileUpdateHistoryAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("DmsFileUpdateHistory", "Id", id.ToString());

                return fileUpdateHistory.MapTo<DmsFileUpdateHistoryDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? throw new NotFoundException("DmsFileUpdateHistory", "Id", id.ToString());
    }

    public async Task<IEnumerable<DmsFileUpdateHistoryDto>> DmsFileUpdateHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all file update histories. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "FileUpdateHistory:All",
            factory: async () =>
            {
                var histories = await _repository.DmsFileUpdateHistories.FileUpdateHistoriesAsync(trackChanges, cancellationToken);

                if (!histories.Any())
                {
                    _logger.LogWarning("No file update histories found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<DmsFileUpdateHistoryDto>();
                }

                return histories.MapToList<DmsFileUpdateHistoryDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? Enumerable.Empty<DmsFileUpdateHistoryDto>();
    }

    public async Task<IEnumerable<DmsFileUpdateHistoryDto>> DmsFileUpdateHistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching file update histories for dropdown list");

        return await _cache.OrSetAsync(
            key: "FileUpdateHistory:DDL",
            factory: async () =>
            {
                var histories = await _repository.DmsFileUpdateHistories.FileUpdateHistoriesAsync(trackChanges, cancellationToken);

                if (!histories.Any())
                {
                    _logger.LogWarning("No file update histories found for dropdown");
                    return Enumerable.Empty<DmsFileUpdateHistoryDDLDto>();
                }

                return histories.MapToList<DmsFileUpdateHistoryDDLDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? Enumerable.Empty<DmsFileUpdateHistoryDDLDto>();
    }

    public async Task<IEnumerable<DmsFileUpdateHistoryDto>> FileUpdateHistoriesByEntityAsync(string entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching file update histories for entity: {EntityId}", entityId);

        return await _cache.OrSetAsync(
            key: $"FileUpdateHistory:Entity:{entityId}",
            factory: async () =>
            {
                var histories = await _repository.DmsFileUpdateHistories.FileUpdateHistoriesByEntityAsync(entityId, trackChanges, cancellationToken);
                return histories.MapToList<DmsFileUpdateHistoryDto>();
            },
            profile: CacheProfile.Dynamic
        ) ?? Enumerable.Empty<DmsFileUpdateHistoryDto>();
    }

    public async Task<GridEntity<DmsFileUpdateHistoryDto>> DmsFileUpdateHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM FileUpdateHistory";
        const string orderBy = "UpdatedDate DESC";

        _logger.LogInformation("Fetching file update histories summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.DmsFileUpdateHistories.AdoGridDataAsync<DmsFileUpdateHistoryDto>(sql, options, orderBy, "", cancellationToken);
    }
}
