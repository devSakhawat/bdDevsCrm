using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmIntakeYear data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmIntakeYearRepository : RepositoryBase<CrmIntakeYear>, ICrmIntakeYearRepository
{
	public CrmIntakeYearRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmIntakeYear records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeYear>> CrmIntakeYearsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.IntakeYearId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmIntakeYear record by ID asynchronously.
	/// </summary>
	public async Task<CrmIntakeYear?> CrmIntakeYearAsync(int crmintakeyearid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.IntakeYearId.Equals(crmintakeyearid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmIntakeYear records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeYear>> CrmIntakeYearsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.IntakeYearId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmIntakeYear records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeYear>> CrmIntakeYearsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmIntakeYear WHERE ParentId = {parentId} ORDER BY IntakeYearId";
		return await AdoExecuteListQueryAsync<CrmIntakeYear>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmIntakeYear record.
	/// </summary>
	public async Task<CrmIntakeYear> CreateCrmIntakeYearAsync(CrmIntakeYear entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.IntakeYearId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmIntakeYear record.
	/// </summary>
	public void UpdateCrmIntakeYear(CrmIntakeYear entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmIntakeYear record.
	/// </summary>
	public async Task DeleteCrmIntakeYearAsync(CrmIntakeYear entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.IntakeYearId == entity.IntakeYearId, trackChanges, cancellationToken);
}
