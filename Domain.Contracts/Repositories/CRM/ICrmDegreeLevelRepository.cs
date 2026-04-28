using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmDegreeLevelRepository : IRepositoryBase<CrmDegreeLevel>
{
    Task<IEnumerable<CrmDegreeLevel>> CrmDegreeLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmDegreeLevel?> CrmDegreeLevelAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmDegreeLevel>> CrmDegreeLevelsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
