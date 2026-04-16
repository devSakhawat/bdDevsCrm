using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class PasswordHistoryRepository : RepositoryBase<PasswordHistory>, IPasswordHistoryRepository
{
    public PasswordHistoryRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<PasswordHistory?> PasswordHistoryAsync(int historyId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(p => p.HistoryId == historyId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<PasswordHistory>> PasswordHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
