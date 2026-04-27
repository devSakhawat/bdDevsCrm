using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;

namespace Domain.Contracts.CRM;

public interface ICrmTaskRepository : IRepositoryBase<CrmTask>
{
    /// <summary>Retrieves all CrmTask records asynchronously.</summary>
    Task<IEnumerable<CrmTask>> CrmTasksAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single CrmTask record by ID asynchronously.</summary>
    Task<CrmTask?> CrmTaskAsync(int taskId, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves CrmTask records by a collection of IDs asynchronously.</summary>
    Task<IEnumerable<CrmTask>> CrmTasksByIdsAsync(IEnumerable<int> ids, bool trackChanges, CancellationToken cancellationToken = default);
}
