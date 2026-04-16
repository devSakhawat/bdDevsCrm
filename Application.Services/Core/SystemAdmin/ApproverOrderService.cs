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
/// ApproverOrder service implementing business logic for approver order management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class ApproverOrderService : IApproverOrderService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<ApproverOrderService> _logger;
    private readonly IConfiguration _configuration;

    public ApproverOrderService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<ApproverOrderService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new approver order record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverOrderDto> CreateAsync(CreateApproverOrderRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateApproverOrderRecord));

        // FluentValidation
        var validator = new CreateApproverOrderRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new approver order. Time: {Time}",
            DateTime.UtcNow);

        // Map Record to Entity using Mapster
        ApproverOrder approverOrder = record.MapTo<ApproverOrder>();
        approverOrder.LastUpdatedDate = DateTime.UtcNow;

        int approverOrderId = await _repository.ApproverOrders.CreateAndIdAsync(approverOrder, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverOrder created successfully. ID: {ApproverOrderId}, Time: {Time}",
            approverOrderId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverOrder:All");
        await _cache.RemoveAsync("ApproverOrder:Active");

        // Return as DTO
        var resultDto = approverOrder.MapTo<ApproverOrderDto>();
        resultDto.ApproverOrderId = approverOrderId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing approver order record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverOrderDto> UpdateAsync(UpdateApproverOrderRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateApproverOrderRecord));

        // FluentValidation
        var validator = new UpdateApproverOrderRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating approver order. ID: {ApproverOrderId}, Time: {Time}",
            record.ApproverOrderId, DateTime.UtcNow);

        // Check if approver order exists
        var existingApproverOrder = await _repository.ApproverOrders.ApproverOrderAsync(
            record.ApproverOrderId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("ApproverOrder", "ApproverOrderId", record.ApproverOrderId.ToString());

        // Map Record to Entity using Mapster
        ApproverOrder approverOrder = record.MapTo<ApproverOrder>();
        approverOrder.LastUpdatedDate = DateTime.UtcNow;

        _repository.ApproverOrders.UpdateByState(approverOrder);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverOrder updated successfully. ID: {ApproverOrderId}, Time: {Time}",
            record.ApproverOrderId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverOrder:All");
        await _cache.RemoveAsync("ApproverOrder:Active");
        await _cache.RemoveAsync($"ApproverOrder:{record.ApproverOrderId}");

        return approverOrder.MapTo<ApproverOrderDto>();
    }

    /// <summary>
    /// Deletes an approver order record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteApproverOrderRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ApproverOrderId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteApproverOrderRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting approver order. ID: {ApproverOrderId}, Time: {Time}",
            record.ApproverOrderId, DateTime.UtcNow);

        var approverOrderEntity = await _repository.ApproverOrders.ApproverOrderAsync(
            record.ApproverOrderId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("ApproverOrder", "ApproverOrderId", record.ApproverOrderId.ToString());

        await _repository.ApproverOrders.DeleteAsync(a => a.ApproverOrderId == record.ApproverOrderId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverOrder deleted successfully. ID: {ApproverOrderId}, Time: {Time}",
            record.ApproverOrderId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverOrder:All");
        await _cache.RemoveAsync("ApproverOrder:Active");
        await _cache.RemoveAsync($"ApproverOrder:{record.ApproverOrderId}");
    }

    /// <summary>
    /// Retrieves a single approver order record by its ID with caching.
    /// </summary>
    public async Task<ApproverOrderDto> ApproverOrderAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver order. ID: {ApproverOrderId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"ApproverOrder:{id}",
            factory: async () =>
            {
                var approverOrder = await _repository.ApproverOrders.ApproverOrderAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("ApproverOrder", "ApproverOrderId", id.ToString());

                _logger.LogInformation("ApproverOrder fetched successfully. ID: {ApproverOrderId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return approverOrder.MapTo<ApproverOrderDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("ApproverOrder", "ApproverOrderId", id.ToString());
    }

    /// <summary>
    /// Retrieves all approver order records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverOrderDto>> ApproverOrdersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all approver orders. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "ApproverOrder:All",
            factory: async () =>
            {
                var approverOrders = await _repository.ApproverOrders.ApproverOrdersAsync(trackChanges, cancellationToken);

                if (!approverOrders.Any())
                {
                    _logger.LogWarning("No approver orders found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<ApproverOrderDto>();
                }

                var approverOrdersDto = approverOrders.MapToList<ApproverOrderDto>();

                _logger.LogInformation("ApproverOrders fetched successfully. Count: {Count}, Time: {Time}",
                    approverOrdersDto.Count(), DateTime.UtcNow);

                return approverOrdersDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverOrderDto>();
    }

    /// <summary>
    /// Retrieves all approver orders for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverOrderDDLDto>> ApproverOrdersForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver orders for dropdown list");

        return await _cache.OrSetAsync(
            key: "ApproverOrder:DDL",
            factory: async () =>
            {
                var approverOrders = await _repository.ApproverOrders.ActiveApproverOrdersAsync(trackChanges, cancellationToken);

                if (!approverOrders.Any())
                {
                    _logger.LogWarning("No approver orders found for dropdown");
                    return Enumerable.Empty<ApproverOrderDDLDto>();
                }

                return approverOrders.MapToList<ApproverOrderDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverOrderDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all approver orders.
    /// </summary>
    public async Task<GridEntity<ApproverOrderDto>> ApproverOrdersSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM ApproverOrder";
        const string orderBy = "ApproverOrderId DESC";

        _logger.LogInformation("Fetching approver orders summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.ApproverOrders.AdoGridDataAsync<ApproverOrderDto>(sql, options, orderBy, "", cancellationToken);
    }
}
