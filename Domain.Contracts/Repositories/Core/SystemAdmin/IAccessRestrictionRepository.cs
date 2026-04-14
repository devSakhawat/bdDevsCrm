using Domain.Entities.Entities.System;
using Domain.Contracts.Repositories;

namespace Domain.Contracts.Core.SystemAdmin;

public interface IAccessRestrictionRepository : IRepositoryBase<AccessRestriction>
{
	// Priority 1: Basic Retrieval (Using EF Core for simple queries)
	Task<IEnumerable<AccessRestriction>> AccessRestrictionsAsync(int hrRecordId, CancellationToken cancellationToken = default);

	// Priority 2: Retrieval with Filters / Complex Queries (Using ADO.NET)
	Task<IEnumerable<Groups>> GroupsByHrRecordIdAsync(int hrRecordId, CancellationToken cancellationToken = default);
	Task<IEnumerable<AccessRestriction>> AccessRestrictionsByHrRecordIdAsync(int hrRecordId, string groupCondition, CancellationToken cancellationToken = default);
	Task<IEnumerable<AccessRestriction>> AccessRestrictionConditionsAsync(int hrRecordId, int type, string groupCondition, CancellationToken cancellationToken = default);

	// Priority 3: Business Logic / Generation
	Task<string> GenerateAccessRestrictionConditionAsync(int hrRecordId, CancellationToken cancellationToken = default);


	/// <summary>
	/// Creates a new access Restriction.
	/// </summary>
	Task<AccessRestriction> CreateAccessRestrictionAsync(AccessRestriction accessRestriction, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing access Restriction.
	/// </summary>
	void UpdateAccessRestriction(AccessRestriction accessRestriction);

	/// <summary>
	/// Deletes an access Restriction.
	/// </summary>
	Task DeleteAccessRestrictionAsync(AccessRestriction accessRestriction, bool trackChanges, CancellationToken cancellationToken = default);
}