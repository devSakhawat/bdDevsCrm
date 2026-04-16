using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface ICurrencyRateService
{
    Task<CurrencyRateDto> CreateAsync(CreateCurrencyRateRecord record, CancellationToken cancellationToken = default);
    Task<CurrencyRateDto> UpdateAsync(UpdateCurrencyRateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCurrencyRateRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CurrencyRateDto> CurrencyRateAsync(int currencyRateId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CurrencyRateDto>> CurrencyRatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CurrencyRateDDLDto>> CurrencyRatesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<IEnumerable<CurrencyRateDto>> CurrencyRatesByCurrencyIdAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CurrencyRateDto>> CurrencyRatesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
