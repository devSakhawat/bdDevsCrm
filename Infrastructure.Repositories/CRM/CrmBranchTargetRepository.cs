using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmBranchTargetRepository : RepositoryBase<CrmBranchTarget>, ICrmBranchTargetRepository
{
    public CrmBranchTargetRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmBranchTarget>> CrmBranchTargetsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.BranchTargetId, trackChanges, cancellationToken);

    public async Task<CrmBranchTarget?> CrmBranchTargetAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.BranchTargetId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmBranchTarget>> CrmBranchTargetsByBranchIdAsync(int branchId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => x.BranchId == branchId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmBranchTarget>> CrmBranchTargetsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.BranchTargetId), trackChanges, cancellationToken);
}
