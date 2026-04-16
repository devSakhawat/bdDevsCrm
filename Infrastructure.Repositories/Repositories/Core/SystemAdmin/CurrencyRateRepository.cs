using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class CurrencyRateRepository : RepositoryBase<CurrencyRate>, ICurrencyRateRepository
{
    public CurrencyRateRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<CurrencyRate?> CurrencyRateAsync(int currencyRateId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(cr => cr.CurencyRateId == currencyRateId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CurrencyRate>> CurrencyRatesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<CurrencyRate>> CurrencyRatesByCurrencyIdAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(cr => cr.CurrencyId == currencyId, null, trackChanges, false, cancellationToken);
    }
}
