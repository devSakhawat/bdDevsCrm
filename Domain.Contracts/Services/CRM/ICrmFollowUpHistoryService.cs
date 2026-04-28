using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmFollowUpHistoryService
{
    Task<CrmFollowUpHistoryDto> CreateAsync(CreateCrmFollowUpHistoryRecord record, CancellationToken cancellationToken = default);
    Task<CrmFollowUpHistoryDto> UpdateAsync(UpdateCrmFollowUpHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmFollowUpHistoryRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmFollowUpHistoryDto> FollowUpHistoryAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUpHistoryDto>> FollowUpHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmFollowUpHistoryDto>> FollowUpHistoriesByFollowUpIdAsync(int followUpId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmFollowUpHistoryDto>> FollowUpHistoriesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
