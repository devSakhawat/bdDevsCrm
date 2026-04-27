using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmVisaStatusHistoryService
{
    Task<CrmVisaStatusHistoryDto> CreateAsync(CreateCrmVisaStatusHistoryRecord record, CancellationToken cancellationToken = default);
    Task<CrmVisaStatusHistoryDto> UpdateAsync(UpdateCrmVisaStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmVisaStatusHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmVisaStatusHistoryDto> VisaStatusHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaStatusHistoryDto>> VisaStatusHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmVisaStatusHistoryDto>> VisaStatusHistoriesByVisaApplicationIdAsync(int visaApplicationId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmVisaStatusHistoryDto>> VisaStatusHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
