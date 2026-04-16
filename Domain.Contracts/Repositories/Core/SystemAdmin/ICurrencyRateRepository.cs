using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Core.SystemAdmin;

public interface ICurrencyRateRepository : IRepositoryBase<CurrencyRate>
{
    Task<CurrencyRate?> CurrencyRateAsync(int currencyRateId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CurrencyRate>> CurrencyRatesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CurrencyRate>> CurrencyRatesByCurrencyIdAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default);
}
