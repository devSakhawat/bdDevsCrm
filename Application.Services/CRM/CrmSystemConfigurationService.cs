using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

internal sealed class CrmSystemConfigurationService : ICrmSystemConfigurationService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmSystemConfigurationService> _logger;
    private readonly IConfiguration _config;

    public CrmSystemConfigurationService(IRepositoryManager repository, ILogger<CrmSystemConfigurationService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmSystemConfigurationDto> CreateAsync(CreateCrmSystemConfigurationRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmSystemConfigurationRecord));

        bool exists = await _repository.CrmSystemConfigurations.ExistsAsync(
            x => x.ConfigKey.Trim().ToLower() == record.ConfigKey.Trim().ToLower(), cancellationToken: cancellationToken);
        if (exists) throw new DuplicateRecordException("SystemConfiguration", "ConfigKey");

        CrmSystemConfiguration entity = record.MapTo<CrmSystemConfiguration>();
        int newId = await _repository.CrmSystemConfigurations.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("SystemConfig created. ID: {Id}, Key: {Key}", newId, record.ConfigKey);
        return entity.MapTo<CrmSystemConfigurationDto>() with { ConfigId = newId };
    }

    public async Task<CrmSystemConfigurationDto> UpdateAsync(UpdateCrmSystemConfigurationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmSystemConfigurationRecord));

        _ = await _repository.CrmSystemConfigurations
            .FirstOrDefaultAsync(x => x.ConfigId == record.ConfigId, false, cancellationToken)
            ?? throw new NotFoundException("SystemConfiguration", "ConfigId", record.ConfigId.ToString());

        bool dup = await _repository.CrmSystemConfigurations.ExistsAsync(
            x => x.ConfigKey.Trim().ToLower() == record.ConfigKey.Trim().ToLower() && x.ConfigId != record.ConfigId, cancellationToken: cancellationToken);
        if (dup) throw new DuplicateRecordException("SystemConfiguration", "ConfigKey");

        CrmSystemConfiguration entity = record.MapTo<CrmSystemConfiguration>();
        _repository.CrmSystemConfigurations.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmSystemConfigurationDto>();
    }

    public async Task DeleteAsync(DeleteCrmSystemConfigurationRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ConfigId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmSystemConfigurations
            .FirstOrDefaultAsync(x => x.ConfigId == record.ConfigId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("SystemConfiguration", "ConfigId", record.ConfigId.ToString());

        await _repository.CrmSystemConfigurations.DeleteAsync(x => x.ConfigId == record.ConfigId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmSystemConfigurationDto> SystemConfigurationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmSystemConfigurations
            .FirstOrDefaultAsync(x => x.ConfigId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("SystemConfiguration", "ConfigId", id.ToString());
        return entity.MapTo<CrmSystemConfigurationDto>();
    }

    public async Task<CrmSystemConfigurationDto?> SystemConfigurationByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmSystemConfigurations.CrmSystemConfigurationByKeyAsync(key, false, cancellationToken);
        return entity?.MapTo<CrmSystemConfigurationDto>();
    }

    public async Task<IEnumerable<CrmSystemConfigurationDto>> SystemConfigurationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmSystemConfigurations.CrmSystemConfigurationsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmSystemConfigurationDto>() : Enumerable.Empty<CrmSystemConfigurationDto>();
    }

    public async Task<GridEntity<CrmSystemConfigurationDto>> SystemConfigurationsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT ConfigId, ConfigKey, ConfigValue, Description, IsActive, CreatedDate FROM CrmSystemConfiguration";
        const string orderBy = "ConfigKey ASC";
        return await _repository.CrmSystemConfigurations.AdoGridDataAsync<CrmSystemConfigurationDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
