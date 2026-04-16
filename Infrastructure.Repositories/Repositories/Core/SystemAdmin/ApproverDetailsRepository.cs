using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class ApproverDetailsRepository : RepositoryBase<ApproverDetails>, IApproverDetailsRepository
{
    public ApproverDetailsRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<ApproverDetails?> ApproverDetailsAsync(int remarksId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.RemarksId == remarksId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverDetails>> ApproverDetailsListAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverDetails>> ActiveApproverDetailsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(a => a.IsOpen == true, null, trackChanges, false, cancellationToken);
    }
}
