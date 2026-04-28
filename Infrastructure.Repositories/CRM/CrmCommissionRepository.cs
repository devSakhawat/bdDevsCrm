using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCommissionRepository : RepositoryBase<CrmCommission>, ICrmCommissionRepository
{
    public CrmCommissionRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCommission>> CommissionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.CommissionId, trackChanges, cancellationToken);

    public async Task<CrmCommission?> CommissionAsync(int commissionId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.CommissionId == commissionId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmCommission>> CommissionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.ApplicationId == applicationId, x => x.CommissionId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmCommission>> CommissionsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.AgentId == agentId, x => x.CommissionId, trackChanges, false, cancellationToken);
}
