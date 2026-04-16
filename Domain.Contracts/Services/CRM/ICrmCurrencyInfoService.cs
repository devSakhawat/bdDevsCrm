using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCurrencyInfoService
{
    Task<CrmCurrencyInfoDto> CreateAsync(CreateCrmCurrencyInfoRecord record, CancellationToken cancellationToken = default);
    Task<CrmCurrencyInfoDto> UpdateAsync(UpdateCrmCurrencyInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCurrencyInfoRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCurrencyInfoDto> CrmCurrencyInfoAsync(int currencyId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCurrencyInfoDto>> CrmCurrencyInfosAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCurrencyInfoDto>> CrmCurrencyInfosForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCurrencyInfoDto>> CrmCurrencyInfosSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
