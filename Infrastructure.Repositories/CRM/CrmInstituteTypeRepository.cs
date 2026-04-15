using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmInstituteType data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmInstituteTypeRepository : RepositoryBase<CrmInstituteType>, ICrmInstituteTypeRepository
{
	public CrmInstituteTypeRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmInstituteType records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteType>> CrmInstituteTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.InstituteTypeId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmInstituteType record by ID asynchronously.
	/// </summary>
	public async Task<CrmInstituteType?> CrmInstituteTypeAsync(int crminstitutetypeid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.InstituteTypeId.Equals(crminstitutetypeid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmInstituteType records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteType>> CrmInstituteTypesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.InstituteTypeId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmInstituteType records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmInstituteType>> CrmInstituteTypesByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmInstituteType WHERE ParentId = {parentId} ORDER BY InstituteTypeId";
		return await AdoExecuteListQueryAsync<CrmInstituteType>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmInstituteType record.
	/// </summary>
	public async Task<CrmInstituteType> CreateCrmInstituteTypeAsync(CrmInstituteType entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.InstituteTypeId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmInstituteType record.
	/// </summary>
	public void UpdateCrmInstituteType(CrmInstituteType entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmInstituteType record.
	/// </summary>
	public async Task DeleteCrmInstituteTypeAsync(CrmInstituteType entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.InstituteTypeId.Equals(entity.InstituteTypeId), trackChanges, cancellationToken);
}
