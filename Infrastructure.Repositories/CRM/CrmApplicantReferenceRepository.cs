using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmApplicantReference data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmApplicantReferenceRepository : RepositoryBase<CrmApplicantReference>, ICrmApplicantReferenceRepository
{
	public CrmApplicantReferenceRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmApplicantReference records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.ApplicantReferenceId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmApplicantReference record by ID asynchronously.
	/// </summary>
	public async Task<CrmApplicantReference?> CrmApplicantReferenceAsync(int crmapplicantreferenceid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.ApplicantReferenceId.Equals(crmapplicantreferenceid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplicantReference records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.ApplicantReferenceId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplicantReference records by a applicantId asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplicantReference records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantReference>> CrmApplicantReferencesByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmApplicantReference WHERE ParentId = {parentId} ORDER BY CrmApplicantReferenceId";
		return await AdoExecuteListQueryAsync<CrmApplicantReference>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmApplicantReference record.
	/// </summary>
	public async Task<CrmApplicantReference> CreateCrmApplicantReference(CrmApplicantReference entity, CancellationToken cancellation = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellation);
		entity.ApplicantReferenceId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmApplicantReference record.
	/// </summary>
	public void UpdateCrmApplicantReference(CrmApplicantReference entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmApplicantReference record.
	/// </summary>
	public async Task DeleteCrmApplicantReference(CrmApplicantReference entity, bool trackChanges, CancellationToken cancellationToken)
	=> await DeleteAsync(x => x.ApplicantReferenceId.Equals(entity.ApplicantReferenceId), trackChanges, cancellationToken);
}
