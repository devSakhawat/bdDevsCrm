using bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;
using bdDevs.Shared.Records.Core.SystemAdmin;
using Application.Shared.Grid;

namespace Domain.Contracts.Services.Core.SystemAdmin;

public interface IAuditTypeService
{
    Task<AuditTypeDto> CreateAsync(CreateAuditTypeRecord record, CancellationToken cancellationToken = default);
    Task<AuditTypeDto> UpdateAsync(UpdateAuditTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteAuditTypeRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<AuditTypeDto> AuditTypeAsync(int auditTypeId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditTypeDto>> AuditTypesAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<AuditTypeDDLDto>> AuditTypesForDDLAsync(bool trackChanges = false, CancellationToken cancellationToken = default);
    Task<GridEntity<AuditTypeDto>> AuditTypesSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
