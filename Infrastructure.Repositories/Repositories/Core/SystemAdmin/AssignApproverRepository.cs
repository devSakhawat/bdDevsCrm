using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class AssignApproverRepository : RepositoryBase<AssignApprover>, IAssignApproverRepository
{
    public AssignApproverRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<AssignApprover?> AssignApproverAsync(int assignApproverId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.AssignApproverId == assignApproverId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<AssignApprover>> AssignApproversAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<AssignApprover>> ActiveAssignApproversAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListByConditionAsync(a => a.IsActive == true, null, trackChanges, false, cancellationToken);
    }
}
