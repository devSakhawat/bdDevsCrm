using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmAgentLeadService
{
    Task<CrmAgentLeadDto> CreateAsync(CreateCrmAgentLeadRecord record, CancellationToken cancellationToken = default);
    Task<CrmAgentLeadDto> UpdateAsync(UpdateCrmAgentLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmAgentLeadRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmAgentLeadDto> AgentLeadAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmAgentLeadDto?> AgentLeadByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgentLeadDto>> AgentLeadsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgentLeadDto>> AgentLeadsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmAgentLeadDto>> AgentLeadsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
