using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmAgentRepository : RepositoryBase<CrmAgent>, ICrmAgentRepository
{
    public CrmAgentRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmAgent>> AgentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(x => x.AgentId, trackChanges, cancellationToken);
    }

    public async Task<CrmAgent?> AgentAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(x => x.AgentId == id, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CrmAgent>> AgentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByIdsAsync(x => ids.Contains(x.AgentId), trackChanges, cancellationToken);
    }

    public async Task<CrmAgent> CreateCrmAgentAsync(CrmAgent entity, CancellationToken cancellationToken = default)
    {
        var newId = await CreateAndIdAsync(entity, cancellationToken);
        entity.AgentId = newId;
        return entity;
    }

    public void UpdateCrmAgent(CrmAgent entity) => UpdateByState(entity);

    public async Task DeleteCrmAgentAsync(CrmAgent entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        await DeleteAsync(x => x.AgentId == entity.AgentId, trackChanges, cancellationToken);
    }
}
