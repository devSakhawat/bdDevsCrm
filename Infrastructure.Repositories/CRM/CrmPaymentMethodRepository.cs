using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>
/// Repository for CrmPaymentMethod data access operations.
/// Implements enterprise patterns with async support and raw SQL capabilities.
/// </summary>
public class CrmPaymentMethodRepository : RepositoryBase<CrmPaymentMethod>, ICrmPaymentMethodRepository
{
	public CrmPaymentMethodRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all CrmPaymentMethod records asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethod>> CrmPaymentMethodsAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(x => x.PaymentMethodId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single CrmPaymentMethod record by ID asynchronously.
	/// </summary>
	public async Task<CrmPaymentMethod?> CrmPaymentMethodAsync(int crmpaymentmethodid, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.PaymentMethodId.Equals(crmpaymentmethodid),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPaymentMethod records by a collection of IDs asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethod>> CrmPaymentMethodsByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListByIdsAsync(x => ids.Contains(x.PaymentMethodId), trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves CrmPaymentMethod records by parent ID asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmPaymentMethod>> CrmPaymentMethodsByParentIdAsync(int parentId, CancellationToken cancellationToken = default)
	{
		string query = $"SELECT * FROM CrmPaymentMethod WHERE ParentId = {parentId} ORDER BY PaymentMethodId";
		return await AdoExecuteListQueryAsync<CrmPaymentMethod>(query, null, cancellationToken);
	}

	/// <summary>
	/// Creates a new CrmPaymentMethod record.
	/// </summary>
	public async Task<CrmPaymentMethod> CreateCrmPaymentMethodAsync(CrmPaymentMethod entity, CancellationToken cancellationToken = default)
	{
		var newId = await CreateAndIdAsync(entity, cancellationToken);
		entity.PaymentMethodId = newId;
		return entity;
	}

	/// <summary>
	/// Updates an existing CrmPaymentMethod record.
	/// </summary>
	public void UpdateCrmPaymentMethod(CrmPaymentMethod entity) => UpdateByState(entity);

	/// <summary>
	/// Deletes a CrmPaymentMethod record.
	/// </summary>
	public async Task DeleteCrmPaymentMethodAsync(CrmPaymentMethod entity, bool trackChanges, CancellationToken cancellationToken = default)
	=> await DeleteAsync(x => x.PaymentMethodId.Equals(entity.PaymentMethodId), trackChanges, cancellationToken);
}
