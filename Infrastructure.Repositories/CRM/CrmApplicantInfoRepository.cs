using bdDevCRM.Entities.Entities.CRM;
using bdDevCRM.RepositoriesContracts.CRM;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmApplicantInfo data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmApplicantInfoRepository : RepositoryBase<CrmApplicantInfo>, ICrmApplicantInfoRepository
{
	public CrmApplicantInfoRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmApplicantInfo records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantInfo>> CrmApplicantInfosAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.ApplicationId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmApplicantInfo record by ID asynchronously.
	/// </summary>
	public async Task<CrmApplicantInfo?> CrmApplicantInfoAsync(int crmapplicantinfoid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.ApplicationId.Equals(crmapplicantinfoid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplicantInfo records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantInfo>> CrmApplicantInfosByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.ApplicationId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmApplicantInfo records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmApplicantInfo>> CrmApplicantInfosByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmApplicantInfo WHERE ParentId = {parentId} ORDER BY CrmApplicantInfoId";
		return await AdoExecuteListQueryAsync<CrmApplicantInfo>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmApplicantInfo record.
	/// </summary>
	public async Task<CrmApplicantInfo> CreateCrmApplicantInfoAsync(CrmApplicantInfo entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.ApplicationId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmApplicantInfo record.
	/// </summary>
	public void UpdateCrmApplicantInfo(CrmApplicantInfo entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmApplicantInfo record.
	/// </summary>
	public async Task DeleteCrmApplicantInfo(CrmApplicantInfo entity, bool trackChanges, CancellationToken cancellation = default)
		=> await DeleteAsync(x => x.ApplicationId.Equals(entity.ApplicationId), trackChanges, cancellation);
}
