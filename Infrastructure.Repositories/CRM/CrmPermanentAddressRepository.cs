using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmPermanentAddress data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmPermanentAddressRepository : RepositoryBase<CrmPermanentAddress>, ICrmPermanentAddressRepository
{
	public CrmPermanentAddressRepository(CrmContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmPermanentAddress records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.PermanentAddressId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmPermanentAddress record by ID asynchronously.
	/// </summary>
	public async Task<CrmPermanentAddress?> CrmPermanentAddressAsync(int crmpermanentaddressid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.PermanentAddressId.Equals(crmpermanentaddressid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPermanentAddress records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.PermanentAddressId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPermanentAddress records by a countryId asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssByCountryIdAsync(int countryId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => x.CountryId == countryId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPermanentAddress records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPermanentAddress>> CrmPermanentAddresssByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmPermanentAddress WHERE ParentId = {parentId} ORDER BY PermanentAddressId";
		return await AdoExecuteListQueryAsync<CrmPermanentAddress>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmPermanentAddress record.
	/// </summary>
	public async Task<CrmPermanentAddress> CreateCrmPermanentAddressAsync(CrmPermanentAddress entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.PermanentAddressId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmPermanentAddress record.
	/// </summary>
	public void UpdateCrmPermanentAddress(CrmPermanentAddress entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmPermanentAddress record.
	/// </summary>
	public async Task DeleteCrmPermanentAddressAsync(CrmPermanentAddress entity, bool trackChanges, CancellationToken cancellationToken = default)
		=> await DeleteAsync(x => x.PermanentAddressId.Equals(entity.PermanentAddressId), trackChanges, cancellationToken);
}
