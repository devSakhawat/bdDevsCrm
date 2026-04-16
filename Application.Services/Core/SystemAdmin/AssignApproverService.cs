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
/// AssignApprover service implementing business logic for assign approver management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class AssignApproverService : IAssignApproverService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AssignApproverService> _logger;
    private readonly IConfiguration _configuration;

    public AssignApproverService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AssignApproverService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new assign approver record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AssignApproverDto> CreateAsync(CreateAssignApproverRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAssignApproverRecord));

        // FluentValidation
        var validator = new CreateAssignApproverRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new assign approver. Time: {Time}",
            DateTime.UtcNow);

        // Map Record to Entity using Mapster
        AssignApprover assignApprover = record.MapTo<AssignApprover>();
        assignApprover.LastUpdatedDate = DateTime.UtcNow;

        int assignApproverId = await _repository.AssignApprovers.CreateAndIdAsync(assignApprover, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("AssignApprover created successfully. ID: {AssignApproverId}, Time: {Time}",
            assignApproverId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AssignApprover:All");
        await _cache.RemoveAsync("AssignApprover:Active");

        // Return as DTO
        var resultDto = assignApprover.MapTo<AssignApproverDto>();
        resultDto.AssignApproverId = assignApproverId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing assign approver record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<AssignApproverDto> UpdateAsync(UpdateAssignApproverRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAssignApproverRecord));

        // FluentValidation
        var validator = new UpdateAssignApproverRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating assign approver. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        // Check if assign approver exists
        var existingAssignApprover = await _repository.AssignApprovers.AssignApproverAsync(
            record.AssignApproverId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AssignApprover", "AssignApproverId", record.AssignApproverId.ToString());

        // Map Record to Entity using Mapster
        AssignApprover assignApprover = record.MapTo<AssignApprover>();
        assignApprover.LastUpdatedDate = DateTime.UtcNow;

        _repository.AssignApprovers.UpdateByState(assignApprover);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("AssignApprover updated successfully. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AssignApprover:All");
        await _cache.RemoveAsync("AssignApprover:Active");
        await _cache.RemoveAsync($"AssignApprover:{record.AssignApproverId}");

        return assignApprover.MapTo<AssignApproverDto>();
    }

    /// <summary>
    /// Deletes an assign approver record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteAssignApproverRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AssignApproverId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteAssignApproverRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting assign approver. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        var assignApproverEntity = await _repository.AssignApprovers.AssignApproverAsync(
            record.AssignApproverId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AssignApprover", "AssignApproverId", record.AssignApproverId.ToString());

        await _repository.AssignApprovers.DeleteAsync(a => a.AssignApproverId == record.AssignApproverId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("AssignApprover deleted successfully. ID: {AssignApproverId}, Time: {Time}",
            record.AssignApproverId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("AssignApprover:All");
        await _cache.RemoveAsync("AssignApprover:Active");
        await _cache.RemoveAsync($"AssignApprover:{record.AssignApproverId}");
    }

    /// <summary>
    /// Retrieves a single assign approver record by its ID with caching.
    /// </summary>
    public async Task<AssignApproverDto> AssignApproverAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching assign approver. ID: {AssignApproverId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"AssignApprover:{id}",
            factory: async () =>
            {
                var assignApprover = await _repository.AssignApprovers.AssignApproverAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AssignApprover", "AssignApproverId", id.ToString());

                _logger.LogInformation("AssignApprover fetched successfully. ID: {AssignApproverId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return assignApprover.MapTo<AssignApproverDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AssignApprover", "AssignApproverId", id.ToString());
    }

    /// <summary>
    /// Retrieves all assign approver records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<AssignApproverDto>> AssignApproversAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all assign approvers. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "AssignApprover:All",
            factory: async () =>
            {
                var assignApprovers = await _repository.AssignApprovers.AssignApproversAsync(trackChanges, cancellationToken);

                if (!assignApprovers.Any())
                {
                    _logger.LogWarning("No assign approvers found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<AssignApproverDto>();
                }

                var assignApproversDto = assignApprovers.MapToList<AssignApproverDto>();

                _logger.LogInformation("AssignApprovers fetched successfully. Count: {Count}, Time: {Time}",
                    assignApproversDto.Count(), DateTime.UtcNow);

                return assignApproversDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AssignApproverDto>();
    }

    /// <summary>
    /// Retrieves all assign approvers for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<AssignApproverDDLDto>> AssignApproversForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching assign approvers for dropdown list");

        return await _cache.OrSetAsync(
            key: "AssignApprover:DDL",
            factory: async () =>
            {
                var assignApprovers = await _repository.AssignApprovers.ActiveAssignApproversAsync(trackChanges, cancellationToken);

                if (!assignApprovers.Any())
                {
                    _logger.LogWarning("No assign approvers found for dropdown");
                    return Enumerable.Empty<AssignApproverDDLDto>();
                }

                return assignApprovers.MapToList<AssignApproverDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AssignApproverDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all assign approvers.
    /// </summary>
    public async Task<GridEntity<AssignApproverDto>> AssignApproversSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AssignApprover";
        const string orderBy = "AssignApproverId DESC";

        _logger.LogInformation("Fetching assign approvers summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AssignApprovers.AdoGridDataAsync<AssignApproverDto>(sql, options, orderBy, "", cancellationToken);
    }
}
