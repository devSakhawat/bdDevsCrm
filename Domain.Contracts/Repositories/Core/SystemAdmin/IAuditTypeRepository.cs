using Domain.Contracts.Repositories;
using Domain.Entities.Entities.System;

namespace Domain.Contracts.Repositories.Core.SystemAdmin;

public interface IAuditTypeRepository : IRepositoryBase<AuditType>
{
    Task<AuditType?> AuditTypeAsync(int auditTypeId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditType>> AuditTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
}
