using Domain.Contracts.CRM;
using Domain.Entities.Entities.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

public class CrmCommunicationLogRepository : RepositoryBase<CrmCommunicationLog>, ICrmCommunicationLogRepository
{
    public CrmCommunicationLogRepository(CrmContext context) : base(context) { }

    public async Task<IEnumerable<CrmCommunicationLog>> CommunicationLogsAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.CommunicationLogId, trackChanges, cancellationToken);

    public async Task<CrmCommunicationLog?> CommunicationLogAsync(int communicationLogId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.CommunicationLogId == communicationLogId, trackChanges, cancellationToken);

    public async Task<IEnumerable<CrmCommunicationLog>> CommunicationLogsByEntityAsync(byte entityType, int entityId, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByConditionAsync(x => x.EntityType == entityType && x.EntityId == entityId && !x.IsDeleted, x => x.LoggedDate, trackChanges, true, cancellationToken);
}
