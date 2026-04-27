using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCommunicationTypeRepository : IRepositoryBase<CrmCommunicationType>
{
  Task<IEnumerable<CrmCommunicationType>> CommunicationTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmCommunicationType?> CommunicationTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmCommunicationType>> CommunicationTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmCommunicationType> CreateCrmCommunicationTypeAsync(CrmCommunicationType entity, CancellationToken cancellationToken = default);
  void UpdateCrmCommunicationType(CrmCommunicationType entity);
  Task DeleteCrmCommunicationTypeAsync(CrmCommunicationType entity, bool trackChanges, CancellationToken cancellationToken = default);
}
