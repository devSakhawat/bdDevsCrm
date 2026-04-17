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

internal sealed class AuditTypeService : IAuditTypeService
{
    private readonly IRepositoryManager _repository;
    private readonly IHybridCacheService _cache;
    private readonly ILogger<AuditTypeService> _logger;
    private readonly IConfiguration _configuration;

    public AuditTypeService(
        IRepositoryManager repository,
        IHybridCacheService cache,
        ILogger<AuditTypeService> logger,
        IConfiguration configuration)
    {
        _repository = repository;
        _cache = cache;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<AuditTypeDto> CreateAsync(CreateAuditTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateAuditTypeRecord));

        var validator = new CreateAuditTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        _logger.LogInformation("Creating new audit type. Name: {AuditType1}, Time: {Time}",
            record.AuditType1, DateTime.UtcNow);

        AuditType auditType = record.MapTo<AuditType>();

        int auditTypeId = await _repository.AuditTypes.CreateAndIdAsync(auditType, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("AuditType created successfully. ID: {AuditTypeId}, Time: {Time}",
            auditTypeId, DateTime.UtcNow);

        await _cache.RemoveAsync("AuditType:All");

        var resultDto = auditType.MapTo<AuditTypeDto>();
        resultDto.AuditTypeId = auditTypeId;
        return resultDto;
    }

    public async Task<AuditTypeDto> UpdateAsync(UpdateAuditTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateAuditTypeRecord));

        var validator = new UpdateAuditTypeRecordValidator();
        var validationResult = await validator.ValidateAsync(record, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (record.AuditTypeId is not { } updateId || updateId <= 0)
            throw new BadRequestException("AuditTypeId is required");

        var auditType = await _repository.AuditTypes.AuditTypeAsync(updateId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AuditType", "AuditTypeId", updateId.ToString());

        _logger.LogInformation("Updating audit type. ID: {AuditTypeId}, Time: {Time}",
            record.AuditTypeId, DateTime.UtcNow);

        record.MapTo(auditType);
        
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("AuditType updated successfully. ID: {AuditTypeId}, Time: {Time}",
            record.AuditTypeId, DateTime.UtcNow);

        await _cache.RemoveAsync($"AuditType:{record.AuditTypeId}");
        await _cache.RemoveAsync("AuditType:All");

        return auditType.MapTo<AuditTypeDto>();
    }

    public async Task DeleteAsync(DeleteAuditTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(DeleteAuditTypeRecord));

        if (record.AuditTypeId <= 0)
            throw new BadRequestException("AuditTypeId is required");

        var auditType = await _repository.AuditTypes.AuditTypeAsync(record.AuditTypeId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AuditType", "AuditTypeId", record.AuditTypeId.ToString());

        _logger.LogInformation("Deleting audit type. ID: {AuditTypeId}, Time: {Time}",
            record.AuditTypeId, DateTime.UtcNow);

        _repository.AuditTypes.Delete(auditType);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("AuditType deleted successfully. ID: {AuditTypeId}, Time: {Time}",
            record.AuditTypeId, DateTime.UtcNow);

        await _cache.RemoveAsync($"AuditType:{record.AuditTypeId}");
        await _cache.RemoveAsync("AuditType:All");
    }

    public async Task<AuditTypeDto> AuditTypeAsync(int auditTypeId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: $"AuditType:{auditTypeId}",
            factory: async () =>
            {
                var auditType = await _repository.AuditTypes.AuditTypeAsync(auditTypeId, trackChanges, cancellationToken)
                    ?? throw new NotFoundException("AuditType", "AuditTypeId", auditTypeId.ToString());
                return auditType.MapTo<AuditTypeDto>();
            },
            profile: CacheProfile.Static
        ) ?? throw new NotFoundException("AuditType", "AuditTypeId", auditTypeId.ToString());
    }

    public async Task<IEnumerable<AuditTypeDto>> AuditTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "AuditType:All",
            factory: async () =>
            {
                var auditTypes = await _repository.AuditTypes.AuditTypesAsync(trackChanges, cancellationToken);
                return auditTypes.MapToList<AuditTypeDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AuditTypeDto>();
    }

    public async Task<IEnumerable<AuditTypeDDLDto>> AuditTypesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default)
    {
        return await _cache.OrSetAsync(
            key: "AuditType:All",
            factory: async () =>
            {
                var auditTypes = await _repository.AuditTypes.AuditTypesAsync(trackChanges, cancellationToken);
                return auditTypes.MapToList<AuditTypeDDLDto>();
            },
            profile: CacheProfile.Static
        ) ?? Enumerable.Empty<AuditTypeDDLDto>();
    }

    public async Task<GridEntity<AuditTypeDto>> AuditTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT * FROM AuditType";
        const string orderBy = "AuditType1 ASC";

        _logger.LogInformation("Fetching audit types summary grid. Time: {Time}", DateTime.UtcNow);

        return await _repository.AuditTypes.AdoGridDataAsync<AuditTypeDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
