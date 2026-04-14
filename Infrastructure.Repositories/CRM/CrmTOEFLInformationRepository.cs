using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmTOEFLInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmTOEFLInformationRepository : RepositoryBase<CrmTOEFLInformation>, ICrmTOEFLInformationRepository
{
	public CrmTOEFLInformationRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmTOEFLInformation records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmTOEFLInformation>> CrmTOEFLInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.TOEFLInformationId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmTOEFLInformation record by ID asynchronously.
	/// </summary>
	public async Task<CrmTOEFLInformation?> CrmTOEFLInformationAsync(int crmtoeflinformationid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.TOEFLInformationId.Equals(crmtoeflinformationid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmTOEFLInformation records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmTOEFLInformation>> CrmTOEFLInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.TOEFLInformationId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmTOEFLInformation records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmTOEFLInformation>> CrmTOEFLInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmTOEFLInformation WHERE ParentId = {parentId} ORDER BY TOEFLInformationId";
		return await AdoExecuteListQueryAsync<CrmTOEFLInformation>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmTOEFLInformation record.
	/// </summary>
	public async Task<CrmTOEFLInformation> CreateCrmTOEFLInformationAsync(CrmTOEFLInformation entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.TOEFLInformationId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmTOEFLInformation record.
	/// </summary>
	public void UpdateCrmTOEFLInformation(CrmTOEFLInformation entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmTOEFLInformation record.
	/// </summary>
	public async Task DeleteCrmTOEFLInformationAsync(CrmTOEFLInformation entity, bool trackChanges, CancellationToken cancellationToken = default)
	{
		await DeleteAsync(x => x.TOEFLInformationId == entity.TOEFLInformationId, trackChanges, cancellationToken);
	}
}
