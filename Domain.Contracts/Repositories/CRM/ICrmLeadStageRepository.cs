using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmLeadStageRepository : IRepositoryBase<CrmLeadStage>
{
  Task<IEnumerable<CrmLeadStage>> LeadStagesAsync(bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmLeadStage?> LeadStageAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<CrmLeadStage>> LeadStagesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CrmLeadStage> CreateCrmLeadStageAsync(CrmLeadStage entity, CancellationToken cancellationToken = default);
  void UpdateCrmLeadStage(CrmLeadStage entity);
  Task DeleteCrmLeadStageAsync(CrmLeadStage entity, bool trackChanges, CancellationToken cancellationToken = default);
}
