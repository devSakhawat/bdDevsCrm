using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAuditLogService
{
    Task<AuditLogDto> CreateAsync(CreateAuditLogRecord record, CancellationToken cancellationToken = default);
    Task<AuditLogDto> UpdateAsync(UpdateAuditLogRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAuditLogRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AuditLogDto> AuditLogAsync(long auditId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLogDto>> AuditLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditLogDDLDto>> AuditLogsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AuditLogDto>> AuditLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
