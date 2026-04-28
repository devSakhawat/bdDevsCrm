using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

public interface ICrmCommunicationLogService
{
    Task<CrmCommunicationLogDto> CreateAsync(CreateCrmCommunicationLogRecord record, CancellationToken cancellationToken = default);
    Task<CrmCommunicationLogDto> UpdateAsync(UpdateCrmCommunicationLogRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(DeleteCrmCommunicationLogRecord record, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommunicationLogDto> CommunicationLogAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommunicationLogDto>> CommunicationLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommunicationLogDto>> CommunicationLogsByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<GridEntity<CrmCommunicationLogDto>> CommunicationLogsSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
