using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmAgentRepository : IRepositoryBase<CrmAgent>
{
    Task<IEnumerable<CrmAgent>> AgentsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmAgent?> AgentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmAgent>> AgentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmAgent> CreateCrmAgentAsync(CrmAgent entity, CancellationToken cancellationToken = default);
    void UpdateCrmAgent(CrmAgent entity);
    Task DeleteCrmAgentAsync(CrmAgent entity, bool trackChanges, CancellationToken cancellationToken = default);
}
