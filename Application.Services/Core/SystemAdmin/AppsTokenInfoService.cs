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

/// <summary>
/// Apps token info service implementing business logic for apps token info management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class AppsTokenInfoService : IAppsTokenInfoService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AppsTokenInfoService> _logger;
    private readonly IConfiguration _configuration;

    public AppsTokenInfoService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AppsTokenInfoService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new apps token info record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AppsTokenInfoDto> CreateAsync(CreateAppsTokenInfoRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAppsTokenInfoRecord));

        // FluentValidation
        var validator = new CreateAppsTokenInfoRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new apps token info. TokenNumber: {TokenNumber}, AppsUserId: {AppsUserId}, Time: {Time}",
            record.TokenNumber, record.AppsUserId, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        AppsTokenInfo appsTokenInfo = record.MapTo<AppsTokenInfo>();

        int appsTokenInfoId = await _repository.AppsTokenInfos.CreateAndIdAsync(appsTokenInfo, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Apps token info created successfully. ID: {AppsTokenInfoId}, Time: {Time}",
            appsTokenInfoId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AppsTokenInfo:All");
        await _cache.RemoveAsync("AppsTokenInfo:Active");

        // Return as DTO
        var resultDto = appsTokenInfo.MapTo<AppsTokenInfoDto>();
        resultDto.AppsTokenInfoId = appsTokenInfoId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing apps token info record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AppsTokenInfoDto> UpdateAsync(UpdateAppsTokenInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAppsTokenInfoRecord));

        // FluentValidation
        var validator = new UpdateAppsTokenInfoRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating apps token info. ID: {AppsTokenInfoId}, Time: {Time}",
            record.AppsTokenInfoId, DateTime.UtcNow);

        // Check if apps token info exists
        var existingAppsTokenInfo = await _repository.AppsTokenInfos.AppsTokenInfoAsync(
            record.AppsTokenInfoId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AppsTokenInfo", "AppsTokenInfoId", record.AppsTokenInfoId.ToString());

        // Map Record to Entity using Mapster
        AppsTokenInfo appsTokenInfo = record.MapTo<AppsTokenInfo>();

        _repository.AppsTokenInfos.UpdateByState(appsTokenInfo);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Apps token info updated successfully. ID: {AppsTokenInfoId}, Time: {Time}",
            record.AppsTokenInfoId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AppsTokenInfo:All");
        await _cache.RemoveAsync("AppsTokenInfo:Active");
        await _cache.RemoveAsync($"AppsTokenInfo:{record.AppsTokenInfoId}");

        return appsTokenInfo.MapTo<AppsTokenInfoDto>();
    }

    /// <summary>
    /// Deletes an apps token info record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteAppsTokenInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AppsTokenInfoId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteAppsTokenInfoRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting apps token info. ID: {AppsTokenInfoId}, Time: {Time}",
            record.AppsTokenInfoId, DateTime.UtcNow);

        var appsTokenInfoEntity = await _repository.AppsTokenInfos.AppsTokenInfoAsync(
            record.AppsTokenInfoId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AppsTokenInfo", "AppsTokenInfoId", record.AppsTokenInfoId.ToString());

        await _repository.AppsTokenInfos.DeleteAsync(a => a.AppsTokenInfoId == record.AppsTokenInfoId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Apps token info deleted successfully. ID: {AppsTokenInfoId}, Time: {Time}",
            record.AppsTokenInfoId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AppsTokenInfo:All");
        await _cache.RemoveAsync("AppsTokenInfo:Active");
        await _cache.RemoveAsync($"AppsTokenInfo:{record.AppsTokenInfoId}");
    }

    /// <summary>
    /// Retrieves a single apps token info record by its ID with caching.
    /// </summary>
    public async Task<AppsTokenInfoDto> AppsTokenInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching apps token info. ID: {AppsTokenInfoId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"AppsTokenInfo:{id}",
            factory: async () =>
            {
                var appsTokenInfo = await _repository.AppsTokenInfos.AppsTokenInfoAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AppsTokenInfo", "AppsTokenInfoId", id.ToString());

                _logger.LogInformation("Apps token info fetched successfully. ID: {AppsTokenInfoId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return appsTokenInfo.MapTo<AppsTokenInfoDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AppsTokenInfo", "AppsTokenInfoId", id.ToString());
    }

    /// <summary>
    /// Retrieves all apps token info records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<AppsTokenInfoDto>> AppsTokenInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all apps token infos. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "AppsTokenInfo:All",
            factory: async () =>
            {
                var appsTokenInfos = await _repository.AppsTokenInfos.AppsTokenInfosAsync(trackChanges, cancellationToken);

                if (!appsTokenInfos.Any())
                {
                    _logger.LogWarning("No apps token infos found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<AppsTokenInfoDto>();
                }

                var appsTokenInfosDto = appsTokenInfos.MapToList<AppsTokenInfoDto>();

                _logger.LogInformation("Apps token infos fetched successfully. Count: {Count}, Time: {Time}",
                    appsTokenInfosDto.Count(), DateTime.UtcNow);

                return appsTokenInfosDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AppsTokenInfoDto>();
    }

    /// <summary>
    /// Retrieves all apps token infos for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<AppsTokenInfoDDLDto>> AppsTokenInfosForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching apps token infos for dropdown list");

        return await _cache.OrSetAsync(
            key: "AppsTokenInfo:DDL",
            factory: async () =>
            {
                var appsTokenInfos = await _repository.AppsTokenInfos.AppsTokenInfosAsync(trackChanges, cancellationToken);

                if (!appsTokenInfos.Any())
                {
                    _logger.LogWarning("No apps token infos found for dropdown");
                    return Enumerable.Empty<AppsTokenInfoDDLDto>();
                }

                return appsTokenInfos.MapToList<AppsTokenInfoDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AppsTokenInfoDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all apps token infos.
    /// </summary>
    public async Task<GridEntity<AppsTokenInfoDto>> AppsTokenInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AppsTokenInfo";
        const string orderBy = "IssueDate DESC";

        _logger.LogInformation("Fetching apps token infos summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AppsTokenInfos.AdoGridDataAsync<AppsTokenInfoDto>(sql, options, orderBy, "", cancellationToken);
    }
}
