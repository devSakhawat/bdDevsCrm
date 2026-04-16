using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class AuditTypeRepository : RepositoryBase<AuditType>, IAuditTypeRepository
{
    public AuditTypeRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<AuditType?> AuditTypeAsync(int auditTypeId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.AuditTypeId == auditTypeId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<AuditType>> AuditTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
