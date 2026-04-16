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
/// Audit trail service implementing business logic for audit trail management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class AuditTrailService : IAuditTrailService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AuditTrailService> _logger;
    private readonly IConfiguration _configuration;

    public AuditTrailService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AuditTrailService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new audit trail record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AuditTrailDto> CreateAsync(CreateAuditTrailRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAuditTrailRecord));

        // FluentValidation
        var validator = new CreateAuditTrailRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new audit trail. UserId: {UserId}, AuditType: {AuditType}, Time: {Time}",
            record.UserId, record.AuditType, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        AuditTrail auditTrail = record.MapTo<AuditTrail>();

        int auditId = await _repository.AuditTrails.CreateAndIdAsync(auditTrail, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Audit trail created successfully. ID: {AuditId}, Time: {Time}",
            auditId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AuditTrail:All");
        await _cache.RemoveAsync("AuditTrail:Active");

        // Return as DTO
        var resultDto = auditTrail.MapTo<AuditTrailDto>();
        resultDto.AuditId = auditId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing audit trail record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AuditTrailDto> UpdateAsync(UpdateAuditTrailRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAuditTrailRecord));

        // FluentValidation
        var validator = new UpdateAuditTrailRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating audit trail. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        // Check if audit trail exists
        var existingAuditTrail = await _repository.AuditTrails.AuditTrailAsync(
            record.AuditId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AuditTrail", "AuditId", record.AuditId.ToString());

        // Map Record to Entity using Mapster
        AuditTrail auditTrail = record.MapTo<AuditTrail>();

        _repository.AuditTrails.UpdateByState(auditTrail);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Audit trail updated successfully. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AuditTrail:All");
        await _cache.RemoveAsync("AuditTrail:Active");
        await _cache.RemoveAsync($"AuditTrail:{record.AuditId}");

        return auditTrail.MapTo<AuditTrailDto>();
    }

    /// <summary>
    /// Deletes an audit trail record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteAuditTrailRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AuditId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteAuditTrailRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting audit trail. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        var auditTrailEntity = await _repository.AuditTrails.AuditTrailAsync(
            record.AuditId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AuditTrail", "AuditId", record.AuditId.ToString());

        await _repository.AuditTrails.DeleteAsync(a => a.AuditId == record.AuditId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Audit trail deleted successfully. ID: {AuditId}, Time: {Time}",
            record.AuditId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AuditTrail:All");
        await _cache.RemoveAsync("AuditTrail:Active");
        await _cache.RemoveAsync($"AuditTrail:{record.AuditId}");
    }

    /// <summary>
    /// Retrieves a single audit trail record by its ID with caching.
    /// </summary>
    public async Task<AuditTrailDto> AuditTrailAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching audit trail. ID: {AuditId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"AuditTrail:{id}",
            factory: async () =>
            {
                var auditTrail = await _repository.AuditTrails.AuditTrailAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AuditTrail", "AuditId", id.ToString());

                _logger.LogInformation("Audit trail fetched successfully. ID: {AuditId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return auditTrail.MapTo<AuditTrailDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AuditTrail", "AuditId", id.ToString());
    }

    /// <summary>
    /// Retrieves all audit trail records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<AuditTrailDto>> AuditTrailsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all audit trails. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "AuditTrail:All",
            factory: async () =>
            {
                var auditTrails = await _repository.AuditTrails.AuditTrailsAsync(trackChanges, cancellationToken);

                if (!auditTrails.Any())
                {
                    _logger.LogWarning("No audit trails found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<AuditTrailDto>();
                }

                var auditTrailsDto = auditTrails.MapToList<AuditTrailDto>();

                _logger.LogInformation("Audit trails fetched successfully. Count: {Count}, Time: {Time}",
                    auditTrailsDto.Count(), DateTime.UtcNow);

                return auditTrailsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AuditTrailDto>();
    }

    /// <summary>
    /// Retrieves all audit trails for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<AuditTrailDDLDto>> AuditTrailsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching audit trails for dropdown list");

        return await _cache.OrSetAsync(
            key: "AuditTrail:DDL",
            factory: async () =>
            {
                var auditTrails = await _repository.AuditTrails.AuditTrailsAsync(trackChanges, cancellationToken);

                if (!auditTrails.Any())
                {
                    _logger.LogWarning("No audit trails found for dropdown");
                    return Enumerable.Empty<AuditTrailDDLDto>();
                }

                return auditTrails.MapToList<AuditTrailDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AuditTrailDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all audit trails.
    /// </summary>
    public async Task<GridEntity<AuditTrailDto>> AuditTrailsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AuditTrail";
        const string orderBy = "ActionDate DESC";

        _logger.LogInformation("Fetching audit trails summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AuditTrails.AdoGridDataAsync<AuditTrailDto>(sql, options, orderBy, "", cancellationToken);
    }
}
