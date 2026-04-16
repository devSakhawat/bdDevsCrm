using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories;
using Infrastructure.Sql.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class AuditTrailRepository : RepositoryBase<AuditTrail>, IAuditTrailRepository
{
    public AuditTrailRepository(CrmContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<AuditTrail?> AuditTrailAsync(int auditId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.AuditId == auditId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<AuditTrail>> AuditTrailsAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(null, trackChanges, cancellationToken);
    }
}
