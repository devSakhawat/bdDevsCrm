using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmAdditionalInfo data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmAdditionalInfoRepository : RepositoryBase<CrmAdditionalInfo>, ICrmAdditionalInfoRepository
{
	public CrmAdditionalInfoRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmAdditionalInfo records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.AdditionalInfoId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmAdditionalInfo record by ID asynchronously.
	/// </summary>
	public async Task<CrmAdditionalInfo?> CrmAdditionalInfoAsync(int crmadditionalinfoid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.AdditionalInfoId.Equals(crmadditionalinfoid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmAdditionalInfo records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.AdditionalInfoId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmAdditionalInfo records by applicantId asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosByApplicantIdAsync(int applicantId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => x.ApplicantId == applicantId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmAdditionalInfo records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmAdditionalInfo>> CrmAdditionalInfosByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmAdditionalInfo WHERE ParentId = {parentId} ORDER BY CrmAdditionalInfoId";
		return await AdoExecuteListQueryAsync<CrmAdditionalInfo>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmAdditionalInfo record.
	/// </summary>
	public async Task<CrmAdditionalInfo> CreateCrmAdditionalInfo(CrmAdditionalInfo entity, CancellationToken cancellation = default)
	{
		int newId = await CreateAndIdAsync(entity, cancellation);
		entity.AdditionalInfoId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmAdditionalInfo record.
	/// </summary>
	public void UpdateCrmAdditionalInfo(CrmAdditionalInfo entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmAdditionalInfo record.
	/// </summary>
	public async Task DeleteCrmAdditionalInfo(CrmAdditionalInfo entity, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.AdditionalInfoId.Equals(entity.AdditionalInfoId), trackChanges, cancellationToken);
}
