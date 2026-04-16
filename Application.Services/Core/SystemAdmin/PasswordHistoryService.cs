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
/// Password history service implementing business logic for password history management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class PasswordHistoryService : IPasswordHistoryService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<PasswordHistoryService> _logger;
    private readonly IConfiguration _configuration;

    public PasswordHistoryService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<PasswordHistoryService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new password history record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<PasswordHistoryDto> CreateAsync(CreatePasswordHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreatePasswordHistoryRecord));

        // FluentValidation
        var validator = new CreatePasswordHistoryRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new password history. UserId: {UserId}, PasswordChangeDate: {PasswordChangeDate}, Time: {Time}",
            record.UserId, record.PasswordChangeDate, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        PasswordHistory passwordHistory = record.MapTo<PasswordHistory>();

        int historyId = await _repository.PasswordHistories.CreateAndIdAsync(passwordHistory, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Password history created successfully. ID: {HistoryId}, Time: {Time}",
            historyId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("PasswordHistory:All");
        await _cache.RemoveAsync("PasswordHistory:Active");

        // Return as DTO
        var resultDto = passwordHistory.MapTo<PasswordHistoryDto>();
        resultDto.HistoryId = historyId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing password history record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<PasswordHistoryDto> UpdateAsync(UpdatePasswordHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdatePasswordHistoryRecord));

        // FluentValidation
        var validator = new UpdatePasswordHistoryRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating password history. ID: {HistoryId}, Time: {Time}",
            record.HistoryId, DateTime.UtcNow);

        // Check if password history exists
        var existingPasswordHistory = await _repository.PasswordHistories.PasswordHistoryAsync(
            record.HistoryId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("PasswordHistory", "HistoryId", record.HistoryId.ToString());

        // Map Record to Entity using Mapster
        PasswordHistory passwordHistory = record.MapTo<PasswordHistory>();

        _repository.PasswordHistories.UpdateByState(passwordHistory);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Password history updated successfully. ID: {HistoryId}, Time: {Time}",
            record.HistoryId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("PasswordHistory:All");
        await _cache.RemoveAsync("PasswordHistory:Active");
        await _cache.RemoveAsync($"PasswordHistory:{record.HistoryId}");

        return passwordHistory.MapTo<PasswordHistoryDto>();
    }

    /// <summary>
    /// Deletes a password history record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeletePasswordHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.HistoryId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeletePasswordHistoryRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting password history. ID: {HistoryId}, Time: {Time}",
            record.HistoryId, DateTime.UtcNow);

        var passwordHistoryEntity = await _repository.PasswordHistories.PasswordHistoryAsync(
            record.HistoryId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("PasswordHistory", "HistoryId", record.HistoryId.ToString());

        await _repository.PasswordHistories.DeleteAsync(p => p.HistoryId == record.HistoryId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Password history deleted successfully. ID: {HistoryId}, Time: {Time}",
            record.HistoryId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("PasswordHistory:All");
        await _cache.RemoveAsync("PasswordHistory:Active");
        await _cache.RemoveAsync($"PasswordHistory:{record.HistoryId}");
    }

    /// <summary>
    /// Retrieves a single password history record by its ID with caching.
    /// </summary>
    public async Task<PasswordHistoryDto> PasswordHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching password history. ID: {HistoryId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"PasswordHistory:{id}",
            factory: async () =>
            {
                var passwordHistory = await _repository.PasswordHistories.PasswordHistoryAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("PasswordHistory", "HistoryId", id.ToString());

                _logger.LogInformation("Password history fetched successfully. ID: {HistoryId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return passwordHistory.MapTo<PasswordHistoryDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("PasswordHistory", "HistoryId", id.ToString());
    }

    /// <summary>
    /// Retrieves all password history records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<PasswordHistoryDto>> PasswordHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all password histories. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "PasswordHistory:All",
            factory: async () =>
            {
                var passwordHistories = await _repository.PasswordHistories.PasswordHistoriesAsync(trackChanges, cancellationToken);

                if (!passwordHistories.Any())
                {
                    _logger.LogWarning("No password histories found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<PasswordHistoryDto>();
                }

                var passwordHistoriesDto = passwordHistories.MapToList<PasswordHistoryDto>();

                _logger.LogInformation("Password histories fetched successfully. Count: {Count}, Time: {Time}",
                    passwordHistoriesDto.Count(), DateTime.UtcNow);

                return passwordHistoriesDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<PasswordHistoryDto>();
    }

    /// <summary>
    /// Retrieves all password histories for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<PasswordHistoryDDLDto>> PasswordHistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching password histories for dropdown list");

        return await _cache.OrSetAsync(
            key: "PasswordHistory:DDL",
            factory: async () =>
            {
                var passwordHistories = await _repository.PasswordHistories.PasswordHistoriesAsync(trackChanges, cancellationToken);

                if (!passwordHistories.Any())
                {
                    _logger.LogWarning("No password histories found for dropdown");
                    return Enumerable.Empty<PasswordHistoryDDLDto>();
                }

                return passwordHistories.MapToList<PasswordHistoryDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<PasswordHistoryDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all password histories.
    /// </summary>
    public async Task<GridEntity<PasswordHistoryDto>> PasswordHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM PasswordHistory";
        const string orderBy = "PasswordChangeDate DESC";

        _logger.LogInformation("Fetching password histories summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.PasswordHistories.AdoGridDataAsync<PasswordHistoryDto>(sql, options, orderBy, "", cancellationToken);
    }
}
