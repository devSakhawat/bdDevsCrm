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

internal sealed class CrmFollowUpService : ICrmFollowUpService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmFollowUpService> _logger;
    private readonly IConfiguration _config;

    public CrmFollowUpService(IRepositoryManager repository, ILogger<CrmFollowUpService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmFollowUpDto> CreateAsync(CreateCrmFollowUpRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmFollowUpRecord));

        var entity = record.MapTo<CrmFollowUp>();
        entity.Status = record.Status == 0 ? (byte)1 : record.Status;
        int newId = await _repository.CrmFollowUps.CreateAndIdAsync(entity, cancellationToken);
        await AddHistoryAsync(newId, 0, entity.Status, entity.CreatedBy, "Follow-up created", cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmFollowUpDto>() with { FollowUpId = newId };
    }

    public async Task<CrmFollowUpDto> UpdateAsync(UpdateCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmFollowUpRecord));

        var existing = await _repository.CrmFollowUps.CrmFollowUpAsync(record.FollowUpId, false, cancellationToken)
            ?? throw new NotFoundException("FollowUp", "FollowUpId", record.FollowUpId.ToString());

        if (record.Status == 2 && string.IsNullOrWhiteSpace(record.Notes))
            throw new BadRequestException("Completed follow-up requires remarks in notes.");

        var entity = record.MapTo<CrmFollowUp>();
        _repository.CrmFollowUps.UpdateByState(entity);

        if (existing.Status != entity.Status)
            await AddHistoryAsync(entity.FollowUpId, existing.Status, entity.Status, entity.UpdatedBy ?? entity.CreatedBy, entity.Notes, cancellationToken);

        await _repository.SaveAsync(cancellationToken);

        return entity.MapTo<CrmFollowUpDto>();
    }

    public async Task DeleteAsync(DeleteCrmFollowUpRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.FollowUpId <= 0) throw new BadRequestException("Invalid delete request!");
        _ = await _repository.CrmFollowUps.CrmFollowUpAsync(record.FollowUpId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("FollowUp", "FollowUpId", record.FollowUpId.ToString());
        await _repository.CrmFollowUps.DeleteAsync(x => x.FollowUpId == record.FollowUpId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmFollowUpDto> FollowUpAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmFollowUps.CrmFollowUpAsync(id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("FollowUp", "FollowUpId", id.ToString());
        return entity.MapTo<CrmFollowUpDto>();
    }

    public async Task<IEnumerable<CrmFollowUpDto>> FollowUpsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFollowUps.CrmFollowUpsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFollowUpDto>() : Enumerable.Empty<CrmFollowUpDto>();
    }

    public async Task<IEnumerable<CrmFollowUpDto>> FollowUpsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFollowUps.CrmFollowUpsByLeadIdAsync(leadId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFollowUpDto>() : Enumerable.Empty<CrmFollowUpDto>();
    }

    public async Task<IEnumerable<CrmFollowUpDto>> FollowUpForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmFollowUps.CrmFollowUpsAsync(false, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmFollowUpDto>() : Enumerable.Empty<CrmFollowUpDto>();
    }

    public async Task<GridEntity<CrmFollowUpDto>> FollowUpsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT FollowUpId, LeadId, EnquiryId, FollowUpDate, ScheduledTime, FollowUpType, ContactMethod, Notes, NextFollowUpDate, Status, MissedReason, OverriddenById, CancelledById, CancelledDate, CounselorId, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmFollowUp";
        const string orderBy = "FollowUpDate DESC";
        return await _repository.CrmFollowUps.AdoGridDataAsync<CrmFollowUpDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

    public async Task<CrmFollowUpDto> ChangeStatusAsync(ChangeCrmFollowUpStatusRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(ChangeCrmFollowUpStatusRecord));
        var entity = await _repository.CrmFollowUps.CrmFollowUpAsync(record.FollowUpId, false, cancellationToken)
            ?? throw new NotFoundException("FollowUp", "FollowUpId", record.FollowUpId.ToString());

        if (!IsAllowedTransition(entity.Status, record.NewStatus))
            throw new BadRequestException($"Invalid follow-up status transition from {entity.Status} to {record.NewStatus}.");

        if (record.NewStatus == 2 && (string.IsNullOrWhiteSpace(record.Remarks) || record.Remarks.Trim().Length < 10))
            throw new BadRequestException("Completed transition requires remarks with at least 10 characters.");

        byte oldStatus = entity.Status;
        entity.Status = record.NewStatus;
        entity.MissedReason = record.NewStatus == 3 ? record.MissedReason ?? record.Remarks : entity.MissedReason;
        entity.OverriddenById = record.NewStatus == 4 ? record.OverriddenById : entity.OverriddenById;
        entity.CancelledById = record.NewStatus == 5 ? record.CancelledById : entity.CancelledById;
        entity.CancelledDate = record.NewStatus == 5 ? DateTime.UtcNow : entity.CancelledDate;
        entity.Notes = record.Remarks ?? entity.Notes;
        entity.UpdatedBy = record.ChangedBy;
        entity.UpdatedDate = DateTime.UtcNow;

        _repository.CrmFollowUps.UpdateByState(entity);
        await AddHistoryAsync(entity.FollowUpId, oldStatus, entity.Status, record.ChangedBy, record.Remarks, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmFollowUpDto>();
    }

    public async Task<int> ProcessOverdueFollowUpsAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var overdue = (await _repository.CrmFollowUps.ListByConditionAsync(
            x => x.Status == 1 && x.FollowUpDate < today,
            x => x.FollowUpDate,
            false,
            false,
            cancellationToken)).ToList();

        foreach (var item in overdue)
        {
            item.Status = 3;
            item.MissedReason = string.IsNullOrWhiteSpace(item.MissedReason) ? "Auto marked missed by overdue tagging job." : item.MissedReason;
            item.UpdatedDate = DateTime.UtcNow;
            item.UpdatedBy = 0;
            _repository.CrmFollowUps.UpdateByState(item);
            await AddHistoryAsync(item.FollowUpId, 1, 3, 0, item.MissedReason, cancellationToken);
        }

        if (overdue.Count > 0) await _repository.SaveAsync(cancellationToken);
        return overdue.Count;
    }

    public async Task<int> ProcessUnresponsiveLeadsAsync(CancellationToken cancellationToken = default)
    {
        var followups = await _repository.CrmFollowUps.ListByConditionAsync(x => x.LeadId.HasValue, x => x.FollowUpDate, false, true, cancellationToken);
        var leadIds = followups.Where(x => x.LeadId.HasValue).Select(x => x.LeadId!.Value).Distinct().ToList();
        var unresponsiveStatus = (await _repository.CrmLeadStatuses.ListByConditionAsync(
            x => x.IsActive && (x.StatusName.ToLower().Contains("unresponsive") || (x.StatusCode ?? string.Empty).ToLower().Contains("unresponsive")),
            x => x.LeadStatusId,
            false,
            false,
            cancellationToken)).FirstOrDefault();
        if (unresponsiveStatus == null) return 0;

        int updatedCount = 0;
        foreach (var leadId in leadIds)
        {
            var recent = followups.Where(x => x.LeadId == leadId).OrderByDescending(x => x.FollowUpDate).Take(3).ToList();
            if (recent.Count == 3 && recent.All(x => x.Status == 3))
            {
                var lead = await _repository.CrmLeads.CrmLeadAsync(leadId, false, cancellationToken);
                if (lead != null && lead.LeadStatusId != unresponsiveStatus.LeadStatusId)
                {
                    lead.LeadStatusId = unresponsiveStatus.LeadStatusId;
                    lead.UpdatedDate = DateTime.UtcNow;
                    lead.UpdatedBy = 0;
                    _repository.CrmLeads.UpdateByState(lead);
                    updatedCount++;
                }
            }
        }

        if (updatedCount > 0) await _repository.SaveAsync(cancellationToken);
        return updatedCount;
    }

    private async Task AddHistoryAsync(int followUpId, byte oldStatus, byte newStatus, int changedBy, string? remarks, CancellationToken cancellationToken)
    {
        var history = new CrmFollowUpHistory
        {
            FollowUpId = followUpId,
            OldStatus = oldStatus,
            NewStatus = newStatus,
            ChangedBy = changedBy,
            ChangedDate = DateTime.UtcNow,
            Remarks = remarks
        };
        await _repository.CrmFollowUpHistories.CreateAsync(history, cancellationToken);
    }

    private static bool IsAllowedTransition(byte oldStatus, byte newStatus)
        => oldStatus == newStatus || oldStatus switch
        {
            1 => newStatus is 2 or 3 or 4 or 5,
            3 => newStatus is 2 or 4 or 5,
            4 => newStatus is 2 or 5,
            2 => false,
            5 => false,
            _ => true
        };
}
