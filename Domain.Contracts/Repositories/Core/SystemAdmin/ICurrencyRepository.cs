// Interface: ICurrencyRepository
using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.Core.SystemAdmin;

public interface ICurrencyRepository : IRepositoryBase<CrmCurrencyInfo>
{
	/// <summary>
	/// Retrieves all currencies asynchronously.
	/// </summary>
	Task<IEnumerable<CrmCurrencyInfo>> CurrenciesAsync(bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Retrieves a single currency by ID asynchronously.
	/// </summary>
	Task<CrmCurrencyInfo?> CurrencyAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default);

	/// <summary>
	/// Creates a new currency.
	/// </summary>
	Task<CrmCurrencyInfo> CreateCurrencyAsync(CrmCurrencyInfo currency, CancellationToken cancellationToken = default);

	/// <summary>
	/// Updates an existing currency.
	/// </summary>
	void UpdateCurrency(CrmCurrencyInfo currency);

	/// <summary>
	/// Deletes a currency.
	/// </summary>
	Task DeleteCurrencyAsync(CrmCurrencyInfo currency, bool trackChanges, CancellationToken cancellationToken = default);
}







//using Domain.Entities.Entities.CRM;

//namespace bdDevCRM.RepositoriesContracts.Core.SystemAdmin;

//public interface ICurrencyRepository : IRepositoryBase<CrmCurrencyInfo>
//{

//  //void UpdateCurrency(Currency currency);
//  //void DeleteCurrency(Currency currency);
//}
