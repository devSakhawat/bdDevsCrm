using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmVisaStatusRepository : IRepositoryBase<CrmVisaStatus>
{
  Task<IEnumerable<CrmVisaStatus>> VisaStatusesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmVisaStatus?> VisaStatusAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmVisaStatus>> VisaStatusesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmVisaStatus> CreateCrmVisaStatusAsync(CrmVisaStatus entity, CancellationToken cancellationToken = default);
  void UpdateCrmVisaStatus(CrmVisaStatus entity);
  Task DeleteCrmVisaStatusAsync(CrmVisaStatus entity, bool trackChanges, CancellationToken cancellationToken = default);
}
