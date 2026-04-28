using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmCommunicationLogRepository : IRepositoryBase<CrmCommunicationLog>
{
    Task<IEnumerable<CrmCommunicationLog>> CommunicationLogsAsync(bool trackChanges, CancellationToken cancellationToken = default);
    Task<CrmCommunicationLog?> CommunicationLogAsync(int communicationLogId, bool trackChanges, CancellationToken cancellationToken = default);
    Task<IEnumerable<CrmCommunicationLog>> CommunicationLogsByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default);
}
