using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class ApproverHistoryRepository : RepositoryBase<ApproverHistory>, IApproverHistoryRepository
{
    public ApproverHistoryRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<ApproverHistory?> ApproverHistoryAsync(int assignApproverId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.AssignApproverId == assignApproverId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverHistory>> ApproverHistoryListAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverHistory>> ActiveApproverHistoriesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(a => a.IsActive == true, null, trackChanges, false, cancellationToken);
    }
}
