using Domain.Contracts.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class ApproverTypeToGroupMappingRepository : RepositoryBase<ApproverTypeToGroupMapping>, IApproverTypeToGroupMappingRepository
{
    public ApproverTypeToGroupMappingRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<ApproverTypeToGroupMapping?> ApproverTypeToGroupMappingAsync(int approverTypeMapId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.ApproverTypeMapId == approverTypeMapId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<ApproverTypeToGroupMapping>> ApproverTypeToGroupMappingsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
