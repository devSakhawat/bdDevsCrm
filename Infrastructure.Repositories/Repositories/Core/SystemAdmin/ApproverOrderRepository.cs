using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class ApproverOrderRepository : RepositoryBase<ApproverOrder>, IApproverOrderRepository
{
    public ApproverOrderRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<ApproverOrder?> ApproverOrderAsync(int approverOrderId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.ApproverOrderId == approverOrderId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverOrder>> ApproverOrdersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverOrder>> ActiveApproverOrdersAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
