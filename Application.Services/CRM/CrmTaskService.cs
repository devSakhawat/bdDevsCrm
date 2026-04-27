using Domain.Contracts.Repositories;
using Domain.Entities.Entities.CRM;
using Domain.Contracts.Services.CRM;
using bdDevs.Shared.DataTransferObjects.CRM;
using Domain.Exceptions;
using Application.Shared.Grid;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using bdDevs.Shared.Records.CRM;
using bdDevs.Shared.Extensions;

namespace Application.Services.CRM;

/// <summary>CrmTask service implementing business logic for task management.</summary>
internal sealed class CrmTaskService : ICrmTaskService
{
    private readonly IRepositoryManager _repository;
    private readonly ILogger<CrmTaskService> _logger;
    private readonly IConfiguration _config;

    public CrmTaskService(IRepositoryManager repository, ILogger<CrmTaskService> logger, IConfiguration configuration)
    {
        _repository = repository;
        _logger = logger;
        _config = configuration;
    }

    /// <summary>Creates a new task record.</summary>
    public async Task<CrmTaskDto> CreateAsync(CreateCrmTaskRecord record, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(CreateCrmTaskRecord));

        _logger.LogInformation("Creating new Task. Time: {Time}", DateTime.UtcNow);

        CrmTask entity = record.MapTo<CrmTask>();
        int newId = await _repository.CrmTasks.CreateAndIdAsync(entity, cancellationToken);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Task created successfully. ID: {Id}, Time: {Time}", newId, DateTime.UtcNow);
        return entity.MapTo<CrmTaskDto>() with { TaskId = newId };
    }

    /// <summary>Updates an existing task record.</summary>
    public async Task<CrmTaskDto> UpdateAsync(UpdateCrmTaskRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null)
            throw new BadRequestException(nameof(UpdateCrmTaskRecord));

        _ = await _repository.CrmTasks
            .FirstOrDefaultAsync(x => x.TaskId == record.TaskId, trackChanges: false, cancellationToken)
            ?? throw new NotFoundException("Task", "TaskId", record.TaskId.ToString());

        _logger.LogInformation("Updating Task. ID: {Id}, Time: {Time}", record.TaskId, DateTime.UtcNow);

        CrmTask entity = record.MapTo<CrmTask>();
        _repository.CrmTasks.UpdateByState(entity);
        await _repository.SaveAsync(cancellationToken);

        _logger.LogInformation("Task updated successfully. ID: {Id}, Time: {Time}", record.TaskId, DateTime.UtcNow);
        return entity.MapTo<CrmTaskDto>();
    }

    /// <summary>Deletes a task record.</summary>
    public async Task DeleteAsync(DeleteCrmTaskRecord record, bool trackChanges, CancellationToken cancellationToken = default)
    {
        if (record == null || record.TaskId <= 0)
            throw new BadRequestException("Invalid delete request!");

        _ = await _repository.CrmTasks
            .FirstOrDefaultAsync(x => x.TaskId == record.TaskId, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Task", "TaskId", record.TaskId.ToString());

        _logger.LogInformation("Deleting Task. ID: {Id}, Time: {Time}", record.TaskId, DateTime.UtcNow);
        await _repository.CrmTasks.DeleteAsync(x => x.TaskId == record.TaskId, trackChanges: false, cancellationToken);
        await _repository.SaveAsync(cancellationToken);
        _logger.LogWarning("Task deleted successfully. ID: {Id}, Time: {Time}", record.TaskId, DateTime.UtcNow);
    }

    /// <summary>Retrieves a single task record by ID.</summary>
    public async Task<CrmTaskDto> TaskAsync(int id, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Task. ID: {Id}, Time: {Time}", id, DateTime.UtcNow);
        var entity = await _repository.CrmTasks
            .FirstOrDefaultAsync(x => x.TaskId == id, trackChanges, cancellationToken)
            ?? throw new NotFoundException("Task", "TaskId", id.ToString());
        return entity.MapTo<CrmTaskDto>();
    }

    /// <summary>Retrieves all task records.</summary>
    public async Task<IEnumerable<CrmTaskDto>> TasksAsync(bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching all Tasks. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmTasks.CrmTasksAsync(trackChanges, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Tasks found. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmTaskDto>();
        }
        _logger.LogInformation("Tasks fetched. Count: {Count}, Time: {Time}", entities.Count(), DateTime.UtcNow);
        return entities.MapToList<CrmTaskDto>();
    }

    /// <summary>Retrieves a lightweight list of tasks for dropdown binding.</summary>
    public async Task<IEnumerable<CrmTaskDto>> TaskForDDLAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Tasks for DDL. Time: {Time}", DateTime.UtcNow);
        var entities = await _repository.CrmTasks.CrmTasksAsync(false, cancellationToken);
        if (!entities.Any())
        {
            _logger.LogWarning("No Tasks found for DDL. Time: {Time}", DateTime.UtcNow);
            return Enumerable.Empty<CrmTaskDto>();
        }
        return entities.MapToList<CrmTaskDto>();
    }

    /// <summary>Retrieves a paginated summary grid of tasks.</summary>
    public async Task<GridEntity<CrmTaskDto>> TasksSummaryAsync(GridOptions options, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Fetching Tasks summary grid. Time: {Time}", DateTime.UtcNow);
        const string sql = @"SELECT TaskId, TaskTitle, TaskDescription, DueDate, AssignedTo, RelatedEntityType, RelatedEntityId, Priority, IsCompleted, CompletedDate, CreatedDate, CreatedBy, UpdatedDate, UpdatedBy FROM CrmTask";
        const string orderBy = "DueDate ASC";
        return await _repository.CrmTasks.AdoGridDataAsync<CrmTaskDto>(sql, options, orderBy, string.Empty, cancellationToken);
    }
}
