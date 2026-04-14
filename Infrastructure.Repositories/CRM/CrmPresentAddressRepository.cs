using bdDevCRM.Entities.Entities.CRM;
using bdDevCRM.RepositoriesContracts.CRM;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmPresentAddress data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmPresentAddressRepository : RepositoryBase<CrmPresentAddress>, ICrmPresentAddressRepository
{
	public CrmPresentAddressRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmPresentAddress records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.PresentAddressId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmPresentAddress record by ID asynchronously.
	/// </summary>
	public async Task<CrmPresentAddress?> CrmPresentAddressAsync(int crmpresentaddressid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.PresentAddressId.Equals(crmpresentaddressid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPresentAddress records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.PresentAddressId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPresentAddress records by a countryId asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => x.CountryId.Equals(countryId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPresentAddress records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPresentAddress>> CrmPresentAddresssByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmPresentAddress WHERE ParentId = {parentId} ORDER BY PresentAddressId";
		return await AdoExecuteListQueryAsync<CrmPresentAddress>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmPresentAddress record.
	/// </summary>
	public async Task<CrmPresentAddress> CreateCrmPresentAddressAsync(CrmPresentAddress entity, CancellationToken cancellation = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellation);
		entity.PresentAddressId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmPresentAddress record.
	/// </summary>
	public void UpdateCrmPresentAddress(CrmPresentAddress entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmPresentAddress record.
	/// </summary>
	public async Task DeleteCrmPresentAddressAsync(CrmPresentAddress entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.PresentAddressId.Equals(entity.PresentAddressId), trackChanges, cancellationToken);
}
