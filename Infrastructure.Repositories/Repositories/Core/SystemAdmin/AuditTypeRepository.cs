using Domain.Contracts.Repositories.Core.SystemAdmin;
using Domain.Entities.Entities.System;
using Infrastructure.Repositories.Repositories.Common;
using Infrastructure.Sql;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories.Repositories.Core.SystemAdmin;

internal sealed class AuditTypeRepository : RepositoryBase<AuditType>, IAuditTypeRepository
{
    public AuditTypeRepository(ApplicationDbContext repositoryContext) : base(repositoryContext)
    {
    }

    public async Task<AuditType?> AuditTypeAsync(int auditTypeId, bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await FirstOrDefaultAsync(a => a.AuditTypeId == auditTypeId, trackChanges, cancellationToken);
    }

    public async Task<IEnumerable<AuditType>> AuditTypesAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        return await ListAsync(trackChanges, cancellationToken);
    }
}
