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

internal sealed class ThanaService : IThanaService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<ThanaService> _logger;
    private readonly IConfiguration _configuration;

    public ThanaService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<ThanaService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<ThanaDto> CreateAsync(CreateThanaRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateThanaRecord));

        var validator = new CreateThanaRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new thana. Name: {ThanaName}, Time: {Time}",
            record.ThanaName, DateTime.UtcNow);

        bool exists = await _repository.Thanas.ExistsAsync(
            x => x.ThanaName != null && x.ThanaName.Trim().ToLower() == record.ThanaName!.Trim().ToLower() 
                && x.DistrictId == record.DistrictId,
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("Thana", "ThanaName");

        Thana thana = record.MapTo<Thana>();
        
        int thanaId = await _repository.Thanas.CreateAndIdAsync(thana, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Thana created successfully. ID: {ThanaId}, Time: {Time}",
            thanaId, DateTime.UtcNow);

        await _cache.RemoveAsync("Thana:All");
        await _cache.RemoveAsync("Thana:Active");
        await _cache.RemoveAsync($"Thana:District:{record.DistrictId}");

        var resultDto = thana.MapTo<ThanaDto>();
        resultDto.ThanaId = thanaId;
        return resultDto;
    }

    public async Task<ThanaDto> UpdateAsync(UpdateThanaRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateThanaRecord));

        var validator = new UpdateThanaRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating thana. ID: {ThanaId}, Time: {Time}", 
            record.ThanaId, DateTime.UtcNow);

        var existing = await _repository.Thanas.ThanaAsync(
            record.ThanaId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Thana", "ThanaId", record.ThanaId.ToString());

        bool duplicateExists = await _repository.Thanas.ExistsAsync(
            x => x.ThanaName != null && x.ThanaName.Trim().ToLower() == record.ThanaName!.Trim().ToLower()
                && x.DistrictId == record.DistrictId && x.ThanaId != record.ThanaId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("Thana", "ThanaName");

        Thana thana = record.MapTo<Thana>();
        _repository.Thanas.UpdateByState(thana);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Thana updated successfully. ID: {ThanaId}, Time: {Time}",
            record.ThanaId, DateTime.UtcNow);

        await _cache.RemoveAsync("Thana:All");
        await _cache.RemoveAsync("Thana:Active");
        await _cache.RemoveAsync($"Thana:{record.ThanaId}");
        await _cache.RemoveAsync($"Thana:District:{record.DistrictId}");

        return thana.MapTo<ThanaDto>();
    }

    public async Task DeleteAsync(DeleteThanaRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ThanaId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var validator = new DeleteThanaRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting thana. ID: {ThanaId}, Time: {Time}", 
            record.ThanaId, DateTime.UtcNow);

        var entity = await _repository.Thanas.ThanaAsync(
            record.ThanaId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Thana", "ThanaId", record.ThanaId.ToString());

        await _repository.Thanas.DeleteAsync(t => t.ThanaId == record.ThanaId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Thana deleted successfully. ID: {ThanaId}, Time: {Time}",
            record.ThanaId, DateTime.UtcNow);

        await _cache.RemoveAsync("Thana:All");
        await _cache.RemoveAsync("Thana:Active");
        await _cache.RemoveAsync($"Thana:{record.ThanaId}");
    }

    public async Task<ThanaDto> ThanaAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching thana. ID: {ThanaId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"Thana:{id}",
            factory: async () =>
            {
                var thana = await _repository.Thanas.ThanaAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("Thana", "ThanaId", id.ToString());

                return thana.MapTo<ThanaDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("Thana", "ThanaId", id.ToString());
    }

    public async Task<IEnumerable<ThanaDto>> ThanasAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all thanas. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "Thana:All",
            factory: async () =>
            {
                var thanas = await _repository.Thanas.ThanasAsync(trackChanges, cancellationToken);

                if (!thanas.Any())
                {
                    _logger.LogWarning("No thanas found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<ThanaDto>();
                }

                return thanas.MapToList<ThanaDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ThanaDto>();
    }

    public async Task<IEnumerable<ThanaDDLDto>> ThanasForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching thanas for dropdown list");

        return await _cache.OrSetAsync(
            key: "Thana:DDL",
            factory: async () =>
            {
                var thanas = await _repository.Thanas.ActiveThanasAsync(trackChanges, cancellationToken);

                if (!thanas.Any())
                {
                    _logger.LogWarning("No active thanas found for dropdown");
                    return Enumerable.Empty<ThanaDDLDto>();
                }

                return thanas.MapToList<ThanaDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ThanaDDLDto>();
    }

    public async Task<IEnumerable<ThanaDto>> ThanasByDistrictIdAsync(int districtId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching thanas for district: {DistrictId}", districtId);

        return await _cache.OrSetAsync(
            key: $"Thana:District:{districtId}",
            factory: async () =>
            {
                var thanas = await _repository.Thanas.ThanasByDistrictIdAsync(districtId, trackChanges, cancellationToken);
                return thanas.MapToList<ThanaDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ThanaDto>();
    }

    public async Task<GridEntity<ThanaDto>> ThanasSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM Thana";
        const string orderBy = "ThanaName ASC";

        _logger.LogInformation("Fetching thanas summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.Thanas.AdoGridDataAsync<ThanaDto>(sql, options, orderBy, "", cancellationToken);
    }
}
