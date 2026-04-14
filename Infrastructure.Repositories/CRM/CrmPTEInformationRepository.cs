using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmPTEInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmPTEInformationRepository : RepositoryBase<CrmPTEInformation>, ICrmPTEInformationRepository
{
	public CrmPTEInformationRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmPTEInformation records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPTEInformation>> CrmPTEInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.PTEInformationId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmPTEInformation record by ID asynchronously.
	/// </summary>
	public async Task<CrmPTEInformation?> CrmPTEInformationAsync(int PTEInformationId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.PTEInformationId.Equals(PTEInformationId),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPTEInformation records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPTEInformation>> CrmPTEInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.PTEInformationId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPTEInformation records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPTEInformation>> CrmPTEInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmPTEInformation WHERE ParentId = {parentId} ORDER BY PTEInformationId";
		return await AdoExecuteListQueryAsync<CrmPTEInformation>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmPTEInformation record.
	/// </summary>
	public async Task<CrmPTEInformation> CreateCrmPTEInformationAsync(CrmPTEInformation entity, CancellationToken cancellationToken = default)
	{ 
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.PTEInformationId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmPTEInformation record.
	/// </summary>
	public void UpdateCrmPTEInformation(CrmPTEInformation entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmPTEInformation record.
	/// </summary>
	public async Task DeleteCrmPTEInformationAsync(CrmPTEInformation entity, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.PTEInformationId == entity.PTEInformationId, trackChanges, cancellationToken);
}
