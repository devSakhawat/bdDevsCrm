using bdDevCRM.Entities.Entities.CRM;
using bdDevCRM.RepositoriesContracts.Core.SystemAdmin;
using bdDevCRM.Sql.Context;

namespace Infrastructure.Repositories.Core.SystemAdmin;

/// <summary>
/// Repository for currency data access operations.
/// Implements enterprise patterns with async support.
/// </summary>
public class CurrencyRepository : RepositoryBase<CrmCurrencyInfo>, ICurrencyRepository
{
	public CurrencyRepository(CRMContext context) : base(context) { }

	/// <summary>
	/// Retrieves all currencies asynchronously.
	/// </summary>
	public async Task<IEnumerable<CrmCurrencyInfo>> CurrenciesAsync(bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await ListAsync(c => c.CurrencyId, trackChanges, cancellationToken);
	}

	/// <summary>
	/// Retrieves a single currency by ID asynchronously.
	/// </summary>
	public async Task<CrmCurrencyInfo?> CurrencyAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default)
	{
		return await FirstOrDefaultAsync(
				x => x.CurrencyId.Equals(currencyId),
				trackChanges,
				cancellationToken);
	}

	/// <summary>
	/// Creates a new currency.
	/// </summary>
	public async Task<CrmCurrencyInfo> CreateCurrencyAsync(CrmCurrencyInfo currency, CancellationToken cancellationToken = default)
	{
		int currencyId = await CreateAndIdAsync(currency, cancellationToken);
		currency.CurrencyId = currencyId;
		return currency;
	}

	/// <summary>
	/// Updates an existing currency.
	/// </summary>
	public void UpdateCurrency(CrmCurrencyInfo currency) => UpdateByState(currency);

	/// <summary>
	/// Deletes a currency.
	/// </summary>
	public async Task DeleteCurrencyAsync(CrmCurrencyInfo currency, bool trackChanges, CancellationToken cancellationToken = default)
=> await DeleteAsync(x => x.CurrencyId == currency.CurrencyId, trackChanges, cancellationToken);
}
