using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmAgentService
{
    Task<CrmAgentDto> CreateAsync(CreateCrmAgentRecord record, CancellationToken cancellationToken = default);
    Task<CrmAgentDto> UpdateAsync(UpdateCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmAgentRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmAgentDto> AgentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgentDto>> AgentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgentDDLDto>> AgentForDDLAsync(CancellationToken cancellationToken = default);
    Task<GridEntity<CrmAgentDto>> AgentSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
