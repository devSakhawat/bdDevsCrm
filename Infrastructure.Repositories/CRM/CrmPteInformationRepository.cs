using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmPteInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmPteInformationRepository : RepositoryBase<CrmPteInformation>, ICrmPteInformationRepository
{
	public CrmPteInformationRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmPteInformation records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPteInformation>> CrmPteInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.PTEInformationId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmPteInformation record by ID asynchronously.
	/// </summary>
	public async Task<CrmPteInformation?> CrmPteInformationAsync(int PTEInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.PTEInformationId.Equals(PTEInformationId),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPteInformation records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPteInformation>> CrmPteInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.PTEInformationId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPteInformation records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPteInformation>> CrmPteInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmPteInformation WHERE ParentId = {parentId} ORDER BY PTEInformationId";
		return await AdoExecuteListQueryAsync<CrmPteInformation>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmPteInformation record.
	/// </summary>
	public async Task<CrmPteInformation> CreateCrmPTEInformationAsync(CrmPteInformation entity, CancellationToken cancellationToken = default)
	{ 
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.PTEInformationId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmPteInformation record.
	/// </summary>
	public void UpdateCrmPTEInformation(CrmPteInformation entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmPteInformation record.
	/// </summary>
	public async Task DeleteCrmPTEInformationAsync(CrmPteInformation entity, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.PTEInformationId == entity.PTEInformationId, trackChanges, cancellationToken);
}
