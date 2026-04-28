using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Extensions;
using bdDevs.Shared.Records.CRM;
using Domain.Contracts.Repositories;
using Domain.Contracts.Services.CRM;
using Domain.Entities.Entities.CRM;
using Domain.Exceptions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Application.Services.CRM;

internal sealed class CrmCommunicationLogService : ICrmCommunicationLogService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmCommunicationLogService> _logger;
    private readonly IConfiguration _configuration;

    public CrmCommunicationLogService(IRepositoryManager repository, ILogger<CrmCommunicationLogService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmCommunicationLogDto> CreateAsync(CreateCrmCommunicationLogRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmCommunicationLogRecord));
        await ValidateEntityAsync(record.EntityType, record.EntityId, cancellationToken);
        var entity = record.MapTo<CrmCommunicationLog>();
        int newId = await _repository.CrmCommunicationLogs.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        entity.CommunicationLogId = newId;
        return entity.MapTo<CrmCommunicationLogDto>();
    }

    public async Task<CrmCommunicationLogDto> UpdateAsync(UpdateCrmCommunicationLogRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmCommunicationLogRecord));
        _ = await _repository.CrmCommunicationLogs.CommunicationLogAsync(record.CommunicationLogId, false, cancellationToken)
            ?? throw new NotFoundException("CrmCommunicationLog", "CommunicationLogId", record.CommunicationLogId.ToString());
        await ValidateEntityAsync(record.EntityType, record.EntityId, cancellationToken);
        var entity = record.MapTo<CrmCommunicationLog>();
        _repository.CrmCommunicationLogs.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmCommunicationLogDto>();
    }

    public async Task DeleteAsync(DeleteCrmCommunicationLogRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.CommunicationLogId <= 0) throw new BadRequestException("Invalid delete request!");
        var entity = await _repository.CrmCommunicationLogs.CommunicationLogAsync(record.CommunicationLogId, true, cancellationToken)
            ?? throw new NotFoundException("CrmCommunicationLog", "CommunicationLogId", record.CommunicationLogId.ToString());
        entity.IsDeleted = true;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmCommunicationLogs.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmCommunicationLogDto> CommunicationLogAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmCommunicationLogs.CommunicationLogAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmCommunicationLog", "CommunicationLogId", id.ToString())).MapTo<CrmCommunicationLogDto>();

    public async Task<IEnumerable<CrmCommunicationLogDto>> CommunicationLogsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommunicationLogs.CommunicationLogsAsync(trackChanges, cancellationToken);
        return entities.Where(x => !x.IsDeleted).Any() ? entities.Where(x => !x.IsDeleted).MapToList<CrmCommunicationLogDto>() : Enumerable.Empty<CrmCommunicationLogDto>();
    }

    public async Task<IEnumerable<CrmCommunicationLogDto>> CommunicationLogsByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmCommunicationLogs.CommunicationLogsByEntityAsync(entityType, entityId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmCommunicationLogDto>() : Enumerable.Empty<CrmCommunicationLogDto>();
    }

    public async Task<GridEntity<CrmCommunicationLogDto>> CommunicationLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT CommunicationLogId, EntityType, EntityId, BranchId, CommunicationType, Direction, Subject, BodyOrNotes, DurationSeconds, OutcomeStatus, LoggedBy, LoggedDate, IsDeleted, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmCommunicationLog WHERE IsDeleted = 0";
        return await _repository.CrmCommunicationLogs.AdoGridDataAsync<CrmCommunicationLogDto>(sql, options, "CommunicationLogId DESC", string.Empty, cancellationToken);
    }

    private async Task ValidateEntityAsync(byte entityType, int entityId, CancellationToken cancellationToken)
    {
        if (entityId <= 0) throw new BadRequestException("Entity id must be greater than zero.");
        switch (entityType)
        {
            case 1:
                _ = await _repository.CrmLeads.CrmLeadAsync(entityId, false, cancellationToken)
                    ?? throw new NotFoundException("CrmLead", "LeadId", entityId.ToString());
                break;
            case 2:
                _ = await _repository.CrmStudents.CrmStudentAsync(entityId, false, cancellationToken)
                    ?? throw new NotFoundException("CrmStudent", "StudentId", entityId.ToString());
                break;
            case 3:
                _ = await _repository.CrmApplications.CrmApplicationAsync(entityId, false, cancellationToken)
                    ?? throw new NotFoundException("CrmApplication", "ApplicationId", entityId.ToString());
                break;
            default:
                throw new BadRequestException("Unsupported communication entity type.");
        }
    }
}
