using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCommunicationTypeRepository : RepositoryBase<CrmCommunicationType>, ICrmCommunicationTypeRepository
{
  public CrmCommunicationTypeRepository(CrmContext context) : base(context) { }

  public async Task<IEnumerable<CrmCommunicationType>> CommunicationTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.CommunicationTypeId, trackChanges, cancellationToken);
  }

  public async Task<CrmCommunicationType?> CommunicationTypeAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.CommunicationTypeId == id, trackChanges, cancellationToken);
  }

  public async Task<IEnumerable<CrmCommunicationType>> CommunicationTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.CommunicationTypeId), trackChanges, cancellationToken);
  }

  public async Task<CrmCommunicationType> CreateCrmCommunicationTypeAsync(CrmCommunicationType entity, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.CommunicationTypeId = newId;
    return entity;
  }

  public void UpdateCrmCommunicationType(CrmCommunicationType entity) => UpdateByState(entity);

  public async Task DeleteCrmCommunicationTypeAsync(CrmCommunicationType entity, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.CommunicationTypeId == entity.CommunicationTypeId, trackChanges, cancellationToken);
  }
}
