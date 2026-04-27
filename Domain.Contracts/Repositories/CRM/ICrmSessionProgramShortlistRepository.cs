using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmSessionProgramShortlistRepository : IRepositoryBase<CrmSessionProgramShortlist>
{
    Task<IEnumerable<CrmSessionProgramShortlist>> SessionProgramShortlistsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmSessionProgramShortlist?> CrmSessionProgramShortlistAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmSessionProgramShortlist>> SessionProgramShortlistsBySessionIdAsync(int sessionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmSessionProgramShortlist>> SessionProgramShortlistsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
