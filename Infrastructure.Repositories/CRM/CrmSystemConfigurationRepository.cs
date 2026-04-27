using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.CRM;

public class CrmSystemConfigurationRepository : RepositoryBase<CrmSystemConfiguration>, ICrmSystemConfigurationRepository
{
    public CrmSystemConfigurationRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmSystemConfiguration>> CrmSystemConfigurationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.ConfigKey, trackChanges, cancellationToken);

    public async Task<CrmSystemConfiguration?> CrmSystemConfigurationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.ConfigId == id, trackChanges, cancellationToken);

    public async Task<CrmSystemConfiguration?> CrmSystemConfigurationByKeyAsync(string key, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.ConfigKey == key, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmSystemConfiguration>> CrmSystemConfigurationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.ConfigId), trackChanges, cancellationToken);
}
