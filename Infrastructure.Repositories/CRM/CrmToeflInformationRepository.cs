using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmToeflInformation data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmToeflInformationRepository : RepositoryBase<CrmToeflInformation>, ICrmToeflInformationRepository
{
	public CrmToeflInformationRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmToeflInformation records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmToeflInformation>> CrmToeflInformationsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.TOEFLInformationId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmToeflInformation record by ID asynchronously.
	/// </summary>
	public async Task<CrmToeflInformation?> CrmToeflInformationAsync(int crmtoeflinformationid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.TOEFLInformationId.Equals(crmtoeflinformationid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmToeflInformation records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmToeflInformation>> CrmToeflInformationsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.TOEFLInformationId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmToeflInformation records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmToeflInformation>> CrmToeflInformationsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmToeflInformation WHERE ParentId = {parentId} ORDER BY TOEFLInformationId";
		return await AdoExecuteListQueryAsync<CrmToeflInformation>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmToeflInformation record.
	/// </summary>
	public async Task<CrmToeflInformation> CreateCrmTOEFLInformationAsync(CrmToeflInformation entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.TOEFLInformationId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmToeflInformation record.
	/// </summary>
	public void UpdateCrmTOEFLInformation(CrmToeflInformation entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmToeflInformation record.
	/// </summary>
	public async Task DeleteCrmTOEFLInformationAsync(CrmToeflInformation entity, bool trackChanges, CancellationToken cancellationToken = default)
	{
		await DeleteAsync(x => x.TOEFLInformationId == entity.TOEFLInformationId, trackChanges, cancellationToken);
	}
}
