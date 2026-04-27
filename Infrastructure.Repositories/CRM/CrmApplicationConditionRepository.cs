using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmApplicationConditionRepository : RepositoryBase<CrmApplicationCondition>, ICrmApplicationConditionRepository
{
    public CrmApplicationConditionRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmApplicationCondition>> ApplicationConditionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.ApplicationConditionId, trackChanges, cancellationToken);

    public async Task<CrmApplicationCondition?> ApplicationConditionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.ApplicationConditionId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmApplicationCondition>> ApplicationConditionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.ApplicationId == applicationId, x => x.ApplicationConditionId, trackChanges, false, cancellationToken);
}
