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

/// <summary>
/// CrmAgentType service implementing business logic for agent type management.
/// </summary>
internal sealed class CrmAgentTypeService : ICrmAgentTypeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmAgentTypeService> _logger;
    private readonly IConfiguration _config;

    public CrmAgentTypeService(IRepositoryManager repository, ILogger<CrmAgentTypeService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new agent type record.</summary>
    public async Task<CrmAgentTypeDto> CreateAsync(CreateCrmAgentTypeRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmAgentTypeRecord));

        _logger.LogInformation("Creating new agent type. AgentTypeName: {AgentTypeName}, Time: {Time}", record.AgentTypeName, DateTime.UtcNow);

        bool exists = await _repository.CrmAgentTypes.ExistsAsync(
            x => x.AgentTypeName.Trim().ToLower() == record.AgentTypeName.Trim().ToLower(),
            cancellationToken: cancellationToken);

        if (exists)
            throw new DuplicateRecordException("AgentType", "AgentTypeName");

        CrmAgentType entity = record.MapTo<CrmAgentType>();
        int newId = await _repository.CrmAgentTypes.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Agent type created successfully. ID: {AgentTypeId}, Time: {Time}", newId, DateTime.UtcNow);

        return entity.MapTo<CrmAgentTypeDto>() with { AgentTypeId = newId };
    }

    /// <summary>Updates an existing agent type record.</summary>
    public async Task<CrmAgentTypeDto> UpdateAsync(UpdateCrmAgentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmAgentTypeRecord));

        _logger.LogInformation("Updating agent type. ID: {AgentTypeId}, Time: {Time}", record.AgentTypeId, DateTime.UtcNow);

        _ = await _repository.CrmAgentTypes
            .FirstOrDefaultAsync(x => x.AgentTypeId == record.AgentTypeId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("AgentType", "AgentTypeId", record.AgentTypeId.ToString());

        bool duplicateExists = await _repository.CrmAgentTypes.ExistsAsync(
            x => x.AgentTypeName.Trim().ToLower() == record.AgentTypeName.Trim().ToLower()
                 && x.AgentTypeId != record.AgentTypeId,
            cancellationToken: cancellationToken);

        if (duplicateExists)
            throw new DuplicateRecordException("AgentType", "AgentTypeName");

        CrmAgentType entity = record.MapTo<CrmAgentType>();
        _repository.CrmAgentTypes.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Agent type updated successfully. ID: {AgentTypeId}, Time: {Time}", record.AgentTypeId, DateTime.UtcNow);

        return entity.MapTo<CrmAgentTypeDto>();
    }

    /// <summary>Deletes an agent type record.</summary>
    public async Task DeleteAsync(DeleteCrmAgentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.AgentTypeId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _logger.LogInformation("Deleting agent type. ID: {AgentTypeId}, Time: {Time}", record.AgentTypeId, DateTime.UtcNow);

        _ = await _repository.CrmAgentTypes
            .FirstOrDefaultAsync(x => x.AgentTypeId == record.AgentTypeId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AgentType", "AgentTypeId", record.AgentTypeId.ToString());

        await _repository.CrmAgentTypes.DeleteAsync(x => x.AgentTypeId == record.AgentTypeId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogWarning("Agent type deleted successfully. ID: {AgentTypeId}, Time: {Time}", record.AgentTypeId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single agent type record by ID.</summary>
    public async Task<CrmAgentTypeDto> AgentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching agent type. ID: {AgentTypeId}, Time: {Time}", id, DateTime.UtcNow);

        var entity = await _repository.CrmAgentTypes
            .FirstOrDefaultAsync(x => x.AgentTypeId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("AgentType", "AgentTypeId", id.ToString());

        return entity.MapTo<CrmAgentTypeDto>();
    }

    /// <summary>Retrieves all agent type records.</summary>
    public async Task<IEnumerable<CrmAgentTypeDto>> AgentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all agent types. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmAgentTypes.CrmAgentTypesAsync(trackChanges, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No agent types found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmAgentTypeDto>();
        }

        _logger.LogInformation("Agent types fetched successfully. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmAgentTypeDto>();
    }

    /// <summary>Retrieves a lightweight list of agent types for dropdown binding.</summary>
    public async Task<IEnumerable<CrmAgentTypeDto>> AgentTypeForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching agent types for DDL. Time: {Time}", DateTime.UtcNow);

        var entities = await _repository.CrmAgentTypes.CrmAgentTypesAsync(false, cancellationToken);

        if (!entities.Any())
        {
            _logger.LogWarning("No agent types found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmAgentTypeDto>();
        }

        return entities.MapToList<CrmAgentTypeDto>();
    }

    /// <summary>Retrieves a paginated summary grid of agent types.</summary>
    public async Task<GridEntity<CrmAgentTypeDto>> AgentTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching agent types summary grid. Time: {Time}", DateTime.UtcNow);

        const string sql = @"SELECT AgentTypeId, AgentTypeName, Description, IsActive, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmAgentType";
        const string orderBy = "AgentTypeName ASC";

        return await _repository.CrmAgentTypes.AdoGridDataAsync<CrmAgentTypeDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
