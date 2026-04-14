using bdDevCRM.Entities.Entities.CRM;
using bdDevCRM.RepositoriesContracts.CRM;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmIntakeMonth data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmIntakeMonthRepository : RepositoryBase<CrmIntakeMonth>, ICrmIntakeMonthRepository
{
	public CrmIntakeMonthRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmIntakeMonth records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeMonth>> CrmIntakeMonthsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.IntakeMonthId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmIntakeMonth record by ID asynchronously.
	/// </summary>
	public async Task<CrmIntakeMonth?> CrmIntakeMonthAsync(int crmintakemonthid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.IntakeMonthId.Equals(crmintakemonthid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmIntakeMonth records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeMonth>> CrmIntakeMonthsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.IntakeMonthId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmIntakeMonth records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmIntakeMonth>> CrmIntakeMonthsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmIntakeMonth WHERE ParentId = {parentId} ORDER BY IntakeMonthId";
		return await AdoExecuteListQueryAsync<CrmIntakeMonth>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmIntakeMonth record.
	/// </summary>
	public async Task<CrmIntakeMonth> CreateCrmIntakeMonthAsync(CrmIntakeMonth entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.IntakeMonthId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmIntakeMonth record.
	/// </summary>
	public void UpdateCrmIntakeMonth(CrmIntakeMonth entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmIntakeMonth record.
	/// </summary>
	public async Task DeleteCrmIntakeMonthAsync(CrmIntakeMonth entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.IntakeMonthId.Equals(entity.IntakeMonthId), trackChanges, cancellationToken);
}
