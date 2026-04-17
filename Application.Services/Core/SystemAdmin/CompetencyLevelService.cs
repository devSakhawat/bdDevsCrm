using Application.Services.Caching;
using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Domain.Exceptions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.Core.SystemAdmin;

internal sealed class CompetencyLevelService : ICompetencyLevelService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<CompetencyLevelService> _logger;
    private readonly IConfiguration _configuration;

    public CompetencyLevelService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<CompetencyLevelService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CompetencyLevelDto> CreateAsync(CreateCompetencyLevelRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCompetencyLevelRecord));

        var validator = new CreateCompetencyLevelRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new competency level. Title: {LevelTitle}, Time: {Time}",
            record.LevelTitle, DateTime.UtcNow);

        CompetencyLevel competencyLevel = record.MapTo<CompetencyLevel>();
        
        int levelId = await _repository.CompetencyLevels.CreateAndIdAsync(competencyLevel, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("CompetencyLevel created successfully. ID: {LevelId}, Time: {Time}",
            levelId, DateTime.UtcNow);

        await _cache.RemoveAsync("CompetencyLevel:All");

        var resultDto = competencyLevel.MapTo<CompetencyLevelDto>();
        resultDto.LevelId = levelId;
        return resultDto;
    }

    public async Task<CompetencyLevelDto> UpdateAsync(UpdateCompetencyLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCompetencyLevelRecord));

        var validator = new UpdateCompetencyLevelRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var competencyLevel = await _repository.CompetencyLevels.CompetencyLevelAsync(record.LevelId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CompetencyLevel", "LevelId", record.LevelId.ToString());

        _logger.LogInformation("Updating competency level. ID: {LevelId}, Time: {Time}",
            record.LevelId, DateTime.UtcNow);

        record.MapTo(competencyLevel);
        
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("CompetencyLevel updated successfully. ID: {LevelId}, Time: {Time}",
            record.LevelId, DateTime.UtcNow);

        await _cache.RemoveAsync($"CompetencyLevel:{record.LevelId}");
        await _cache.RemoveAsync("CompetencyLevel:All");

        return competencyLevel.MapTo<CompetencyLevelDto>();
    }

    public async Task DeleteAsync(DeleteCompetencyLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(DeleteCompetencyLevelRecord));

        var competencyLevel = await _repository.CompetencyLevels.CompetencyLevelAsync(record.LevelId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CompetencyLevel", "LevelId", record.LevelId.ToString());

        _logger.LogInformation("Deleting competency level. ID: {LevelId}, Time: {Time}",
            record.LevelId, DateTime.UtcNow);

        _repository.CompetencyLevels.Delete(competencyLevel);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("CompetencyLevel deleted successfully. ID: {LevelId}, Time: {Time}",
            record.LevelId, DateTime.UtcNow);

        await _cache.RemoveAsync($"CompetencyLevel:{record.LevelId}");
        await _cache.RemoveAsync("CompetencyLevel:All");
    }

    public async Task<CompetencyLevelDto> CompetencyLevelAsync(int levelId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: $"CompetencyLevel:{levelId}",
            factory: async () =>
            {
                var competencyLevel = await _repository.CompetencyLevels.CompetencyLevelAsync(levelId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("CompetencyLevel", "LevelId", levelId.ToString());
                return competencyLevel.MapTo<CompetencyLevelDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("CompetencyLevel", "LevelId", levelId.ToString());
    }

    public async Task<IEnumerable<CompetencyLevelDto>> CompetencyLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "CompetencyLevel:All",
            factory: async () =>
            {
                var competencyLevels = await _repository.CompetencyLevels.CompetencyLevelsAsync(trackChanges, cancellationToken);
                return competencyLevels.MapToList<CompetencyLevelDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CompetencyLevelDto>();
    }

    public async Task<IEnumerable<CompetencyLevelDDLDto>> CompetencyLevelsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "CompetencyLevel:All",
            factory: async () =>
            {
                var competencyLevels = await _repository.CompetencyLevels.CompetencyLevelsAsync(trackChanges, cancellationToken);
                return competencyLevels.MapToList<CompetencyLevelDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CompetencyLevelDDLDto>();
    }

    public async Task<GridEntity<CompetencyLevelDto>> CompetencyLevelsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM CompetencyLevel";
        const string orderBy = "CompetencyLevelId ASC";

        _logger.LogInformation("Fetching competency level summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.CompetencyLevels.AdoGridDataAsync<CompetencyLevelDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
