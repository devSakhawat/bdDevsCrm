using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmApplication data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmApplicationRepository : RepositoryBase<CrmApplication>, ICRMApplicationRepository
{
	public CrmApplicationRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmApplication records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplication>> CrmApplicationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.ApplicationId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmApplication record by ID asynchronously.
	/// </summary>
	public async Task<CrmApplication?> CrmApplicationAsync(int applicationid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(x => x.ApplicationId.Equals(applicationid), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplication records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplication>> CrmApplicationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.ApplicationId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplication records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplication>> CrmApplicationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmApplication WHERE ParentId = {parentId} ORDER BY ApplicationId";
		return await AdoExecuteListQueryAsync<CrmApplication>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmApplication record.
	/// </summary>
	public async Task<CrmApplication> CreateCrmApplicationAsync(CrmApplication entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.ApplicationId = newId;
		return entity;
	}


	/// <summary>
	/// Updates an existing CrmApplication record.
	/// </summary>
	public void UpdateCrmApplication(CrmApplication entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmApplication record.
	/// </summary>
	public async Task DeleteCrmApplicationAsync(CrmApplication entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.ApplicationId.Equals(entity.ApplicationId), trackChanges, cancellationToken);
}
