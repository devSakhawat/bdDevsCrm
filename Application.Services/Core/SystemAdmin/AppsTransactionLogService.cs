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
/// Apps transaction log service implementing business logic for apps transaction log management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class AppsTransactionLogService : IAppsTransactionLogService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AppsTransactionLogService> _logger;
    private readonly IConfiguration _configuration;

    public AppsTransactionLogService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AppsTransactionLogService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new apps transaction log record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AppsTransactionLogDto> CreateAsync(CreateAppsTransactionLogRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAppsTransactionLogRecord));

        // FluentValidation
        var validator = new CreateAppsTransactionLogRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new apps transaction log. TransactionType: {TransactionType}, AppsUserId: {AppsUserId}, Time: {Time}",
            record.TransactionType, record.AppsUserId, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        AppsTransactionLog appsTransactionLog = record.MapTo<AppsTransactionLog>();

        int transactionLogId = await _repository.AppsTransactionLogs.CreateAndIdAsync(appsTransactionLog, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Apps transaction log created successfully. ID: {TransactionLogId}, Time: {Time}",
            transactionLogId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AppsTransactionLog:All");
        await _cache.RemoveAsync("AppsTransactionLog:Active");

        // Return as DTO
        var resultDto = appsTransactionLog.MapTo<AppsTransactionLogDto>();
        resultDto.TransactionLogId = transactionLogId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing apps transaction log record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AppsTransactionLogDto> UpdateAsync(UpdateAppsTransactionLogRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAppsTransactionLogRecord));

        // FluentValidation
        var validator = new UpdateAppsTransactionLogRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating apps transaction log. ID: {TransactionLogId}, Time: {Time}",
            record.TransactionLogId, DateTime.UtcNow);

        // Check if apps transaction log exists
        var existingAppsTransactionLog = await _repository.AppsTransactionLogs.AppsTransactionLogAsync(
            record.TransactionLogId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AppsTransactionLog", "TransactionLogId", record.TransactionLogId.ToString());

        // Map Record to Entity using Mapster
        AppsTransactionLog appsTransactionLog = record.MapTo<AppsTransactionLog>();

        _repository.AppsTransactionLogs.UpdateByState(appsTransactionLog);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Apps transaction log updated successfully. ID: {TransactionLogId}, Time: {Time}",
            record.TransactionLogId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AppsTransactionLog:All");
        await _cache.RemoveAsync("AppsTransactionLog:Active");
        await _cache.RemoveAsync($"AppsTransactionLog:{record.TransactionLogId}");

        return appsTransactionLog.MapTo<AppsTransactionLogDto>();
    }

    /// <summary>
    /// Deletes an apps transaction log record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteAppsTransactionLogRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.TransactionLogId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteAppsTransactionLogRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting apps transaction log. ID: {TransactionLogId}, Time: {Time}",
            record.TransactionLogId, DateTime.UtcNow);

        var appsTransactionLogEntity = await _repository.AppsTransactionLogs.AppsTransactionLogAsync(
            record.TransactionLogId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AppsTransactionLog", "TransactionLogId", record.TransactionLogId.ToString());

        await _repository.AppsTransactionLogs.DeleteAsync(a => a.TransactionLogId == record.TransactionLogId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Apps transaction log deleted successfully. ID: {TransactionLogId}, Time: {Time}",
            record.TransactionLogId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AppsTransactionLog:All");
        await _cache.RemoveAsync("AppsTransactionLog:Active");
        await _cache.RemoveAsync($"AppsTransactionLog:{record.TransactionLogId}");
    }

    /// <summary>
    /// Retrieves a single apps transaction log record by its ID with caching.
    /// </summary>
    public async Task<AppsTransactionLogDto> AppsTransactionLogAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching apps transaction log. ID: {TransactionLogId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"AppsTransactionLog:{id}",
            factory: async () =>
            {
                var appsTransactionLog = await _repository.AppsTransactionLogs.AppsTransactionLogAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AppsTransactionLog", "TransactionLogId", id.ToString());

                _logger.LogInformation("Apps transaction log fetched successfully. ID: {TransactionLogId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return appsTransactionLog.MapTo<AppsTransactionLogDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AppsTransactionLog", "TransactionLogId", id.ToString());
    }

    /// <summary>
    /// Retrieves all apps transaction log records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<AppsTransactionLogDto>> AppsTransactionLogsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all apps transaction logs. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "AppsTransactionLog:All",
            factory: async () =>
            {
                var appsTransactionLogs = await _repository.AppsTransactionLogs.AppsTransactionLogsAsync(trackChanges, cancellationToken);

                if (!appsTransactionLogs.Any())
                {
                    _logger.LogWarning("No apps transaction logs found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<AppsTransactionLogDto>();
                }

                var appsTransactionLogsDto = appsTransactionLogs.MapToList<AppsTransactionLogDto>();

                _logger.LogInformation("Apps transaction logs fetched successfully. Count: {Count}, Time: {Time}",
                    appsTransactionLogsDto.Count(), DateTime.UtcNow);

                return appsTransactionLogsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AppsTransactionLogDto>();
    }

    /// <summary>
    /// Retrieves all apps transaction logs for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<AppsTransactionLogDDLDto>> AppsTransactionLogsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching apps transaction logs for dropdown list");

        return await _cache.OrSetAsync(
            key: "AppsTransactionLog:DDL",
            factory: async () =>
            {
                var appsTransactionLogs = await _repository.AppsTransactionLogs.AppsTransactionLogsAsync(trackChanges, cancellationToken);

                if (!appsTransactionLogs.Any())
                {
                    _logger.LogWarning("No apps transaction logs found for dropdown");
                    return Enumerable.Empty<AppsTransactionLogDDLDto>();
                }

                return appsTransactionLogs.MapToList<AppsTransactionLogDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AppsTransactionLogDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all apps transaction logs.
    /// </summary>
    public async Task<GridEntity<AppsTransactionLogDto>> AppsTransactionLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AppsTransactionLog";
        const string orderBy = "TransactionDate DESC";

        _logger.LogInformation("Fetching apps transaction logs summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AppsTransactionLogs.AdoGridDataAsync<AppsTransactionLogDto>(sql, options, orderBy, "", cancellationToken);
    }
}
