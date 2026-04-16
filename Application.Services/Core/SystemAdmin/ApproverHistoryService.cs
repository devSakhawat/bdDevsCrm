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
/// ApproverHistory service implementing business logic for approver history management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class ApproverHistoryService : IApproverHistoryService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<ApproverHistoryService> _logger;
    private readonly IConfiguration _configuration;

    public ApproverHistoryService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<ApproverHistoryService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new approver history record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverHistoryDto> CreateAsync(CreateApproverHistoryRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateApproverHistoryRecord));

        // FluentValidation
        var validator = new CreateApproverHistoryRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new approver history. Time: {Time}",
            DateTime.UtcNow);

        // Map Record to Entity using Mapster
        ApproverHistory approverHistory = record.MapTo<ApproverHistory>();
        approverHistory.LastUpdatedDate = DateTime.UtcNow;

        int assignApproverId = await _repository.ApproverHistories.CreateAndIdAsync(approverHistory, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverHistory created successfully. ID: {AssignApproverId}, Time: {Time}",
            assignApproverId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverHistory:All");
        await _cache.RemoveAsync("ApproverHistory:Active");

        // Return as DTO
        var resultDto = approverHistory.MapTo<ApproverHistoryDto>();
        resultDto.AssignApproverId = assignApproverId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing approver history record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverHistoryDto> UpdateAsync(UpdateApproverHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateApproverHistoryRecord));

        // FluentValidation
        var validator = new UpdateApproverHistoryRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating approver history. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        // Check if approver history exists
        var existingApproverHistory = await _repository.ApproverHistories.ApproverHistoryAsync(
            record.AssignApproverId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("ApproverHistory", "AssignApproverId", record.AssignApproverId.ToString());

        // Map Record to Entity using Mapster
        ApproverHistory approverHistory = record.MapTo<ApproverHistory>();
        approverHistory.LastUpdatedDate = DateTime.UtcNow;

        _repository.ApproverHistories.UpdateByState(approverHistory);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverHistory updated successfully. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverHistory:All");
        await _cache.RemoveAsync("ApproverHistory:Active");
        await _cache.RemoveAsync($"ApproverHistory:{record.AssignApproverId}");

        return approverHistory.MapTo<ApproverHistoryDto>();
    }

    /// <summary>
    /// Deletes an approver history record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteApproverHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AssignApproverId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteApproverHistoryRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting approver history. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        var approverHistoryEntity = await _repository.ApproverHistories.ApproverHistoryAsync(
            record.AssignApproverId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("ApproverHistory", "AssignApproverId", record.AssignApproverId.ToString());

        await _repository.ApproverHistories.DeleteAsync(a => a.AssignApproverId == record.AssignApproverId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverHistory deleted successfully. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverHistory:All");
        await _cache.RemoveAsync("ApproverHistory:Active");
        await _cache.RemoveAsync($"ApproverHistory:{record.AssignApproverId}");
    }

    /// <summary>
    /// Retrieves a single approver history record by its ID with caching.
    /// </summary>
    public async Task<ApproverHistoryDto> ApproverHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver history. ID: {AssignApproverId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"ApproverHistory:{id}",
            factory: async () =>
            {
                var approverHistory = await _repository.ApproverHistories.ApproverHistoryAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("ApproverHistory", "AssignApproverId", id.ToString());

                _logger.LogInformation("ApproverHistory fetched successfully. ID: {AssignApproverId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return approverHistory.MapTo<ApproverHistoryDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("ApproverHistory", "AssignApproverId", id.ToString());
    }

    /// <summary>
    /// Retrieves all approver history records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverHistoryDto>> ApproverHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all approver histories. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "ApproverHistory:All",
            factory: async () =>
            {
                var approverHistoryList = await _repository.ApproverHistories.ApproverHistoryListAsync(trackChanges, cancellationToken);

                if (!approverHistoryList.Any())
                {
                    _logger.LogWarning("No approver histories found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<ApproverHistoryDto>();
                }

                var approverHistoryDto = approverHistoryList.MapToList<ApproverHistoryDto>();

                _logger.LogInformation("ApproverHistories fetched successfully. Count: {Count}, Time: {Time}",
                    approverHistoryDto.Count(), DateTime.UtcNow);

                return approverHistoryDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverHistoryDto>();
    }

    /// <summary>
    /// Retrieves all approver histories for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverHistoryDDLDto>> ApproverHistoriesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver histories for dropdown list");

        return await _cache.OrSetAsync(
            key: "ApproverHistory:DDL",
            factory: async () =>
            {
                var approverHistoryList = await _repository.ApproverHistories.ActiveApproverHistoriesAsync(trackChanges, cancellationToken);

                if (!approverHistoryList.Any())
                {
                    _logger.LogWarning("No approver histories found for dropdown");
                    return Enumerable.Empty<ApproverHistoryDDLDto>();
                }

                return approverHistoryList.MapToList<ApproverHistoryDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverHistoryDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all approver histories.
    /// </summary>
    public async Task<GridEntity<ApproverHistoryDto>> ApproverHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM ApproverHistory";
        const string orderBy = "AssignApproverId DESC";

        _logger.LogInformation("Fetching approver histories summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.ApproverHistories.AdoGridDataAsync<ApproverHistoryDto>(sql, options, orderBy, "", cancellationToken);
    }
}
