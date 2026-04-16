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

internal sealed class MaritalStatusService : IMaritalStatusService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<MaritalStatusService> _logger;
    private readonly IConfiguration _configuration;

    public MaritalStatusService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<MaritalStatusService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<MaritalStatusDto> CreateAsync(CreateMaritalStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateMaritalStatusRecord));

        var validator = new CreateMaritalStatusRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new marital status. Name: {MaritalStatusName}, Time: {Time}",
            record.MaritalStatusName, DateTime.UtcNow);

        MaritalStatus maritalStatus = record.MapTo<MaritalStatus>();
        maritalStatus.CreatedDate = DateTime.UtcNow;
        maritalStatus.UpdatedDate = DateTime.UtcNow;
        
        int maritalStatusId = await _repository.MaritalStatuses.CreateAndIdAsync(maritalStatus, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("MaritalStatus created successfully. ID: {MaritalStatusId}, Time: {Time}",
            maritalStatusId, DateTime.UtcNow);

        await _cache.RemoveAsync("MaritalStatus:All");
        await _cache.RemoveAsync("MaritalStatus:Active");

        var resultDto = maritalStatus.MapTo<MaritalStatusDto>();
        resultDto.MaritalStatusId = maritalStatusId;
        return resultDto;
    }

    public async Task<MaritalStatusDto> UpdateAsync(UpdateMaritalStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateMaritalStatusRecord));

        var validator = new UpdateMaritalStatusRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var maritalStatus = await _repository.MaritalStatuses.MaritalStatusAsync(record.MaritalStatusId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("MaritalStatus", "MaritalStatusId", record.MaritalStatusId.ToString());

        _logger.LogInformation("Updating marital status. ID: {MaritalStatusId}, Time: {Time}",
            record.MaritalStatusId, DateTime.UtcNow);

        record.MapTo(maritalStatus);
        maritalStatus.UpdatedDate = DateTime.UtcNow;
        
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("MaritalStatus updated successfully. ID: {MaritalStatusId}, Time: {Time}",
            record.MaritalStatusId, DateTime.UtcNow);

        await _cache.RemoveAsync($"MaritalStatus:{record.MaritalStatusId}");
        await _cache.RemoveAsync("MaritalStatus:All");
        await _cache.RemoveAsync("MaritalStatus:Active");

        return maritalStatus.MapTo<MaritalStatusDto>();
    }

    public async Task DeleteAsync(DeleteMaritalStatusRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(DeleteMaritalStatusRecord));

        var maritalStatus = await _repository.MaritalStatuses.MaritalStatusAsync(record.MaritalStatusId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("MaritalStatus", "MaritalStatusId", record.MaritalStatusId.ToString());

        _logger.LogInformation("Deleting marital status. ID: {MaritalStatusId}, Time: {Time}",
            record.MaritalStatusId, DateTime.UtcNow);

        _repository.MaritalStatuses.Delete(maritalStatus);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("MaritalStatus deleted successfully. ID: {MaritalStatusId}, Time: {Time}",
            record.MaritalStatusId, DateTime.UtcNow);

        await _cache.RemoveAsync($"MaritalStatus:{record.MaritalStatusId}");
        await _cache.RemoveAsync("MaritalStatus:All");
        await _cache.RemoveAsync("MaritalStatus:Active");
    }

    public async Task<MaritalStatusDto> MaritalStatusAsync(int maritalStatusId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: $"MaritalStatus:{maritalStatusId}",
            factory: async () =>
            {
                var maritalStatus = await _repository.MaritalStatuses.MaritalStatusAsync(maritalStatusId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("MaritalStatus", "MaritalStatusId", maritalStatusId.ToString());
                return maritalStatus.MapTo<MaritalStatusDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("MaritalStatus", "MaritalStatusId", maritalStatusId.ToString());
    }

    public async Task<IEnumerable<MaritalStatusDto>> MaritalStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "MaritalStatus:All",
            factory: async () =>
            {
                var maritalStatuses = await _repository.MaritalStatuses.MaritalStatusesAsync(trackChanges, cancellationToken);
                return maritalStatuses.MapToList<MaritalStatusDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<MaritalStatusDto>();
    }

    public async Task<IEnumerable<MaritalStatusDDLDto>> MaritalStatusesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "MaritalStatus:Active",
            factory: async () =>
            {
                var maritalStatuses = await _repository.MaritalStatuses.ActiveMaritalStatusesAsync(trackChanges, cancellationToken);
                return maritalStatuses.MapToList<MaritalStatusDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<MaritalStatusDDLDto>();
    }

    public async Task<GridEntity<MaritalStatusDto>> MaritalStatusesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        var maritalStatuses = await _repository.MaritalStatuses.MaritalStatusesAsync(false, cancellationToken);
        var maritalStatusDtos = maritalStatuses.MapToList<MaritalStatusDto>();
        return maritalStatusDtos.GridDataSource(options);
    }
}
