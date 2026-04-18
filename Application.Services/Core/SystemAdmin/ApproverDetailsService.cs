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
/// ApproverDetails service implementing business logic for approver details management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class ApproverDetailsService : IApproverDetailsService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<ApproverDetailsService> _logger;
    private readonly IConfiguration _configuration;

    public ApproverDetailsService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<ApproverDetailsService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new approver details record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverDetailsDto> CreateAsync(CreateApproverDetailsRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateApproverDetailsRecord));

        // FluentValidation
        var validator = new CreateApproverDetailsRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new approver details. Time: {Time}",
            DateTime.UtcNow);

        // Map Record to Entity using Mapster
        ApproverDetails approverDetails = record.MapTo<ApproverDetails>();

        int remarksId = await _repository.ApproverDetails.CreateAndIdAsync(approverDetails, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverDetails created successfully. ID: {RemarksId}, Time: {Time}",
            remarksId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverDetails:All");
        await _cache.RemoveAsync("ApproverDetails:Active");

        // Return as DTO
        var resultDto = approverDetails.MapTo<ApproverDetailsDto>();
        resultDto.RemarksId = remarksId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing approver details record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<ApproverDetailsDto> UpdateAsync(UpdateApproverDetailsRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateApproverDetailsRecord));

        // FluentValidation
        var validator = new UpdateApproverDetailsRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating approver details. ID: {RemarksId}, Time: {Time}",
            record.RemarksId, DateTime.UtcNow);

        // Check if approver details exists
        var existingApproverDetails = await _repository.ApproverDetails.ApproverDetailsAsync(
            record.RemarksId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("ApproverDetails", "RemarksId", record.RemarksId.ToString());

        // Map Record to Entity using Mapster
        ApproverDetails approverDetails = record.MapTo<ApproverDetails>();

        _repository.ApproverDetails.UpdateByState(approverDetails);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverDetails updated successfully. ID: {RemarksId}, Time: {Time}",
            record.RemarksId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverDetails:All");
        await _cache.RemoveAsync("ApproverDetails:Active");
        await _cache.RemoveAsync($"ApproverDetails:{record.RemarksId}");

        return approverDetails.MapTo<ApproverDetailsDto>();
    }

    /// <summary>
    /// Deletes an approver details record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteApproverDetailsRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.RemarksId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteApproverDetailsRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting approver details. ID: {RemarksId}, Time: {Time}",
            record.RemarksId, DateTime.UtcNow);

        var approverDetailsEntity = await _repository.ApproverDetails.ApproverDetailsAsync(
            record.RemarksId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("ApproverDetails", "RemarksId", record.RemarksId.ToString());

        await _repository.ApproverDetails.DeleteAsync(a => a.RemarksId == record.RemarksId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("ApproverDetails deleted successfully. ID: {RemarksId}, Time: {Time}",
            record.RemarksId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("ApproverDetails:All");
        await _cache.RemoveAsync("ApproverDetails:Active");
        await _cache.RemoveAsync($"ApproverDetails:{record.RemarksId}");
    }

    /// <summary>
    /// Retrieves a single approver details record by its ID with caching.
    /// </summary>
    public async Task<ApproverDetailsDto> ApproverDetailAsync(int remarksId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver details. ID: {RemarksId}, Time: {Time}", remarksId, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"ApproverDetails:{remarksId}",
            factory: async () =>
            {
                var approverDetails = await _repository.ApproverDetails.ApproverDetailsAsync(remarksId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("ApproverDetails", "RemarksId", remarksId.ToString());

                _logger.LogInformation("ApproverDetails fetched successfully. ID: {RemarksId}, Time: {Time}",
                    remarksId, DateTime.UtcNow);

                return approverDetails.MapTo<ApproverDetailsDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("ApproverDetails", "RemarksId", remarksId.ToString());
    }

    /// <summary>
    /// Retrieves all approver details records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverDetailsDto>> ApproverDetailsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all approver details. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "ApproverDetails:All",
            factory: async () =>
            {
                var approverDetailsList = await _repository.ApproverDetails.ApproverDetailsListAsync(trackChanges, cancellationToken);

                if (!approverDetailsList.Any())
                {
                    _logger.LogWarning("No approver details found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<ApproverDetailsDto>();
                }

                var approverDetailsDto = approverDetailsList.MapToList<ApproverDetailsDto>();

                _logger.LogInformation("ApproverDetails fetched successfully. Count: {Count}, Time: {Time}",
                    approverDetailsDto.Count(), DateTime.UtcNow);

                return approverDetailsDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverDetailsDto>();
    }

    /// <summary>
    /// Retrieves all approver details for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<ApproverDetailsDDLDto>> ApproverDetailsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching approver details for dropdown list");

        return await _cache.OrSetAsync(
            key: "ApproverDetails:DDL",
            factory: async () =>
            {
                var approverDetailsList = await _repository.ApproverDetails.ActiveApproverDetailsAsync(trackChanges, cancellationToken);

                if (!approverDetailsList.Any())
                {
                    _logger.LogWarning("No approver details found for dropdown");
                    return Enumerable.Empty<ApproverDetailsDDLDto>();
                }

                return approverDetailsList.MapToList<ApproverDetailsDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<ApproverDetailsDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all approver details.
    /// </summary>
    public async Task<GridEntity<ApproverDetailsDto>> ApproverDetailsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM ApproverDetails";
        const string orderBy = "RemarksId DESC";

        _logger.LogInformation("Fetching approver details summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.ApproverDetails.AdoGridDataAsync<ApproverDetailsDto>(sql, options, orderBy, "", cancellationToken);
    }
}
