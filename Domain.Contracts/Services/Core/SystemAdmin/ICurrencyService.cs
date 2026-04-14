using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICurrencyService
{
  /// <summary>
  /// Retrieves all currencies asynchronously.
  /// </summary>
  Task<IEnumerable<CurrencyDto>> CurrenciesAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
  Task<IEnumerable<CurrencyDDLDto>> CurrenciesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
  Task<GridEntity<CurrencyDto>> CurrencySummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
  Task<CurrencyDto> CreateAsync(CurrencyDto modelDto, CancellationToken cancellationToken = default);
  Task<CurrencyDto> UpdateAsync(int key, CurrencyDto modelDto, bool trackChanges, CancellationToken cancellationToken = default);
  Task DeleteAsync(int key, bool trackChanges, CancellationToken cancellationToken = default);
  Task<CurrencyDto> CurrencyAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default);

  //Task<IEnumerable<CurrencyDDL>> CurrenciesDDLAsync();
  //Task<GridEntity<CurrencyDto>> CurrecySummary(GridOptions options);
  //Task<string> SaveOrUpdate(int key,CurrencyDto modelDto);
  //Task<string> DeleteCurrency(int key, CurrencyDto modelDto);

}
