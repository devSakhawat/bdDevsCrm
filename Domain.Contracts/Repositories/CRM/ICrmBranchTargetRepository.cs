using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmBranchTargetRepository : IRepositoryBase<CrmBranchTarget>
{
    Task<IEnumerable<CrmBranchTarget>> CrmBranchTargetsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmBranchTarget?> CrmBranchTargetAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmBranchTarget>> CrmBranchTargetsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmBranchTarget>> CrmBranchTargetsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
