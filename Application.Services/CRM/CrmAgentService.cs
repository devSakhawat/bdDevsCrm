using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>CrmAgent service implementing business logic for agent management.</summary>
internal sealed class CrmAgentService : ICrmAgentService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmAgentService> _logger;
    private readonly IConfiguration _config;

    public CrmAgentService(IRepositoryManager repository, ILogger<CrmAgentService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new agent record.</summary>
    public async Task<CrmAgentDto> CreateAsync(CreateCrmAgentRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmAgentRecord));

        bool exists = await _repository.CrmAgents.ExistsAsync(
            x => x.AgentName.Trim().ToLower() == record.AgentName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("Agent", "AgentName");

        _logger.LogInformation("Creating new Agent. Time: {Time}", DateTime.UtcNow);

        CrmAgent entity = record.MapTo<CrmAgent>();
        int newId = await _repository.CrmAgents.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Agent created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmAgentDto>() with { AgentId = newId };
    }

    /// <summary>Updates an existing agent record.</summary>
    public async Task<CrmAgentDto> UpdateAsync(UpdateCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmAgentRecord));

        _ = await _repository.CrmAgents
            .FirstOrDefaultAsync(x => x.AgentId == record.AgentId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Agent", "AgentId", record.AgentId.ToString());

        bool duplicateExists = await _repository.CrmAgents.ExistsAsync(
            x => x.AgentName.Trim().ToLower() == record.AgentName.Trim().ToLower()
                 && x.AgentId != record.AgentId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("Agent", "AgentName");

        _logger.LogInformation("Updating Agent. ID: {Id}, Time: {Time}", record.AgentId, DateTime.UtcNow);

        CrmAgent entity = record.MapTo<CrmAgent>();
        _repository.CrmAgents.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Agent updated successfully. ID: {Id}, Time: {Time}", record.AgentId, DateTime.UtcNow);
        return entity.MapTo<CrmAgentDto>();
    }

    /// <summary>Deletes an agent record.</summary>
    public async Task DeleteAsync(DeleteCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AgentId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmAgents
            .FirstOrDefaultAsync(x => x.AgentId == record.AgentId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Agent", "AgentId", record.AgentId.ToString());

        _logger.LogInformation("Deleting Agent. ID: {Id}, Time: {Time}", record.AgentId, DateTime.UtcNow);
        await _repository.CrmAgents.DeleteAsync(x => x.AgentId == record.AgentId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("Agent deleted successfully. ID: {Id}, Time: {Time}", record.AgentId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single agent record by ID.</summary>
    public async Task<CrmAgentDto> AgentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Agent. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmAgents
            .FirstOrDefaultAsync(x => x.AgentId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Agent", "AgentId", id.ToString());
        return entity.MapTo<CrmAgentDto>();
    }

    /// <summary>Retrieves all agent records.</summary>
    public async Task<IEnumerable<CrmAgentDto>> AgentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all Agents. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmAgents.CrmAgentsAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Agents found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmAgentDto>();
        }
        _logger.LogInformation("Agents fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmAgentDto>();
    }

    /// <summary>Retrieves a lightweight list of agents for dropdown binding.</summary>
    public async Task<IEnumerable<CrmAgentDto>> AgentForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Agents for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmAgents.CrmAgentsAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Agents found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmAgentDto>();
        }
        return entities.MapToList<CrmAgentDto>();
    }

    /// <summary>Retrieves a paginated summary grid of agents.</summary>
    public async Task<GridEntity<CrmAgentDto>> AgentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Agents summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT AgentId, AgentName, AgentCode, AgentTypeId, ContactPerson, Email, Phone, Address, Country, CommissionRate, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmAgent";
        const string orderBy = "AgentName ASC";
        return await _repository.CrmAgents.AdoGridDataAsync<CrmAgentDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }

    public async Task<CrmAgentPerformanceDto> PerformanceAsync(int agentId, CancellationToken cancellationToken = default)
    {
        const string sql = @"SELECT TOP 1
            a.AgentId,
            a.AgentName,
            CAST(ISNULL(a.CommissionRate, 0) AS decimal(18,2)) AS CommissionRate,
            (SELECT COUNT(1) FROM CrmAgentLead al WHERE al.AgentId = a.AgentId AND al.IsActive = 1) AS AssignedLeadCount,
            (SELECT COUNT(1) FROM CrmCommission c WHERE c.AgentId = a.AgentId AND c.IsDeleted = 0) AS TotalCommissions,
            (SELECT COUNT(1) FROM CrmCommission c INNER JOIN CrmApplication app ON app.ApplicationId = c.ApplicationId WHERE c.AgentId = a.AgentId AND c.IsDeleted = 0 AND app.Status = 9) AS EnrolledStudentCount,
            (SELECT COUNT(1) FROM CrmCommission c WHERE c.AgentId = a.AgentId AND c.IsDeleted = 0 AND c.Status IN (1, 2, 3)) AS PendingCommissions,
            CAST((SELECT ISNULL(SUM(c.NetAmount), 0) FROM CrmCommission c WHERE c.AgentId = a.AgentId AND c.IsDeleted = 0) AS decimal(18,2)) AS TotalNetAmount,
            CAST((SELECT ISNULL(SUM(ISNULL(c.PaidAmount, 0)), 0) FROM CrmCommission c WHERE c.AgentId = a.AgentId AND c.IsDeleted = 0) AS decimal(18,2)) AS TotalPaidAmount,
            CAST((SELECT ISNULL(SUM(c.NetAmount - ISNULL(c.PaidAmount, 0)), 0) FROM CrmCommission c WHERE c.AgentId = a.AgentId AND c.IsDeleted = 0) AS decimal(18,2)) AS OutstandingAmount
        FROM CrmAgent a
        WHERE a.AgentId = @AgentId";
        return await _repository.CrmAgents.AdoExecuteSingleDataAsync<CrmAgentPerformanceDto>(sql, [new SqlParameter("@AgentId", agentId)], cancellationToken)
            ?? throw new NotFoundException("CrmAgent", "AgentId", agentId.ToString());
    }
}
