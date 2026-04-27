using Application.Shared.Grid;
using bdDevs.Shared.DataTransferObjects.CRM;
using bdDevs.Shared.Records.CRM;

namespace Domain.Contracts.Services.CRM;

/// <summary>Service contract for CRM task management operations.</summary>
public interface ICrmTaskService
{
    /// <summary>Creates a new task record.</summary>
    Task<CrmTaskDto> CreateAsync(CreateCrmTaskRecord record, CancellationToken cancellationToken = default);

    /// <summary>Updates an existing task record.</summary>
    Task<CrmTaskDto> UpdateAsync(UpdateCrmTaskRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Deletes a task record.</summary>
    Task DeleteAsync(DeleteCrmTaskRecord record, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a single task record by ID.</summary>
    Task<CrmTaskDto> TaskAsync(int id, bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves all task records.</summary>
    Task<IEnumerable<CrmTaskDto>> TasksAsync(bool trackChanges, CancellationToken cancellationToken = default);

    /// <summary>Retrieves a lightweight list for dropdown binding.</summary>
    Task<IEnumerable<CrmTaskDto>> TaskForDDLAsync(CancellationToken cancellationToken = default);

    /// <summary>Retrieves a paginated summary grid of tasks.</summary>
    Task<GridEntity<CrmTaskDto>> TasksSummaryAsync(GridOptions options, CancellationToken cancellationToken = default);
}
