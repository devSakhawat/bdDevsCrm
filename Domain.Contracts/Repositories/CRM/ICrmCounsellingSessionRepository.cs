using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCounsellingSessionRepository : IRepositoryBase<CrmCounsellingSession>
{
    Task<IEnumerable<CrmCounsellingSession>> CounsellingSessionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCounsellingSession?> CrmCounsellingSessionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCounsellingSession>> CounsellingSessionsByLeadIdAsync(int leadId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCounsellingSession>> CounsellingSessionsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
