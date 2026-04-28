using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmDegreeLevelRepository : RepositoryBase<CrmDegreeLevel>, ICrmDegreeLevelRepository
{
    public CrmDegreeLevelRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmDegreeLevel>> CrmDegreeLevelsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.SortOrder, trackChanges, cancellationToken);

    public async Task<CrmDegreeLevel?> CrmDegreeLevelAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.DegreeLevelId == id, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmDegreeLevel>> CrmDegreeLevelsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.DegreeLevelId), trackChanges, cancellationToken);
}
