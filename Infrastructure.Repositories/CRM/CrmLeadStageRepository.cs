using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmLeadStageRepository : RepositoryBase<CrmLeadStage>, ICrmLeadStageRepository
{
  public CrmLeadStageRepository(CrmContext context) : base(context) { }

  public async Task<IEnumerable<CrmLeadStage>> LeadStagesAsync(bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListAsync(x => x.LeadStageId, trackChanges, cancellationToken);
  }

  public async Task<CrmLeadStage?> LeadStageAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await FirstOrDefaultAsync(x => x.LeadStageId == id, trackChanges, cancellationToken);
  }

  public async Task<IEnumerable<CrmLeadStage>> LeadStagesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
  {
    return await ListByIdsAsync(x => ids.Contains(x.LeadStageId), trackChanges, cancellationToken);
  }

  public async Task<CrmLeadStage> CreateCrmLeadStageAsync(CrmLeadStage entity, CancellationToken cancellationToken = default)
  {
    var newId = await CreateAndIdAsync(entity, cancellationToken);
    entity.LeadStageId = newId;
    return entity;
  }

  public void UpdateCrmLeadStage(CrmLeadStage entity) => UpdateByState(entity);

  public async Task DeleteCrmLeadStageAsync(CrmLeadStage entity, bool trackChanges, CancellationToken cancellationToken = default)
  {
    await DeleteAsync(x => x.LeadStageId == entity.LeadStageId, trackChanges, cancellationToken);
  }
}
