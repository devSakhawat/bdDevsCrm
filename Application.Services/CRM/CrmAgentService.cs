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

internal sealed class CrmAgentService : ICrmAgentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmAgentService> _logger;
    private readonly IConfiguration _configuration;

    public CrmAgentService(IRepositoryManager repository, ILogger<CrmAgentService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task<CrmAgentDto> CreateAsync(CreateCrmAgentRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmAgentRecord));

        bool exists = await _repository.CrmAgents.ExistsAsync(x => x.PrimaryPhone.Trim().ToLower() == record.PrimaryPhone.Trim().ToLower(), cancellationToken: cancellationToken);
        if (exists)
            throw new ConflictException("Agent with this value already exists!");

        CrmAgent entity = record.MapTo<CrmAgent>();
        int newId = await _repository.CrmAgents.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Agent created successfully. ID: {AgentId}", newId);

        return entity.MapTo<CrmAgentDto>() with { AgentId = newId };
    }

    public async Task<CrmAgentDto> UpdateAsync(UpdateCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmAgentRecord));

        _ = await _repository.CrmAgents.FirstOrDefaultAsync(x => x.AgentId == record.AgentId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Agent", "AgentId", record.AgentId.ToString());

        bool duplicateExists = await _repository.CrmAgents.ExistsAsync(x => x.PrimaryPhone.Trim().ToLower() == record.PrimaryPhone.Trim().ToLower() && x.AgentId != record.AgentId, cancellationToken: cancellationToken);
        if (duplicateExists)
            throw new ConflictException("Agent with this value already exists!");

        CrmAgent entity = record.MapTo<CrmAgent>();
        _repository.CrmAgents.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Agent updated successfully. ID: {AgentId}", record.AgentId);

        return entity.MapTo<CrmAgentDto>();
    }

    public async Task DeleteAsync(DeleteCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AgentId <= 0)
            throw new BadRequestException("Invalid delete request!");

        var entity = await _repository.CrmAgents.FirstOrDefaultAsync(x => x.AgentId == record.AgentId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Agent", "AgentId", record.AgentId.ToString());

        await _repository.CrmAgents.DeleteAsync(x => x.AgentId == record.AgentId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Agent deleted successfully. ID: {AgentId}", record.AgentId);
    }

    public async Task<CrmAgentDto> AgentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entity = await _repository.CrmAgents.FirstOrDefaultAsync(x => x.AgentId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Agent", "AgentId", id.ToString());

        return entity.MapTo<CrmAgentDto>();
    }

    public async Task<IEnumerable<CrmAgentDto>> AgentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmAgents.AgentsAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Agent records found.");
            return Enumerable.Empty<CrmAgentDto>();
        }

        return entities.MapToList<CrmAgentDto>();
    }

    public async Task<IEnumerable<CrmAgentDDLDto>> AgentForDDLAsync(CancellationToken cancellationToken = default)
    {
        var entities = await _repository.CrmAgents.ListByConditionAsync(x => true, x => x.AgentName, trackChanges: false, descending: false, cancellationToken: cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Agent records found for dropdown.");
            return Enumerable.Empty<CrmAgentDDLDto>();
        }

        return entities.MapToList<CrmAgentDDLDto>();
    }

    public async Task<GridEntity<CrmAgentDto>> AgentSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        const string query = @"SELECT AgentId, AgentName, AgencyName, PrimaryPhone, PrimaryEmail, CommissionTypeId, DefaultCommissionValue, CountryId, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmAgent";
        const string orderBy = "AgentName ASC";
        return await _repository.CrmAgents.AdoGridDataAsync<CrmAgentDto>(query, options, orderBy, string.Empty, cancellationToken);
    }
}
