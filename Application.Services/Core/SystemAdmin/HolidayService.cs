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
/// Holiday service implementing business logic for holiday management.
/// Follows enterprise patterns with structured logging, caching, and exception handling.
/// </summary>
internal sealed class HolidayService : IHolidayService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<HolidayService> _logger;
    private readonly IConfiguration _configuration;

    public HolidayService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<HolidayService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    /// <summary>
    /// Creates a new holiday record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<HolidayDto> CreateAsync(CreateHolidayRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateHolidayRecord));

        // FluentValidation
        var validator = new CreateHolidayRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new holiday. Date: {HolidayDate}, Time: {Time}",
            record.HolidayDate, DateTime.UtcNow);

        // Map Record to Entity using Mapster
        Holiday holiday = record.MapTo<Holiday>();
        holiday.LastUpdatedDate = DateTime.UtcNow;
        
        int holidayId = await _repository.Holidays.CreateAndIdAsync(holiday, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Holiday created successfully. ID: {HolidayId}, Time: {Time}",
            holidayId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("Holiday:All");
        await _cache.RemoveAsync("Holiday:Active");

        // Return as DTO
        var resultDto = holiday.MapTo<HolidayDto>();
        resultDto.HolidayId = holidayId;
        return resultDto;
    }

    /// <summary>
    /// Updates an existing holiday record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task<HolidayDto> UpdateAsync(UpdateHolidayRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateHolidayRecord));

        // FluentValidation
        var validator = new UpdateHolidayRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating holiday. ID: {HolidayId}, Time: {Time}", 
            record.HolidayId, DateTime.UtcNow);

        // Check if holiday exists
        var existingHoliday = await _repository.Holidays.HolidayAsync(
            record.HolidayId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Holiday", "HolidayId", record.HolidayId.ToString());

        // Map Record to Entity using Mapster
        Holiday holiday = record.MapTo<Holiday>();
        holiday.LastUpdatedDate = DateTime.UtcNow;
        
        _repository.Holidays.UpdateByState(holiday);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Holiday updated successfully. ID: {HolidayId}, Time: {Time}",
            record.HolidayId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("Holiday:All");
        await _cache.RemoveAsync("Holiday:Active");
        await _cache.RemoveAsync($"Holiday:{record.HolidayId}");

        return holiday.MapTo<HolidayDto>();
    }

    /// <summary>
    /// Deletes a holiday record using CRUD Record pattern with validation and caching.
    /// </summary>
    public async Task DeleteAsync(DeleteHolidayRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.HolidayId <= 0)
            throw new BadRequestException("Invalid delete request!");

        // FluentValidation
        var validator = new DeleteHolidayRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting holiday. ID: {HolidayId}, Time: {Time}", 
            record.HolidayId, DateTime.UtcNow);

        var holidayEntity = await _repository.Holidays.HolidayAsync(
            record.HolidayId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Holiday", "HolidayId", record.HolidayId.ToString());

        await _repository.Holidays.DeleteAsync(h => h.HolidayId == record.HolidayId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Holiday deleted successfully. ID: {HolidayId}, Time: {Time}",
            record.HolidayId, DateTime.UtcNow);

        // Clear cache after mutation
        await _cache.RemoveAsync("Holiday:All");
        await _cache.RemoveAsync("Holiday:Active");
        await _cache.RemoveAsync($"Holiday:{record.HolidayId}");
    }

    /// <summary>
    /// Retrieves a single holiday record by its ID with caching.
    /// </summary>
    public async Task<HolidayDto> HolidayAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching holiday. ID: {HolidayId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"Holiday:{id}",
            factory: async () =>
            {
                var holiday = await _repository.Holidays.HolidayAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("Holiday", "HolidayId", id.ToString());

                _logger.LogInformation("Holiday fetched successfully. ID: {HolidayId}, Time: {Time}",
                    id, DateTime.UtcNow);

                return holiday.MapTo<HolidayDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("Holiday", "HolidayId", id.ToString());
    }

    /// <summary>
    /// Retrieves all holiday records from the database with caching.
    /// </summary>
    public async Task<IEnumerable<HolidayDto>> HolidaysAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all holidays. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "Holiday:All",
            factory: async () =>
            {
                var holidays = await _repository.Holidays.HolidaysAsync(trackChanges, cancellationToken);

                if (!holidays.Any())
                {
                    _logger.LogWarning("No holidays found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<HolidayDto>();
                }

                var holidaysDto = holidays.MapToList<HolidayDto>();

                _logger.LogInformation("Holidays fetched successfully. Count: {Count}, Time: {Time}",
                    holidaysDto.Count(), DateTime.UtcNow);

                return holidaysDto;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<HolidayDto>();
    }

    /// <summary>
    /// Retrieves all holidays for dropdown list asynchronously with caching.
    /// </summary>
    public async Task<IEnumerable<HolidayDDLDto>> HolidaysForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching holidays for dropdown list");

        return await _cache.OrSetAsync(
            key: "Holiday:DDL",
            factory: async () =>
            {
                var holidays = await _repository.Holidays.ActiveHolidaysAsync(trackChanges, cancellationToken);

                if (!holidays.Any())
                {
                    _logger.LogWarning("No holidays found for dropdown");
                    return Enumerable.Empty<HolidayDDLDto>();
                }

                return holidays.MapToList<HolidayDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<HolidayDDLDto>();
    }

    /// <summary>
    /// Retrieves a paginated summary grid of all holidays.
    /// </summary>
    public async Task<GridEntity<HolidayDto>> HolidaysSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM Holiday";
        const string orderBy = "HolidayDate DESC";

        _logger.LogInformation("Fetching holidays summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.Holidays.AdoGridDataAsync<HolidayDto>(sql, options, orderBy, "", cancellationToken);
    }
}
