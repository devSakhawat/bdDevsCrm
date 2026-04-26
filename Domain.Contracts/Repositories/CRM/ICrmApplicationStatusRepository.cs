using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmApplicationStatusRepository : IRepositoryBase<CrmApplicationStatus>
{
  Task<IEnumerable<CrmApplicationStatus>> ApplicationStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmApplicationStatus?> ApplicationStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmApplicationStatus>> ApplicationStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmApplicationStatus> CreateCrmApplicationStatusAsync(CrmApplicationStatus entity, CancellationToken cancellationToken = default);
  void UpdateCrmApplicationStatus(CrmApplicationStatus entity);
  Task DeleteCrmApplicationStatusAsync(CrmApplicationStatus entity, bool trackChanges, CancellationToken cancellationToken = default);
}
