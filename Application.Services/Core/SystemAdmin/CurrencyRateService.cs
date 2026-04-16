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

internal sealed class CurrencyRateService : ICurrencyRateService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<CurrencyRateService> _logger;
    private readonly IConfiguration _configuration;

    public CurrencyRateService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<CurrencyRateService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CurrencyRateDto> CreateAsync(CreateCurrencyRateRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCurrencyRateRecord));

        var validator = new CreateCurrencyRateRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new currency rate. CurrencyId: {CurrencyId}, Time: {Time}",
            record.CurrencyId, DateTime.UtcNow);

        CurrencyRate currencyRate = record.MapTo<CurrencyRate>();
        currencyRate.CreatedDate = DateTime.UtcNow;
        
        int currencyRateId = await _repository.CurrencyRates.CreateAndIdAsync(currencyRate, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Currency rate created successfully. ID: {CurrencyRateId}, Time: {Time}",
            currencyRateId, DateTime.UtcNow);

        await _cache.RemoveAsync("CurrencyRate:All");
        await _cache.RemoveAsync($"CurrencyRate:Currency:{record.CurrencyId}");

        var resultDto = currencyRate.MapTo<CurrencyRateDto>();
        resultDto.CurencyRateId = currencyRateId;
        return resultDto;
    }

    public async Task<CurrencyRateDto> UpdateAsync(UpdateCurrencyRateRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCurrencyRateRecord));

        var validator = new UpdateCurrencyRateRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating currency rate. ID: {CurrencyRateId}, Time: {Time}", 
            record.CurencyRateId, DateTime.UtcNow);

        var existing = await _repository.CurrencyRates.CurrencyRateAsync(
            record.CurencyRateId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("CurrencyRate", "CurencyRateId", record.CurencyRateId.ToString());

        CurrencyRate currencyRate = record.MapTo<CurrencyRate>();
        _repository.CurrencyRates.UpdateByState(currencyRate);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Currency rate updated successfully. ID: {CurrencyRateId}, Time: {Time}",
            record.CurencyRateId, DateTime.UtcNow);

        await _cache.RemoveAsync("CurrencyRate:All");
        await _cache.RemoveAsync($"CurrencyRate:{record.CurencyRateId}");
        await _cache.RemoveAsync($"CurrencyRate:Currency:{record.CurrencyId}");

        return currencyRate.MapTo<CurrencyRateDto>();
    }

    public async Task DeleteAsync(DeleteCurrencyRateRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CurencyRateId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var validator = new DeleteCurrencyRateRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting currency rate. ID: {CurrencyRateId}, Time: {Time}", 
            record.CurencyRateId, DateTime.UtcNow);

        var entity = await _repository.CurrencyRates.CurrencyRateAsync(
            record.CurencyRateId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CurrencyRate", "CurencyRateId", record.CurencyRateId.ToString());

        await _repository.CurrencyRates.DeleteAsync(cr => cr.CurencyRateId == record.CurencyRateId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Currency rate deleted successfully. ID: {CurrencyRateId}, Time: {Time}",
            record.CurencyRateId, DateTime.UtcNow);

        await _cache.RemoveAsync("CurrencyRate:All");
        await _cache.RemoveAsync($"CurrencyRate:{record.CurencyRateId}");
    }

    public async Task<CurrencyRateDto> CurrencyRateAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching currency rate. ID: {CurrencyRateId}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"CurrencyRate:{id}",
            factory: async () =>
            {
                var currencyRate = await _repository.CurrencyRates.CurrencyRateAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("CurrencyRate", "CurencyRateId", id.ToString());

                return currencyRate.MapTo<CurrencyRateDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("CurrencyRate", "CurencyRateId", id.ToString());
    }

    public async Task<IEnumerable<CurrencyRateDto>> CurrencyRatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all currency rates. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "CurrencyRate:All",
            factory: async () =>
            {
                var currencyRates = await _repository.CurrencyRates.CurrencyRatesAsync(trackChanges, cancellationToken);

                if (!currencyRates.Any())
                {
                    _logger.LogWarning("No currency rates found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<CurrencyRateDto>();
                }

                return currencyRates.MapToList<CurrencyRateDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CurrencyRateDto>();
    }

    public async Task<IEnumerable<CurrencyRateDDLDto>> CurrencyRatesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching currency rates for dropdown list");

        return await _cache.OrSetAsync(
            key: "CurrencyRate:DDL",
            factory: async () =>
            {
                var currencyRates = await _repository.CurrencyRates.CurrencyRatesAsync(trackChanges, cancellationToken);

                if (!currencyRates.Any())
                {
                    _logger.LogWarning("No currency rates found for dropdown");
                    return Enumerable.Empty<CurrencyRateDDLDto>();
                }

                return currencyRates.MapToList<CurrencyRateDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CurrencyRateDDLDto>();
    }

    public async Task<IEnumerable<CurrencyRateDto>> CurrencyRatesByCurrencyIdAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching currency rates for currency: {CurrencyId}", currencyId);

        return await _cache.OrSetAsync(
            key: $"CurrencyRate:Currency:{currencyId}",
            factory: async () =>
            {
                var rates = await _repository.CurrencyRates.CurrencyRatesByCurrencyIdAsync(currencyId, trackChanges, cancellationToken);
                return rates.MapToList<CurrencyRateDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CurrencyRateDto>();
    }

    public async Task<GridEntity<CurrencyRateDto>> CurrencyRatesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM CurrencyRate";
        const string orderBy = "CurrencyMonth DESC";

        _logger.LogInformation("Fetching currency rates summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.CurrencyRates.AdoGridDataAsync<CurrencyRateDto>(sql, options, orderBy, "", cancellationToken);
    }
}
