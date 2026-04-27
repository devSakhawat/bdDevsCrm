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

internal sealed class CrmApplicationConditionService : ICrmApplicationConditionService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmApplicationConditionService> _logger;
    private readonly IConfiguration _config;

    public CrmApplicationConditionService(IRepositoryManager repository, ILogger<CrmApplicationConditionService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmApplicationConditionDto> CreateAsync(CreateCrmApplicationConditionRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmApplicationConditionRecord));
        var entity = record.MapTo<CrmApplicationCondition>();
        int newId = await _repository.CrmApplicationConditions.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        var dto = entity.MapTo<CrmApplicationConditionDto>();
        dto.ApplicationConditionId = newId;
        return dto;
    }

    public async Task<CrmApplicationConditionDto> UpdateAsync(UpdateCrmApplicationConditionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmApplicationConditionRecord));
        _ = await _repository.CrmApplicationConditions.ApplicationConditionAsync(record.ApplicationConditionId, false, cancellationToken)
            ?? throw new NotFoundException("CrmApplicationCondition", "ApplicationConditionId", record.ApplicationConditionId.ToString());
        var entity = record.MapTo<CrmApplicationCondition>();
        _repository.CrmApplicationConditions.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmApplicationConditionDto>();
    }

    public async Task DeleteAsync(DeleteCrmApplicationConditionRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.ApplicationConditionId <= 0) throw new BadRequestException("Invalid delete request!");
        await _repository.CrmApplicationConditions.DeleteAsync(x => x.ApplicationConditionId == record.ApplicationConditionId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmApplicationConditionDto> ApplicationConditionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => (await _repository.CrmApplicationConditions.ApplicationConditionAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("CrmApplicationCondition", "ApplicationConditionId", id.ToString())).MapTo<CrmApplicationConditionDto>();

    public async Task<IEnumerable<CrmApplicationConditionDto>> ApplicationConditionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmApplicationConditions.ApplicationConditionsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmApplicationConditionDto>() : Enumerable.Empty<CrmApplicationConditionDto>();
    }

    public async Task<IEnumerable<CrmApplicationConditionDto>> ApplicationConditionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmApplicationConditions.ApplicationConditionsByApplicationIdAsync(applicationId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmApplicationConditionDto>() : Enumerable.Empty<CrmApplicationConditionDto>();
    }

    public async Task<GridEntity<CrmApplicationConditionDto>> ApplicationConditionsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT ApplicationConditionId, ApplicationId, ConditionText, ConditionType, Status, DueDate, MetDate, MetBy, Notes, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmApplicationCondition";
        return await _repository.CrmApplicationConditions.AdoGridDataAsync<CrmApplicationConditionDto>(sql, options, "ApplicationConditionId DESC", string.Empty, cancellationToken);
    }

    public async Task<CrmApplicationConditionDto> ChangeStatusAsync(ChangeCrmApplicationConditionStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmApplicationConditionStatusRecord));
        var entity = await _repository.CrmApplicationConditions.ApplicationConditionAsync(record.ApplicationConditionId, true, cancellationToken)
            ?? throw new NotFoundException("CrmApplicationCondition", "ApplicationConditionId", record.ApplicationConditionId.ToString());
        entity.Status = record.Status;
        entity.Notes = record.Notes;
        entity.MetBy = record.ChangedBy;
        entity.MetDate = record.Status is 3 or 4 ? DateTime.UtcNow : entity.MetDate;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;
        _repository.CrmApplicationConditions.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        var remaining = (await _repository.CrmApplicationConditions.ApplicationConditionsByApplicationIdAsync(entity.ApplicationId, false, cancellationToken))
            .Where(x => x.ApplicationConditionId != entity.ApplicationConditionId)
            .ToList();
        remaining.Add(entity);
        if (remaining.All(x => x.Status is 3 or 4))
        {
            var app = await _repository.CrmApplications.CrmApplicationAsync(entity.ApplicationId, true, cancellationToken);
            if (app != null && app.Status == 4)
            {
                app.Status = 5;
                app.UpdatedBy = record.ChangedBy;
                app.UpdatedDate = DateTime.UtcNow;
                _repository.CrmApplications.UpdateByState(app);
                await _repository.SaveAsync(cancellationToken);
            }
        }

        return entity.MapTo<CrmApplicationConditionDto>();
    }
}
