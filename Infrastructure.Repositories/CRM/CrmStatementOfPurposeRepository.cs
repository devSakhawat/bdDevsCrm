using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmStatementOfPurpose data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmStatementOfPurposeRepository : RepositoryBase<CrmStatementOfPurpose>, ICrmStatementOfPurposeRepository
{
	public CrmStatementOfPurposeRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmStatementOfPurpose records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmStatementOfPurpose>> CrmStatementOfPurposesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.StatementOfPurposeId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmStatementOfPurpose record by ID asynchronously.
	/// </summary>
	public async Task<CrmStatementOfPurpose?> CrmStatementOfPurposeAsync(int StatementOfPurposeId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.StatementOfPurposeId.Equals(StatementOfPurposeId),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmStatementOfPurpose records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmStatementOfPurpose>> CrmStatementOfPurposesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.StatementOfPurposeId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmStatementOfPurpose records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmStatementOfPurpose>> CrmStatementOfPurposesByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmStatementOfPurpose WHERE ParentId = {parentId} ORDER BY StatementOfPurposeId";
		return await AdoExecuteListQueryAsync<CrmStatementOfPurpose>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmStatementOfPurpose record.
	/// </summary>
	public async Task<CrmStatementOfPurpose> CreateCrmStatementOfPurposeAsync(CrmStatementOfPurpose entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.StatementOfPurposeId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmStatementOfPurpose record.
	/// </summary>
	public void UpdateCrmStatementOfPurpose(CrmStatementOfPurpose entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmStatementOfPurpose record.
	/// </summary>
	public async Task DeleteCrmStatementOfPurposeAsync(CrmStatementOfPurpose entity, bool trackChanges, CancellationToken cancellationToken = default)
			=> await DeleteAsync(x => x.StatementOfPurposeId == entity.StatementOfPurposeId, trackChanges, cancellationToken);
}
