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

internal sealed class CrmAgentLeadService : ICrmAgentLeadService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmAgentLeadService> _logger;
    private readonly IConfiguration _config;

    public CrmAgentLeadService(IRepositoryManager repository, ILogger<CrmAgentLeadService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    public async Task<CrmAgentLeadDto> CreateAsync(CreateCrmAgentLeadRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(CreateCrmAgentLeadRecord));

        // Enforce one agent per lead
        bool exists = await _repository.CrmAgentLeads.ExistsAsync(
            x => x.LeadId == record.LeadId, cancellationToken: cancellationToken);
        if (exists) throw new DuplicateRecordException("AgentLead", "LeadId");

        CrmAgentLead entity = record.MapTo<CrmAgentLead>();
        int newId = await _repository.CrmAgentLeads.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("AgentLead created. ID: {Id}, LeadId: {LeadId}", newId, record.LeadId);
        return entity.MapTo<CrmAgentLeadDto>() with { AgentLeadId = newId };
    }

    public async Task<CrmAgentLeadDto> UpdateAsync(UpdateCrmAgentLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null) throw new BadRequestException(nameof(UpdateCrmAgentLeadRecord));

        _ = await _repository.CrmAgentLeads
            .FirstOrDefaultAsync(x => x.AgentLeadId == record.AgentLeadId, false, cancellationToken)
            ?? throw new NotFoundException("AgentLead", "AgentLeadId", record.AgentLeadId.ToString());

        // Ensure unique lead assignment (excluding self)
        bool dup = await _repository.CrmAgentLeads.ExistsAsync(
            x => x.LeadId == record.LeadId && x.AgentLeadId != record.AgentLeadId, cancellationToken: cancellationToken);
        if (dup) throw new DuplicateRecordException("AgentLead", "LeadId");

        CrmAgentLead entity = record.MapTo<CrmAgentLead>();
        _repository.CrmAgentLeads.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);
        return entity.MapTo<CrmAgentLeadDto>();
    }

    public async Task DeleteAsync(DeleteCrmAgentLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AgentLeadId <= 0) throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmAgentLeads
            .FirstOrDefaultAsync(x => x.AgentLeadId == record.AgentLeadId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AgentLead", "AgentLeadId", record.AgentLeadId.ToString());

        await _repository.CrmAgentLeads.DeleteAsync(x => x.AgentLeadId == record.AgentLeadId, false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
    }

    public async Task<CrmAgentLeadDto> AgentLeadAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmAgentLeads
            .FirstOrDefaultAsync(x => x.AgentLeadId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AgentLead", "AgentLeadId", id.ToString());
        return entity.MapTo<CrmAgentLeadDto>();
    }

    public async Task<CrmAgentLeadDto?> AgentLeadByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmAgentLeads.CrmAgentLeadByLeadIdAsync(leadId, trackChanges, cancellationToken);
        return entity?.MapTo<CrmAgentLeadDto>();
    }

    public async Task<IEnumerable<CrmAgentLeadDto>> AgentLeadsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmAgentLeads.CrmAgentLeadsAsync(trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmAgentLeadDto>() : Enumerable.Empty<CrmAgentLeadDto>();
    }

    public async Task<IEnumerable<CrmAgentLeadDto>> AgentLeadsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmAgentLeads.CrmAgentLeadsByAgentIdAsync(agentId, trackChanges, cancellationToken);
        return entities.Any() ? entities.MapToList<CrmAgentLeadDto>() : Enumerable.Empty<CrmAgentLeadDto>();
    }

    public async Task<GridEntity<CrmAgentLeadDto>> AgentLeadsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string sql = "SELECT AgentLeadId, AgentId, LeadId, AssignedDate, AssignedBy, IsActive, CreatedDate FROM CrmAgentLead";
        const string orderBy = "AgentLeadId ASC";
        return await _repository.CrmAgentLeads.AdoGridDataAsync<CrmAgentLeadDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
