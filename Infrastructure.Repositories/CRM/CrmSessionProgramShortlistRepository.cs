using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmSessionProgramShortlistRepository : RepositoryBase<CrmSessionProgramShortlist>, ICrmSessionProgramShortlistRepository
{
    public CrmSessionProgramShortlistRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmSessionProgramShortlist>> SessionProgramShortlistsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.SessionProgramShortlistId, trackChanges, cancellationToken);

    public async Task<CrmSessionProgramShortlist?> CrmSessionProgramShortlistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.SessionProgramShortlistId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmSessionProgramShortlist>> SessionProgramShortlistsBySessionIdAsync(int sessionId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.SessionId == sessionId, x => x.SessionProgramShortlistId, trackChanges, false, cancellationToken);

    public async Task<IEnumerable<CrmSessionProgramShortlist>> SessionProgramShortlistsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.SessionProgramShortlistId), trackChanges, cancellationToken);
}
