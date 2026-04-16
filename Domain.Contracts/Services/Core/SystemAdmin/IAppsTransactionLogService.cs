using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAppsTransactionLogService
{
    Task<AppsTransactionLogDto> CreateAsync(CreateAppsTransactionLogRecord record, CancellationToken cancellationToken = default);
    Task<AppsTransactionLogDto> UpdateAsync(UpdateAppsTransactionLogRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAppsTransactionLogRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AppsTransactionLogDto> AppsTransactionLogAsync(int transactionLogId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppsTransactionLogDto>> AppsTransactionLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppsTransactionLogDDLDto>> AppsTransactionLogsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AppsTransactionLogDto>> AppsTransactionLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
