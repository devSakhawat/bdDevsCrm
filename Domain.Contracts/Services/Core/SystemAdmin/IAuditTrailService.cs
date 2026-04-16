using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAuditTrailService
{
    Task<AuditTrailDto> CreateAsync(CreateAuditTrailRecord record, CancellationToken cancellationToken = default);
    Task<AuditTrailDto> UpdateAsync(UpdateAuditTrailRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAuditTrailRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AuditTrailDto> AuditTrailAsync(int auditId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditTrailDto>> AuditTrailsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditTrailDDLDto>> AuditTrailsForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AuditTrailDto>> AuditTrailsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
