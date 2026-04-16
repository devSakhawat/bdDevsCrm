using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class ApproverTypeRepository : RepositoryBase<ApproverType>, IApproverTypeRepository
{
    public ApproverTypeRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<ApproverType?> ApproverTypeAsync(int approverTypeId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.ApproverTypeId == approverTypeId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverType>> ApproverTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
