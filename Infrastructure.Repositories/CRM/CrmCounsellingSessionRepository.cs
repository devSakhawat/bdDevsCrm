using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCounsellingSessionRepository : RepositoryBase<CrmCounsellingSession>, ICrmCounsellingSessionRepository
{
    public CrmCounsellingSessionRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCounsellingSession>> CounsellingSessionsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.CounsellingSessionId, trackChanges, cancellationToken);

    public async Task<CrmCounsellingSession?> CrmCounsellingSessionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.CounsellingSessionId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmCounsellingSession>> CounsellingSessionsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.LeadId == leadId, x => x.CounsellingSessionId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmCounsellingSession>> CounsellingSessionsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.CounsellingSessionId), trackChanges, cancellationToken);
}
