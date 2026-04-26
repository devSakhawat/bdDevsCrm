using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmDocumentTypeRepository : RepositoryBase<CrmDocumentType>, ICrmDocumentTypeRepository
{
  public CrmDocumentTypeRepository(CrmContext context) : base(context) { }

  public async Task<IEnumerable<CrmDocumentType>> DocumentTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.DocumentTypeId, trackChanges, cancellationToken);
  }

  public async Task<CrmDocumentType?> DocumentTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.DocumentTypeId == id, trackChanges, cancellationToken);
  }

  public async Task<IEnumerable<CrmDocumentType>> DocumentTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.DocumentTypeId), trackChanges, cancellationToken);
  }

  public async Task<CrmDocumentType> CreateCrmDocumentTypeAsync(CrmDocumentType entity, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.DocumentTypeId = newId;
    return entity;
  }

  public void UpdateCrmDocumentType(CrmDocumentType entity) => UpdateByState(entity);

  public async Task DeleteCrmDocumentTypeAsync(CrmDocumentType entity, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.DocumentTypeId == entity.DocumentTypeId, trackChanges, cancellationToken);
  }
}
