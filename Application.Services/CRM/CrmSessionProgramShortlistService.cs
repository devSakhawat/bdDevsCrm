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

internal sealed class CrmSessionProgramShortlistService : ICrmSessionProgramShortlistService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmSessionProgramShortlistService> _logger;
    private readonly IConfiguration _config;

    public CrmSessionProgramShortlistService(IRepositoryManager repository, ILogger<CrmSessionProgramShortlistService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmSessionProgramShortlistDto> CreateAsync(CreateCrmSessionProgramShortlistRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmSessionProgramShortlistRecord));

        var entity = record.MapTo<CrmSessionProgramShortlist>();
        int newId = await _repository.CrmSessionProgramShortlists.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogInformation("CrmSessionProgramShortlist created. ID: {Id}", newId);
        return entity.MapTo<CrmSessionProgramShortlistDto>() with { SessionProgramShortlistId = newId };
    }

    public async Task<CrmSessionProgramShortlistDto> UpdateAsync(UpdateCrmSessionProgramShortlistRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmSessionProgramShortlistRecord));
        _ = await _repository.CrmSessionProgramShortlists.CrmSessionProgramShortlistAsync(record.SessionProgramShortlistId, false, cancellationToken)
            ?? throw new NotFoundException("CrmSessionProgramShortlist", "SessionProgramShortlistId", record.SessionProgramShortlistId.ToString());

        var entity = record.MapTo<CrmSessionProgramShortlist>();
        _repository.CrmSessionProgramShortlists.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmSessionProgramShortlistDto>();
    }

    public async Task DeleteAsync(DeleteCrmSessionProgramShortlistRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.SessionProgramShortlistId <= 0) throw new BadRequestException("Invalid delete request!");
        _ = await _repository.CrmSessionProgramShortlists.CrmSessionProgramShortlistAsync(record.SessionProgramShortlistId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmSessionProgramShortlist", "SessionProgramShortlistId", record.SessionProgramShortlistId.ToString());
        await _repository.CrmSessionProgramShortlists.DeleteAsync(x => x.SessionProgramShortlistId == record.SessionProgramShortlistId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmSessionProgramShortlistDto> SessionProgramShortlistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmSessionProgramShortlists.CrmSessionProgramShortlistAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmSessionProgramShortlist", "SessionProgramShortlistId", id.ToString());
        return entity.MapTo<CrmSessionProgramShortlistDto>();
    }

    public async Task<IEnumerable<CrmSessionProgramShortlistDto>> SessionProgramShortlistsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmSessionProgramShortlists.SessionProgramShortlistsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmSessionProgramShortlistDto>() : Enumerable.Empty<CrmSessionProgramShortlistDto>();
    }

    public async Task<IEnumerable<CrmSessionProgramShortlistDto>> SessionProgramShortlistsBySessionIdAsync(int sessionId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmSessionProgramShortlists.SessionProgramShortlistsBySessionIdAsync(sessionId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmSessionProgramShortlistDto>() : Enumerable.Empty<CrmSessionProgramShortlistDto>();
    }

    public async Task<GridEntity<CrmSessionProgramShortlistDto>> SessionProgramShortlistsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT SessionProgramShortlistId, SessionId, UniversityId, ProgramId, IntakeId, Priority, EligibilityStatus, IsRecommended FROM CrmSessionProgramShortlist";
        const string orderBy = "Priority ASC";
        return await _repository.CrmSessionProgramShortlists.AdoGridDataAsync<CrmSessionProgramShortlistDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

}
