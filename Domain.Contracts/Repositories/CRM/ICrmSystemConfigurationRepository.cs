using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmSystemConfigurationRepository : IRepositoryBase<CrmSystemConfiguration>
{
    Task<IEnumerable<CrmSystemConfiguration>> CrmSystemConfigurationsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmSystemConfiguration?> CrmSystemConfigurationAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmSystemConfiguration?> CrmSystemConfigurationByKeyAsync(string key, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmSystemConfiguration>> CrmSystemConfigurationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
