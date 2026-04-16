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
/// Assembly info service implementing business logic for assembly info management.
/// </summary>
internal sealed class AssemblyInfoService : IAssemblyInfoService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AssemblyInfoService> _logger;
    private readonly IConfiguration _configuration;

    public AssemblyInfoService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AssemblyInfoService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<AssemblyInfoDto> CreateAsync(CreateAssemblyInfoRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAssemblyInfoRecord));

        var validator = new CreateAssemblyInfoRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new assembly info. Title: {Title}, Time: {Time}",
            record.AssemblyTitle, DateTime.UtcNow);

        AssemblyInfo assemblyInfo = record.MapTo<AssemblyInfo>();

        int assemblyInfoId = await _repository.AssemblyInfos.CreateAndIdAsync(assemblyInfo, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Assembly info created successfully. ID: {Id}, Time: {Time}",
            assemblyInfoId, DateTime.UtcNow);

        await _cache.RemoveAsync("AssemblyInfo:All");
        await _cache.RemoveAsync("AssemblyInfo:DDL");

        var resultDto = assemblyInfo.MapTo<AssemblyInfoDto>();
        resultDto.AssemblyInfoId = assemblyInfoId;
        return resultDto;
    }

    public async Task<AssemblyInfoDto> UpdateAsync(UpdateAssemblyInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAssemblyInfoRecord));

        var validator = new UpdateAssemblyInfoRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Updating assembly info. ID: {Id}, Time: {Time}",
            record.AssemblyInfoId, DateTime.UtcNow);

        var existing = await _repository.AssemblyInfos.AssemblyInfoAsync(
            record.AssemblyInfoId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AssemblyInfo", "AssemblyInfoId", record.AssemblyInfoId.ToString());

        AssemblyInfo assemblyInfo = record.MapTo<AssemblyInfo>();

        _repository.AssemblyInfos.UpdateByState(assemblyInfo);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Assembly info updated successfully. ID: {Id}, Time: {Time}",
            record.AssemblyInfoId, DateTime.UtcNow);

        await _cache.RemoveAsync("AssemblyInfo:All");
        await _cache.RemoveAsync("AssemblyInfo:DDL");
        await _cache.RemoveAsync($"AssemblyInfo:{record.AssemblyInfoId}");

        return assemblyInfo.MapTo<AssemblyInfoDto>();
    }

    public async Task DeleteAsync(DeleteAssemblyInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AssemblyInfoId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var validator = new DeleteAssemblyInfoRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Deleting assembly info. ID: {Id}, Time: {Time}",
            record.AssemblyInfoId, DateTime.UtcNow);

        var entity = await _repository.AssemblyInfos.AssemblyInfoAsync(
            record.AssemblyInfoId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AssemblyInfo", "AssemblyInfoId", record.AssemblyInfoId.ToString());

        await _repository.AssemblyInfos.DeleteAsync(a => a.AssemblyInfoId == record.AssemblyInfoId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Assembly info deleted successfully. ID: {Id}, Time: {Time}",
            record.AssemblyInfoId, DateTime.UtcNow);

        await _cache.RemoveAsync("AssemblyInfo:All");
        await _cache.RemoveAsync("AssemblyInfo:DDL");
        await _cache.RemoveAsync($"AssemblyInfo:{record.AssemblyInfoId}");
    }

    public async Task<AssemblyInfoDto> AssemblyInfoAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching assembly info. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: $"AssemblyInfo:{id}",
            factory: async () =>
            {
                var assemblyInfo = await _repository.AssemblyInfos.AssemblyInfoAsync(id, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AssemblyInfo", "AssemblyInfoId", id.ToString());

                _logger.LogInformation("Assembly info fetched successfully. ID: {Id}, Time: {Time}",
                    id, DateTime.UtcNow);

                return assemblyInfo.MapTo<AssemblyInfoDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AssemblyInfo", "AssemblyInfoId", id.ToString());
    }

    public async Task<IEnumerable<AssemblyInfoDto>> AssemblyInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all assembly infos. Time: {Time}", DateTime.UtcNow);

        return await _cache.OrSetAsync(
            key: "AssemblyInfo:All",
            factory: async () =>
            {
                var assemblyInfos = await _repository.AssemblyInfos.AssemblyInfosAsync(trackChanges, cancellationToken);

                if (!assemblyInfos.Any())
                {
                    _logger.LogWarning("No assembly infos found. Time: {Time}", DateTime.UtcNow);
                    return Enumerable.Empty<AssemblyInfoDto>();
                }

                var dtos = assemblyInfos.MapToList<AssemblyInfoDto>();

                _logger.LogInformation("Assembly infos fetched successfully. Count: {Count}, Time: {Time}",
                    dtos.Count(), DateTime.UtcNow);

                return dtos;
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AssemblyInfoDto>();
    }

    public async Task<IEnumerable<AssemblyInfoDto>> AssemblyInfosForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching assembly infos for dropdown list");

        return await _cache.OrSetAsync(
            key: "AssemblyInfo:DDL",
            factory: async () =>
            {
                var assemblyInfos = await _repository.AssemblyInfos.AssemblyInfosAsync(trackChanges, cancellationToken);

                if (!assemblyInfos.Any())
                {
                    _logger.LogWarning("No assembly infos found for dropdown");
                    return Enumerable.Empty<AssemblyInfoDto>();
                }

                return assemblyInfos.MapToList<AssemblyInfoDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AssemblyInfoDto>();
    }

    public async Task<GridEntity<AssemblyInfoDto>> AssemblyInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AssemblyInfo";
        const string orderBy = "AssemblyTitle ASC";

        _logger.LogInformation("Fetching assembly infos summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AssemblyInfos.AdoGridDataAsync<AssemblyInfoDto>(sql, options, orderBy, "", cancellationToken);
    }
}

internal sealed class CreateAssemblyInfoRecordValidator : AbstractValidator<CreateAssemblyInfoRecord>
{
    public CreateAssemblyInfoRecordValidator()
    {
        RuleFor(x => x.AssemblyTitle).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.AssemblyCompany).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyProduct).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyCopyright).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyVersion).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProductBanner).NotEmpty().MaximumLength(500);
        RuleFor(x => x.PoweredBy).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PoweredByUrl).NotEmpty().MaximumLength(500);
    }
}

internal sealed class UpdateAssemblyInfoRecordValidator : AbstractValidator<UpdateAssemblyInfoRecord>
{
    public UpdateAssemblyInfoRecordValidator()
    {
        RuleFor(x => x.AssemblyInfoId).GreaterThan(0);
        RuleFor(x => x.AssemblyTitle).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyDescription).NotEmpty().MaximumLength(500);
        RuleFor(x => x.AssemblyCompany).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyProduct).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyCopyright).NotEmpty().MaximumLength(200);
        RuleFor(x => x.AssemblyVersion).NotEmpty().MaximumLength(50);
        RuleFor(x => x.ProductBanner).NotEmpty().MaximumLength(500);
        RuleFor(x => x.PoweredBy).NotEmpty().MaximumLength(200);
        RuleFor(x => x.PoweredByUrl).NotEmpty().MaximumLength(500);
    }
}

internal sealed class DeleteAssemblyInfoRecordValidator : AbstractValidator<DeleteAssemblyInfoRecord>
{
    public DeleteAssemblyInfoRecordValidator()
    {
        RuleFor(x => x.AssemblyInfoId).GreaterThan(0);
    }
}
