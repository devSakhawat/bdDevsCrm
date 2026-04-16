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

internal sealed class CompetenciesService : ICompetenciesService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<CompetenciesService> _logger;
    private readonly IConfiguration _configuration;

    public CompetenciesService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<CompetenciesService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CompetenciesDto> CreateAsync(CreateCompetenciesRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCompetenciesRecord));

        var validator = new CreateCompetenciesRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new competency. Name: {CompetencyName}, Time: {Time}",
            record.CompetencyName, DateTime.UtcNow);

        Competencies competency = record.MapTo<Competencies>();
        
        int competencyId = await _repository.Competencies.CreateAndIdAsync(competency, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Competency created successfully. ID: {Id}, Time: {Time}",
            competencyId, DateTime.UtcNow);

        await _cache.RemoveAsync("Competencies:All");
        await _cache.RemoveAsync("Competencies:Active");

        var resultDto = competency.MapTo<CompetenciesDto>();
        resultDto.Id = competencyId;
        return resultDto;
    }

    public async Task<CompetenciesDto> UpdateAsync(UpdateCompetenciesRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCompetenciesRecord));

        var validator = new UpdateCompetenciesRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        var competency = await _repository.Competencies.CompetencyAsync(record.Id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Competencies", "Id", record.Id.ToString());

        _logger.LogInformation("Updating competency. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        record.MapTo(competency);
        
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Competency updated successfully. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        await _cache.RemoveAsync($"Competencies:{record.Id}");
        await _cache.RemoveAsync("Competencies:All");
        await _cache.RemoveAsync("Competencies:Active");

        return competency.MapTo<CompetenciesDto>();
    }

    public async Task DeleteAsync(DeleteCompetenciesRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(DeleteCompetenciesRecord));

        var competency = await _repository.Competencies.CompetencyAsync(record.Id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Competencies", "Id", record.Id.ToString());

        _logger.LogInformation("Deleting competency. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        _repository.Competencies.Delete(competency);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Competency deleted successfully. ID: {Id}, Time: {Time}",
            record.Id, DateTime.UtcNow);

        await _cache.RemoveAsync($"Competencies:{record.Id}");
        await _cache.RemoveAsync("Competencies:All");
        await _cache.RemoveAsync("Competencies:Active");
    }

    public async Task<CompetenciesDto> CompetencyAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: $"Competencies:{id}",
            factory: async () =>
            {
                var competency = await _repository.Competencies.CompetencyAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("Competencies", "Id", id.ToString());
                return competency.MapTo<CompetenciesDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("Competencies", "Id", id.ToString());
    }

    public async Task<IEnumerable<CompetenciesDto>> CompetenciesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "Competencies:All",
            factory: async () =>
            {
                var competencies = await _repository.Competencies.CompetenciesAsync(trackChanges, cancellationToken);
                return competencies.MapToList<CompetenciesDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CompetenciesDto>();
    }

    public async Task<IEnumerable<CompetenciesDDLDto>> CompetenciesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "Competencies:Active",
            factory: async () =>
            {
                var competencies = await _repository.Competencies.ActiveCompetenciesAsync(trackChanges, cancellationToken);
                return competencies.MapToList<CompetenciesDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<CompetenciesDDLDto>();
    }

    public async Task<GridEntity<CompetenciesDto>> CompetenciesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        var competencies = await _repository.Competencies.CompetenciesAsync(false, cancellationToken);
        var competenciesDtos = competencies.MapToList<CompetenciesDto>();
        return competenciesDtos.GridDataSource(options);
    }
}
