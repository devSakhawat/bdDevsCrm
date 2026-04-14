using bdDevCRM.Entities.Entities.CRM;
using bdDevCRM.RepositoriesContracts.CRM;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmOthersInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmOthersInformationRepository : RepositoryBase<CrmOthersInformation>, ICrmOthersInformationRepository
{
	public CrmOthersInformationRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmOthersInformation records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmOthersInformation>> CrmOthersInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.OthersInformationId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmOthersInformation record by ID asynchronously.
	/// </summary>
	public async Task<CrmOthersInformation?> CrmOthersInformationAsync(int OthersInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.OthersInformationId.Equals(OthersInformationId),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmOthersInformation records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmOthersInformation>> CrmOthersInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.OthersInformationId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmOthersInformation records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmOthersInformation>> CrmOthersInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmOthersInformation WHERE ParentId = {parentId} ORDER BY OthersInformationId";
		return await AdoExecuteListQueryAsync<CrmOthersInformation>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmOthersInformation record.
	/// </summary>
	public async Task<CrmOthersInformation> CreateCrmOthersInformationAsync(CrmOthersInformation entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.OthersInformationId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmOthersInformation record.
	/// </summary>
	public void UpdateCrmOthersInformation(CrmOthersInformation entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmOthersInformation record.
	/// </summary>
	public async Task DeleteCrmOthersInformationAsync(CrmOthersInformation entity, bool trackChanges, CancellationToken cancellationToken = default) 
		=> await DeleteAsync(x=> x.OthersInformationId == entity.OthersInformationId, trackChanges, cancellationToken);
}
