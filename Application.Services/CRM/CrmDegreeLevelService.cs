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

internal sealed class CrmDegreeLevelService : ICrmDegreeLevelService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmDegreeLevelService> _logger;
    private readonly IConfiguration _config;

    public CrmDegreeLevelService(IRepositoryManager repository, ILogger<CrmDegreeLevelService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmDegreeLevelDto> CreateAsync(CreateCrmDegreeLevelRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmDegreeLevelRecord));

        _logger.LogInformation("Creating degree level. Name: {Name}, Time: {Time}", record.Name, DateTime.UtcNow);

        bool exists = await _repository.CrmDegreeLevels.ExistsAsync(
            x => x.Name.Trim().ToLower() == record.Name.Trim().ToLower(), cancellationToken: cancellationToken);
        if (exists) throw new DuplicateRecordException("DegreeLevel", "Name");

        CrmDegreeLevel entity = record.MapTo<CrmDegreeLevel>();
        int newId = await _repository.CrmDegreeLevels.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Degree level created. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmDegreeLevelDto>() with { DegreeLevelId = newId };
    }

    public async Task<CrmDegreeLevelDto> UpdateAsync(UpdateCrmDegreeLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmDegreeLevelRecord));

        _ = await _repository.CrmDegreeLevels
            .FirstOrDefaultAsync(x => x.DegreeLevelId == record.DegreeLevelId, false, cancellationToken)
            ?? throw new NotFoundException("DegreeLevel", "DegreeLevelId", record.DegreeLevelId.ToString());

        bool dup = await _repository.CrmDegreeLevels.ExistsAsync(
            x => x.Name.Trim().ToLower() == record.Name.Trim().ToLower() && x.DegreeLevelId != record.DegreeLevelId, cancellationToken: cancellationToken);
        if (dup) throw new DuplicateRecordException("DegreeLevel", "Name");

        CrmDegreeLevel entity = record.MapTo<CrmDegreeLevel>();
        _repository.CrmDegreeLevels.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        return entity.MapTo<CrmDegreeLevelDto>();
    }

    public async Task DeleteAsync(DeleteCrmDegreeLevelRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.DegreeLevelId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmDegreeLevels
            .FirstOrDefaultAsync(x => x.DegreeLevelId == record.DegreeLevelId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DegreeLevel", "DegreeLevelId", record.DegreeLevelId.ToString());

        await _repository.CrmDegreeLevels.DeleteAsync(x => x.DegreeLevelId == record.DegreeLevelId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmDegreeLevelDto> DegreeLevelAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmDegreeLevels
            .FirstOrDefaultAsync(x => x.DegreeLevelId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("DegreeLevel", "DegreeLevelId", id.ToString());
        return entity.MapTo<CrmDegreeLevelDto>();
    }

    public async Task<IEnumerable<CrmDegreeLevelDto>> DegreeLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmDegreeLevels.CrmDegreeLevelsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmDegreeLevelDto>() : Enumerable.Empty<CrmDegreeLevelDto>();
    }

    public async Task<IEnumerable<CrmDegreeLevelDto>> DegreeLevelsForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmDegreeLevels.CrmDegreeLevelsAsync(false, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmDegreeLevelDto>() : Enumerable.Empty<CrmDegreeLevelDto>();
    }

    public async Task<GridEntity<CrmDegreeLevelDto>> DegreeLevelsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT DegreeLevelId, Name, SortOrder, IsActive, CreatedDate, CreatedBy FROM CrmDegreeLevel";
        const string orderBy = "SortOrder ASC";
        return await _repository.CrmDegreeLevels.AdoGridDataAsync<CrmDegreeLevelDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
