using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmLeadSourceRepository : RepositoryBase<CrmLeadSource>, ICrmLeadSourceRepository
{
  public CrmLeadSourceRepository(CrmContext context) : base(context) { }

  public async Task<IEnumerable<CrmLeadSource>> LeadSourcesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.LeadSourceId, trackChanges, cancellationToken);
  }

  public async Task<CrmLeadSource?> LeadSourceAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.LeadSourceId == id, trackChanges, cancellationToken);
  }

  public async Task<IEnumerable<CrmLeadSource>> LeadSourcesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.LeadSourceId), trackChanges, cancellationToken);
  }

  public async Task<CrmLeadSource> CreateCrmLeadSourceAsync(CrmLeadSource entity, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.LeadSourceId = newId;
    return entity;
  }

  public void UpdateCrmLeadSource(CrmLeadSource entity) => UpdateByState(entity);

  public async Task DeleteCrmLeadSourceAsync(CrmLeadSource entity, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.LeadSourceId == entity.LeadSourceId, trackChanges, cancellationToken);
  }
}
