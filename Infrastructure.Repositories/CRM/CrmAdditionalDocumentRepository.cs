using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmAdditionalDocument data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmAdditionalDocumentRepository : RepositoryBase<CrmAdditionalDocument>, ICrmAdditionalDocumentRepository
{
	public CrmAdditionalDocumentRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmAdditionalDocument records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmAdditionalDocument>> CrmAdditionalDocumentsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.AdditionalDocumentId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmAdditionalDocument record by ID asynchronously.
	/// </summary>
	public async Task<CrmAdditionalDocument?> CrmAdditionalDocumentAsync(int crmadditionaldocumentid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.AdditionalDocumentId.Equals(crmadditionaldocumentid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmAdditionalDocument records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmAdditionalDocument>> CrmAdditionalDocumentsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.AdditionalDocumentId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmAdditionalDocument records by a applicantid asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmAdditionalDocument>> CrmAdditionalDocumentsByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmAdditionalDocument records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<AdditionalDocument>> CrmAdditionalDocumentsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmAdditionalDocument WHERE ParentId = {parentId} ORDER BY CrmAdditionalDocumentId";
		return await AdoExecuteListQueryAsync<AdditionalDocument>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmAdditionalDocument record.
	/// </summary>
	public async Task<CrmAdditionalDocument> CreateCrmAdditionalDocumentAsync(CrmAdditionalDocument entity, CancellationToken cancellation = default)
	{
		int additionaldocumentid = await CreateAndIdAsync(entity, cancellation);
		entity.AdditionalDocumentId = additionaldocumentid;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmAdditionalDocument record.
	/// </summary>
	public void UpdateCrmAdditionalDocument(CrmAdditionalDocument entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmAdditionalDocument record.
	/// </summary>
	public async Task DeleteCrmAdditionalDocumentAsync(CrmAdditionalDocument entity, bool trackChanges, CancellationToken cancellationToken = default) => await DeleteAsync(x => x.AdditionalDocumentId.Equals(entity.AdditionalDocumentId), trackChanges, cancellationToken);
}
