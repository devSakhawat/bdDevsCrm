using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmEducationHistory data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmEducationHistoryRepository : RepositoryBase<CrmEducationHistory>, ICrmEducationHistoryRepository
{
	public CrmEducationHistoryRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmEducationHistory records by ApplicantId asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmEducationHistory>> CrmEducationHistorysByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByConditionAsync(expression: x => x.ApplicantId == applicantId, orderBy: x => x.EducationHistoryId, trackChanges ,descending: false, cancellationToken);
	}

	/// <summary>
	/// Retrieves all CrmEducationHistory records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmEducationHistory>> CrmEducationHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.EducationHistoryId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmEducationHistory record by ID asynchronously.
	/// </summary>
	public async Task<CrmEducationHistory?> CrmEducationHistoryAsync(int crmeducationhistoryid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.EducationHistoryId.Equals(crmeducationhistoryid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmEducationHistory records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmEducationHistory>> CrmEducationHistorysByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.EducationHistoryId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmEducationHistory records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmEducationHistory>> CrmEducationHistorysByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmEducationHistory WHERE ParentId = {parentId} ORDER BY EducationHistoryId";
		return await AdoExecuteListQueryAsync<CrmEducationHistory>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmEducationHistory record.
	/// </summary>
	public async Task<CrmEducationHistory> CreateCrmEducationHistoryAsync(CrmEducationHistory entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.EducationHistoryId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmEducationHistory record.
	/// </summary>
	public void UpdateCrmEducationHistory(CrmEducationHistory entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmEducationHistory record.
	/// </summary>
	public async Task DeleteCrmEducationHistoryAsync(CrmEducationHistory entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.EducationHistoryId.Equals(entity.EducationHistoryId), trackChanges, cancellationToken);
}
