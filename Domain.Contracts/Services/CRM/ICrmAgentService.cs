using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM agent management operations.</summary>
public interface ICrmAgentService
{
    /// <summary>Creates a new agent record.</summary>
    Task<CrmAgentDto> CreateAsync(CreateCrmAgentRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing agent record.</summary>
    Task<CrmAgentDto> UpdateAsync(UpdateCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes an agent record.</summary>
    Task DeleteAsync(DeleteCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single agent record by ID.</summary>
    Task<CrmAgentDto> AgentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all agent records.</summary>
    Task<IEnumerable<CrmAgentDto>> AgentsAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmAgentDto>> AgentForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of agents.</summary>
    Task<GridEntity<CrmAgentDto>> AgentsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
