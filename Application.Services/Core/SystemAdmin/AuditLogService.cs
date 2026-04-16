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
/// Audit log service implementing business logic for audit log management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class AuditLogService : IAuditLogService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AuditLogService> _logger;
    private readonly IConfiguration _configuration;

    public AuditLogService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AuditLogService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new audit log record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AuditLogDto> CreateAsync(CreateAuditLogRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAuditLogRecord));

        // FluentValidation
        var validator = new CreateAuditLogRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new audit log. Action: {Action}, EntityType: {EntityType}, Time: {Time}",
            record.Action, record.EntityType, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        AuditLog auditLog = record.MapTo<AuditLog>();

        long auditId = await _repository.AuditLogs.CreateAndIdAsync(auditLog, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Audit log created successfully. ID: {AuditId}, Time: {Time}",
            auditId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AuditLog:All");
        await _cache.RemoveAsync("AuditLog:Active");

        // Return as DTO
        var resultDto = auditLog.MapTo<AuditLogDto>();
        resultDto.AuditId = auditId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing audit log record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AuditLogDto> UpdateAsync(UpdateAuditLogRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAuditLogRecord));

        // FluentValidation
        var validator = new UpdateAuditLogRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating audit log. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        // Check if audit log exists
        var existingAuditLog = await _repository.AuditLogs.AuditLogAsync(
            record.AuditId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AuditLog", "AuditId", record.AuditId.ToString());

        // Map Record to Entity using Mapster
        AuditLog auditLog = record.MapTo<AuditLog>();

        _repository.AuditLogs.UpdateByState(auditLog);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Audit log updated successfully. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AuditLog:All");
        await _cache.RemoveAsync("AuditLog:Active");
        await _cache.RemoveAsync($"AuditLog:{record.AuditId}");

        return auditLog.MapTo<AuditLogDto>();
    }

    /// <summary>
    /// Deletes an audit log record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteAuditLogRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AuditId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteAuditLogRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting audit log. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        var auditLogEntity = await _repository.AuditLogs.AuditLogAsync(
            record.AuditId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AuditLog", "AuditId", record.AuditId.ToString());

        await _repository.AuditLogs.DeleteAsync(a => a.AuditId == record.AuditId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Audit log deleted successfully. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AuditLog:All");
        await _cache.RemoveAsync("AuditLog:Active");
        await _cache.RemoveAsync($"AuditLog:{record.AuditId}");
    }

    /// <summary>
    /// Retrieves a single audit log record by its ID with caching.
    /// </summary>
    public async Task<AuditLogDto> AuditLogAsync(long id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching audit log. ID: {AuditId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"AuditLog:{id}",
            factory: async () =>
            {
                var auditLog = await _repository.AuditLogs.AuditLogAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AuditLog", "AuditId", id.ToString());

                _logger.LogInformation("Audit log fetched successfully. ID: {AuditId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return auditLog.MapTo<AuditLogDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AuditLog", "AuditId", id.ToString());
    }

    /// <summary>
    /// Retrieves all audit log records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<AuditLogDto>> AuditLogsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all audit logs. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "AuditLog:All",
            factory: async () =>
            {
                var auditLogs = await _repository.AuditLogs.AuditLogsAsync(trackChanges, cancellationToken);

                if (!auditLogs.Any())
                {
                    _logger.LogWarning("No audit logs found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<AuditLogDto>();
                }

                var auditLogsDto = auditLogs.MapToList<AuditLogDto>();

                _logger.LogInformation("Audit logs fetched successfully. Count: {Count}, Time: {Time}",
                    auditLogsDto.Count(), DateTime.UtcNow);

                return auditLogsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AuditLogDto>();
    }

    /// <summary>
    /// Retrieves all audit logs for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<AuditLogDDLDto>> AuditLogsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching audit logs for dropdown list");

        return await _cache.OrSetAsync(
            key: "AuditLog:DDL",
            factory: async () =>
            {
                var auditLogs = await _repository.AuditLogs.AuditLogsAsync(trackChanges, cancellationToken);

                if (!auditLogs.Any())
                {
                    _logger.LogWarning("No audit logs found for dropdown");
                    return Enumerable.Empty<AuditLogDDLDto>();
                }

                return auditLogs.MapToList<AuditLogDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AuditLogDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all audit logs.
    /// </summary>
    public async Task<GridEntity<AuditLogDto>> AuditLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AuditLogs";
        const string orderBy = "Timestamp DESC";

        _logger.LogInformation("Fetching audit logs summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AuditLogs.AdoGridDataAsync<AuditLogDto>(sql, options, orderBy, "", cancellationToken);
    }
}
