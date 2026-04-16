using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IAuditTrailRepository : IRepositoryBase<AuditTrail>
{
    Task<AuditTrail?> AuditTrailAsync(int auditId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditTrail>> AuditTrailsAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
