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
/// ApproverType service implementing business logic for approver type management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class ApproverTypeService : IApproverTypeService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<ApproverTypeService> _logger;
    private readonly IConfiguration _configuration;

    public ApproverTypeService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<ApproverTypeService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new approver type record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverTypeDto> CreateAsync(CreateApproverTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateApproverTypeRecord));

        // FluentValidation
        var validator = new CreateApproverTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new approver type. Time: {Time}",
            DateTime.UtcNow);

        // Map Record to Entity using Mapster
        ApproverType approverType = record.MapTo<ApproverType>();
        approverType.LastUpdatedDate = DateTime.UtcNow;

        int approverTypeId = await _repository.ApproverTypes.CreateAndIdAsync(approverType, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverType created successfully. ID: {ApproverTypeId}, Time: {Time}",
            approverTypeId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverType:All");
        await _cache.RemoveAsync("ApproverType:Active");

        // Return as DTO
        var resultDto = approverType.MapTo<ApproverTypeDto>();
        resultDto.ApproverTypeId = approverTypeId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing approver type record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverTypeDto> UpdateAsync(UpdateApproverTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateApproverTypeRecord));

        // FluentValidation
        var validator = new UpdateApproverTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating approver type. ID: {ApproverTypeId}, Time: {Time}",
            record.ApproverTypeId, DateTime.UtcNow);

        // Check if approver type exists
        var existingApproverType = await _repository.ApproverTypes.ApproverTypeAsync(
            record.ApproverTypeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("ApproverType", "ApproverTypeId", record.ApproverTypeId.ToString());

        // Map Record to Entity using Mapster
        ApproverType approverType = record.MapTo<ApproverType>();
        approverType.LastUpdatedDate = DateTime.UtcNow;

        _repository.ApproverTypes.UpdateByState(approverType);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverType updated successfully. ID: {ApproverTypeId}, Time: {Time}",
            record.ApproverTypeId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverType:All");
        await _cache.RemoveAsync("ApproverType:Active");
        await _cache.RemoveAsync($"ApproverType:{record.ApproverTypeId}");

        return approverType.MapTo<ApproverTypeDto>();
    }

    /// <summary>
    /// Deletes an approver type record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteApproverTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ApproverTypeId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteApproverTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting approver type. ID: {ApproverTypeId}, Time: {Time}",
            record.ApproverTypeId, DateTime.UtcNow);

        var approverTypeEntity = await _repository.ApproverTypes.ApproverTypeAsync(
            record.ApproverTypeId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("ApproverType", "ApproverTypeId", record.ApproverTypeId.ToString());

        await _repository.ApproverTypes.DeleteAsync(a => a.ApproverTypeId == record.ApproverTypeId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverType deleted successfully. ID: {ApproverTypeId}, Time: {Time}",
            record.ApproverTypeId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverType:All");
        await _cache.RemoveAsync("ApproverType:Active");
        await _cache.RemoveAsync($"ApproverType:{record.ApproverTypeId}");
    }

    /// <summary>
    /// Retrieves a single approver type record by its ID with caching.
    /// </summary>
    public async Task<ApproverTypeDto> ApproverTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver type. ID: {ApproverTypeId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"ApproverType:{id}",
            factory: async () =>
            {
                var approverType = await _repository.ApproverTypes.ApproverTypeAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("ApproverType", "ApproverTypeId", id.ToString());

                _logger.LogInformation("ApproverType fetched successfully. ID: {ApproverTypeId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return approverType.MapTo<ApproverTypeDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("ApproverType", "ApproverTypeId", id.ToString());
    }

    /// <summary>
    /// Retrieves all approver type records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverTypeDto>> ApproverTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all approver types. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "ApproverType:All",
            factory: async () =>
            {
                var approverTypes = await _repository.ApproverTypes.ApproverTypesAsync(trackChanges, cancellationToken);

                if (!approverTypes.Any())
                {
                    _logger.LogWarning("No approver types found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<ApproverTypeDto>();
                }

                var approverTypesDto = approverTypes.MapToList<ApproverTypeDto>();

                _logger.LogInformation("ApproverTypes fetched successfully. Count: {Count}, Time: {Time}",
                    approverTypesDto.Count(), DateTime.UtcNow);

                return approverTypesDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverTypeDto>();
    }

    /// <summary>
    /// Retrieves all approver types for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverTypeDDLDto>> ApproverTypesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver types for dropdown list");

        return await _cache.OrSetAsync(
            key: "ApproverType:DDL",
            factory: async () =>
            {
                var approverTypes = await _repository.ApproverTypes.ApproverTypesAsync(trackChanges, cancellationToken);

                if (!approverTypes.Any())
                {
                    _logger.LogWarning("No approver types found for dropdown");
                    return Enumerable.Empty<ApproverTypeDDLDto>();
                }

                return approverTypes.MapToList<ApproverTypeDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverTypeDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all approver types.
    /// </summary>
    public async Task<GridEntity<ApproverTypeDto>> ApproverTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM ApproverType";
        const string orderBy = "ApproverTypeId DESC";

        _logger.LogInformation("Fetching approver types summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.ApproverTypes.AdoGridDataAsync<ApproverTypeDto>(sql, options, orderBy, "", cancellationToken);
    }
}
