using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmDocumentTypeRepository : IRepositoryBase<CrmDocumentType>
{
  Task<IEnumerable<CrmDocumentType>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmDocumentType?> DocumentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmDocumentType>> DocumentTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmDocumentType> CreateCrmDocumentTypeAsync(CrmDocumentType entity, CancellationToken cancellationToken = default);
  void UpdateCrmDocumentType(CrmDocumentType entity);
  Task DeleteCrmDocumentTypeAsync(CrmDocumentType entity, bool trackChanges, CancellationToken cancellationToken = default);
}
