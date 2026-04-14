using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmInstitute data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmInstituteRepository : RepositoryBase<CrmInstitute>, ICrmInstituteRepository
{
	public CrmInstituteRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmInstitute records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmInstitute>> CrmInstitutesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.InstituteId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmInstitute record by ID asynchronously.
	/// </summary>
	public async Task<CrmInstitute?> CrmInstituteAsync(int crminstituteid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.InstituteId.Equals(crminstituteid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmInstitute records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmInstitute>> CrmInstitutesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.InstituteId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmInstitute records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmInstitute>> CrmInstitutesByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmInstitute WHERE ParentId = {parentId} ORDER BY InstituteId";
		return await AdoExecuteListQueryAsync<CrmInstitute>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmInstitute record.
	/// </summary>
	public async Task<CrmInstitute> CreateCrmInstituteAsync(CrmInstitute entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.InstituteId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmInstitute record.
	/// </summary>
	public void UpdateCrmInstitute(CrmInstitute entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmInstitute record.
	/// </summary>
	public async Task DeleteCrmInstituteAsync(CrmInstitute entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.InstituteId.Equals(entity.InstituteId), trackChanges, cancellationToken);
}
