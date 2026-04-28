using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCommissionRepository : IRepositoryBase<CrmCommission>
{
    Task<IEnumerable<CrmCommission>> CommissionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommission?> CommissionAsync(int commissionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommission>> CommissionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommission>> CommissionsByAgentIdAsync(int agentId, bool trackChanges, CancellationToken cancellationToken = default);
}
