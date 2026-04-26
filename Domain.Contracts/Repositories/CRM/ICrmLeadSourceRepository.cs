using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmLeadSourceRepository : IRepositoryBase<CrmLeadSource>
{
  Task<IEnumerable<CrmLeadSource>> LeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmLeadSource?> LeadSourceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmLeadSource>> LeadSourcesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmLeadSource> CreateCrmLeadSourceAsync(CrmLeadSource entity, CancellationToken cancellationToken = default);
  void UpdateCrmLeadSource(CrmLeadSource entity);
  Task DeleteCrmLeadSourceAsync(CrmLeadSource entity, bool trackChanges, CancellationToken cancellationToken = default);
}
