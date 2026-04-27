using Domain.Entities.Entities.CRM;
using Domain.Contracts.CRM;
using Infrastructure.Sql.Context;

namespace Infrastructure.Repositories.CRM;

/// <summary>Repository for CrmTask data access operations.</summary>
public class CrmTaskRepository : RepositoryBase<CrmTask>, ICrmTaskRepository
{
    public CrmTaskRepository(CrmContext context) : base(context) { }

    /// <summary>Retrieves all CrmTask records asynchronously.</summary>
    public async Task<IEnumerable<CrmTask>> CrmTasksAsync(bool trackChanges, CancellationToken cancellationToken = default)
        => await ListAsync(x => x.TaskId, trackChanges, cancellationToken);

    /// <summary>Retrieves a single CrmTask record by ID asynchronously.</summary>
    public async Task<CrmTask?> CrmTaskAsync(int taskId, bool trackChanges, CancellationToken cancellationToken = default)
        => await FirstOrDefaultAsync(x => x.TaskId.Equals(taskId), trackChanges, cancellationToken);

    /// <summary>Retrieves CrmTask records by a collection of IDs asynchronously.</summary>
    public async Task<IEnumerable<CrmTask>> CrmTasksByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default)
        => await ListByIdsAsync(x => ids.Contains(x.TaskId), trackChanges, cancellationToken);
}
