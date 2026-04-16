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
/// Access restriction service implementing business logic for access restriction management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class AccessRestrictionService : IAccessRestrictionService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AccessRestrictionService> _logger;
    private readonly IConfiguration _configuration;

    public AccessRestrictionService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AccessRestrictionService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new access restriction record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AccessRestrictionDto> CreateAsync(CreateAccessRestrictionRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAccessRestrictionRecord));

        // FluentValidation
        var validator = new CreateAccessRestrictionRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new access restriction. HrRecordId: {HrRecordId}, ReferenceType: {ReferenceType}, Time: {Time}",
            record.HrRecordId, record.ReferenceType, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        AccessRestriction accessRestriction = record.MapTo<AccessRestriction>();

        int accessRestrictionId = await _repository.AccessRestrictions.CreateAndIdAsync(accessRestriction, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Access restriction created successfully. ID: {AccessRestrictionId}, Time: {Time}",
            accessRestrictionId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AccessRestriction:All");
        await _cache.RemoveAsync("AccessRestriction:Active");

        // Return as DTO
        var resultDto = accessRestriction.MapTo<AccessRestrictionDto>();
        resultDto.AccessRestrictionId = accessRestrictionId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing access restriction record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AccessRestrictionDto> UpdateAsync(UpdateAccessRestrictionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAccessRestrictionRecord));

        // FluentValidation
        var validator = new UpdateAccessRestrictionRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating access restriction. ID: {AccessRestrictionId}, Time: {Time}",
            record.AccessRestrictionId, DateTime.UtcNow);

        // Check if access restriction exists
        var existingAccessRestriction = await _repository.AccessRestrictions.AccessRestrictionAsync(
            record.AccessRestrictionId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AccessRestriction", "AccessRestrictionId", record.AccessRestrictionId.ToString());

        // Map Record to Entity using Mapster
        AccessRestriction accessRestriction = record.MapTo<AccessRestriction>();

        _repository.AccessRestrictions.UpdateByState(accessRestriction);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Access restriction updated successfully. ID: {AccessRestrictionId}, Time: {Time}",
            record.AccessRestrictionId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AccessRestriction:All");
        await _cache.RemoveAsync("AccessRestriction:Active");
        await _cache.RemoveAsync($"AccessRestriction:{record.AccessRestrictionId}");

        return accessRestriction.MapTo<AccessRestrictionDto>();
    }

    /// <summary>
    /// Deletes an access restriction record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteAccessRestrictionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AccessRestrictionId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteAccessRestrictionRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting access restriction. ID: {AccessRestrictionId}, Time: {Time}",
            record.AccessRestrictionId, DateTime.UtcNow);

        var accessRestrictionEntity = await _repository.AccessRestrictions.AccessRestrictionAsync(
            record.AccessRestrictionId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AccessRestriction", "AccessRestrictionId", record.AccessRestrictionId.ToString());

        await _repository.AccessRestrictions.DeleteAsync(a => a.AccessRestrictionId == record.AccessRestrictionId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Access restriction deleted successfully. ID: {AccessRestrictionId}, Time: {Time}",
            record.AccessRestrictionId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AccessRestriction:All");
        await _cache.RemoveAsync("AccessRestriction:Active");
        await _cache.RemoveAsync($"AccessRestriction:{record.AccessRestrictionId}");
    }

    /// <summary>
    /// Retrieves a single access restriction record by its ID with caching.
    /// </summary>
    public async Task<AccessRestrictionDto> AccessRestrictionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching access restriction. ID: {AccessRestrictionId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"AccessRestriction:{id}",
            factory: async () =>
            {
                var accessRestriction = await _repository.AccessRestrictions.AccessRestrictionAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AccessRestriction", "AccessRestrictionId", id.ToString());

                _logger.LogInformation("Access restriction fetched successfully. ID: {AccessRestrictionId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return accessRestriction.MapTo<AccessRestrictionDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AccessRestriction", "AccessRestrictionId", id.ToString());
    }

    /// <summary>
    /// Retrieves all access restriction records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<AccessRestrictionDto>> AccessRestrictionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all access restrictions. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "AccessRestriction:All",
            factory: async () =>
            {
                var accessRestrictions = await _repository.AccessRestrictions.AccessRestrictionsAsync(trackChanges, cancellationToken);

                if (!accessRestrictions.Any())
                {
                    _logger.LogWarning("No access restrictions found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<AccessRestrictionDto>();
                }

                var accessRestrictionsDto = accessRestrictions.MapToList<AccessRestrictionDto>();

                _logger.LogInformation("Access restrictions fetched successfully. Count: {Count}, Time: {Time}",
                    accessRestrictionsDto.Count(), DateTime.UtcNow);

                return accessRestrictionsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AccessRestrictionDto>();
    }

    /// <summary>
    /// Retrieves all access restrictions for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<AccessRestrictionDDLDto>> AccessRestrictionsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching access restrictions for dropdown list");

        return await _cache.OrSetAsync(
            key: "AccessRestriction:DDL",
            factory: async () =>
            {
                var accessRestrictions = await _repository.AccessRestrictions.AccessRestrictionsAsync(trackChanges, cancellationToken);

                if (!accessRestrictions.Any())
                {
                    _logger.LogWarning("No access restrictions found for dropdown");
                    return Enumerable.Empty<AccessRestrictionDDLDto>();
                }

                return accessRestrictions.MapToList<AccessRestrictionDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AccessRestrictionDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all access restrictions.
    /// </summary>
    public async Task<GridEntity<AccessRestrictionDto>> AccessRestrictionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AccessRestriction";
        const string orderBy = "AccessDate DESC";

        _logger.LogInformation("Fetching access restrictions summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AccessRestrictions.AdoGridDataAsync<AccessRestrictionDto>(sql, options, orderBy, "", cancellationToken);
    }
}
