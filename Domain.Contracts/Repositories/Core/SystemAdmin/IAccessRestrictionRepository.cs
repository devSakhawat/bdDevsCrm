using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IAccessRestrictionRepository : IRepositoryBase<AccessRestriction>
{
    Task<AccessRestriction?> AccessRestrictionAsync(int accessRestrictionId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AccessRestriction>> AccessRestrictionsAsync(bool trackChanges, CancellationToken cancellationToken = default);

	// Additional methods for specific use cases
	Task<IEnumerable<AccessRestriction>> AccessRestrictionsByHrRecordIdAsync(int hrRecordId, CancellationToken cancellationToken = default);
	Task<IEnumerable<Groups>> GroupsByHrRecordIdAsync(int hrRecordId, CancellationToken cancellationToken = default);
	Task<IEnumerable<AccessRestriction>> AccessRestrictionConditionsAsync(int hrRecordId, int type, string groupCondition, CancellationToken cancellationToken = default);
	Task<string> GenerateAccessRestrictionConditionAsync(int hrRecordId, CancellationToken cancellationToken = default);
}