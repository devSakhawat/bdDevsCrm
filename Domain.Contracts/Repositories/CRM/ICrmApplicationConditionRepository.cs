using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmApplicationConditionRepository : IRepositoryBase<CrmApplicationCondition>
{
    Task<IEnumerable<CrmApplicationCondition>> ApplicationConditionsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmApplicationCondition?> ApplicationConditionAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmApplicationCondition>> ApplicationConditionsByApplicationIdAsync(int applicationId, bool trackChanges, CancellationToken cancellationToken = default);
}
