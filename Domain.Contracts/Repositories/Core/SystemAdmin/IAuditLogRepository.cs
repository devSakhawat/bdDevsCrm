using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IAuditLogRepository : IRepositoryBase<AuditLog>
{
    Task<AuditLog?> AuditLogAsync(long auditId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLog>> AuditLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
