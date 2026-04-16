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
/// ApproverTypeToGroupMapping service implementing business logic for approver type to group mapping management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class ApproverTypeToGroupMappingService : IApproverTypeToGroupMappingService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<ApproverTypeToGroupMappingService> _logger;
    private readonly IConfiguration _configuration;

    public ApproverTypeToGroupMappingService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<ApproverTypeToGroupMappingService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new approver type to group mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverTypeToGroupMappingDto> CreateAsync(CreateApproverTypeToGroupMappingRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateApproverTypeToGroupMappingRecord));

        // FluentValidation
        var validator = new CreateApproverTypeToGroupMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new approver type to group mapping. Time: {Time}",
            DateTime.UtcNow);

        // Map Record to Entity using Mapster
        ApproverTypeToGroupMapping approverTypeToGroupMapping = record.MapTo<ApproverTypeToGroupMapping>();
        approverTypeToGroupMapping.LastUpdatedDate = DateTime.UtcNow;

        int approverTypeMapId = await _repository.ApproverTypeToGroupMappings.CreateAndIdAsync(approverTypeToGroupMapping, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverTypeToGroupMapping created successfully. ID: {ApproverTypeMapId}, Time: {Time}",
            approverTypeMapId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverTypeToGroupMapping:All");
        await _cache.RemoveAsync("ApproverTypeToGroupMapping:Active");

        // Return as DTO
        var resultDto = approverTypeToGroupMapping.MapTo<ApproverTypeToGroupMappingDto>();
        resultDto.ApproverTypeMapId = approverTypeMapId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing approver type to group mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverTypeToGroupMappingDto> UpdateAsync(UpdateApproverTypeToGroupMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateApproverTypeToGroupMappingRecord));

        // FluentValidation
        var validator = new UpdateApproverTypeToGroupMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating approver type to group mapping. ID: {ApproverTypeMapId}, Time: {Time}",
            record.ApproverTypeMapId, DateTime.UtcNow);

        // Check if approver type to group mapping exists
        var existingApproverTypeToGroupMapping = await _repository.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingAsync(
            record.ApproverTypeMapId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("ApproverTypeToGroupMapping", "ApproverTypeMapId", record.ApproverTypeMapId.ToString());

        // Map Record to Entity using Mapster
        ApproverTypeToGroupMapping approverTypeToGroupMapping = record.MapTo<ApproverTypeToGroupMapping>();
        approverTypeToGroupMapping.LastUpdatedDate = DateTime.UtcNow;

        _repository.ApproverTypeToGroupMappings.UpdateByState(approverTypeToGroupMapping);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverTypeToGroupMapping updated successfully. ID: {ApproverTypeMapId}, Time: {Time}",
            record.ApproverTypeMapId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverTypeToGroupMapping:All");
        await _cache.RemoveAsync("ApproverTypeToGroupMapping:Active");
        await _cache.RemoveAsync($"ApproverTypeToGroupMapping:{record.ApproverTypeMapId}");

        return approverTypeToGroupMapping.MapTo<ApproverTypeToGroupMappingDto>();
    }

    /// <summary>
    /// Deletes an approver type to group mapping record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteApproverTypeToGroupMappingRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ApproverTypeMapId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteApproverTypeToGroupMappingRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting approver type to group mapping. ID: {ApproverTypeMapId}, Time: {Time}",
            record.ApproverTypeMapId, DateTime.UtcNow);

        var approverTypeToGroupMappingEntity = await _repository.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingAsync(
            record.ApproverTypeMapId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("ApproverTypeToGroupMapping", "ApproverTypeMapId", record.ApproverTypeMapId.ToString());

        await _repository.ApproverTypeToGroupMappings.DeleteAsync(a => a.ApproverTypeMapId == record.ApproverTypeMapId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverTypeToGroupMapping deleted successfully. ID: {ApproverTypeMapId}, Time: {Time}",
            record.ApproverTypeMapId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverTypeToGroupMapping:All");
        await _cache.RemoveAsync("ApproverTypeToGroupMapping:Active");
        await _cache.RemoveAsync($"ApproverTypeToGroupMapping:{record.ApproverTypeMapId}");
    }

    /// <summary>
    /// Retrieves a single approver type to group mapping record by its ID with caching.
    /// </summary>
    public async Task<ApproverTypeToGroupMappingDto> ApproverTypeToGroupMappingAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver type to group mapping. ID: {ApproverTypeMapId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"ApproverTypeToGroupMapping:{id}",
            factory: async () =>
            {
                var approverTypeToGroupMapping = await _repository.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("ApproverTypeToGroupMapping", "ApproverTypeMapId", id.ToString());

                _logger.LogInformation("ApproverTypeToGroupMapping fetched successfully. ID: {ApproverTypeMapId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return approverTypeToGroupMapping.MapTo<ApproverTypeToGroupMappingDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("ApproverTypeToGroupMapping", "ApproverTypeMapId", id.ToString());
    }

    /// <summary>
    /// Retrieves all approver type to group mapping records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverTypeToGroupMappingDto>> ApproverTypeToGroupMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all approver type to group mappings. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "ApproverTypeToGroupMapping:All",
            factory: async () =>
            {
                var approverTypeToGroupMappings = await _repository.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingsAsync(trackChanges, cancellationToken);

                if (!approverTypeToGroupMappings.Any())
                {
                    _logger.LogWarning("No approver type to group mappings found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<ApproverTypeToGroupMappingDto>();
                }

                var approverTypeToGroupMappingsDto = approverTypeToGroupMappings.MapToList<ApproverTypeToGroupMappingDto>();

                _logger.LogInformation("ApproverTypeToGroupMappings fetched successfully. Count: {Count}, Time: {Time}",
                    approverTypeToGroupMappingsDto.Count(), DateTime.UtcNow);

                return approverTypeToGroupMappingsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverTypeToGroupMappingDto>();
    }

    /// <summary>
    /// Retrieves all approver type to group mappings for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverTypeToGroupMappingDDLDto>> ApproverTypeToGroupMappingsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver type to group mappings for dropdown list");

        return await _cache.OrSetAsync(
            key: "ApproverTypeToGroupMapping:DDL",
            factory: async () =>
            {
                var approverTypeToGroupMappings = await _repository.ApproverTypeToGroupMappings.ApproverTypeToGroupMappingsAsync(trackChanges, cancellationToken);

                if (!approverTypeToGroupMappings.Any())
                {
                    _logger.LogWarning("No approver type to group mappings found for dropdown");
                    return Enumerable.Empty<ApproverTypeToGroupMappingDDLDto>();
                }

                return approverTypeToGroupMappings.MapToList<ApproverTypeToGroupMappingDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverTypeToGroupMappingDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all approver type to group mappings.
    /// </summary>
    public async Task<GridEntity<ApproverTypeToGroupMappingDto>> ApproverTypeToGroupMappingsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM ApproverTypeToGroupMapping";
        const string orderBy = "ApproverTypeMapId DESC";

        _logger.LogInformation("Fetching approver type to group mappings summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.ApproverTypeToGroupMappings.AdoGridDataAsync<ApproverTypeToGroupMappingDto>(sql, options, orderBy, "", cancellationToken);
    }
}
