using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM agent type management operations.</summary>
public interface ICrmAgentTypeService
{
    /// <summary>Creates a new agent type record.</summary>
    Task<CrmAgentTypeDto> CreateAsync(CreateCrmAgentTypeRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing agent type record.</summary>
    Task<CrmAgentTypeDto> UpdateAsync(UpdateCrmAgentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes an agent type record.</summary>
    Task DeleteAsync(DeleteCrmAgentTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single agent type record by ID.</summary>
    Task<CrmAgentTypeDto> AgentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all agent type records.</summary>
    Task<IEnumerable<CrmAgentTypeDto>> AgentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmAgentTypeDto>> AgentTypeForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of agent types.</summary>
    Task<GridEntity<CrmAgentTypeDto>> AgentTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
