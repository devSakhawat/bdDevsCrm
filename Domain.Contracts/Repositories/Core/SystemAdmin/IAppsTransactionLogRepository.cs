using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IAppsTransactionLogRepository : IRepositoryBase<AppsTransactionLog>
{
    Task<AppsTransactionLog?> AppsTransactionLogAsync(int transactionLogId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AppsTransactionLog>> AppsTransactionLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
