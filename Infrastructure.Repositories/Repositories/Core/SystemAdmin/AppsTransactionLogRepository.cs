using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class AppsTransactionLogRepository : RepositoryBase<AppsTransactionLog>, IAppsTransactionLogRepository
{
    public AppsTransactionLogRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<AppsTransactionLog?> AppsTransactionLogAsync(int transactionLogId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.TransactionLogId == transactionLogId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<AppsTransactionLog>> AppsTransactionLogsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
