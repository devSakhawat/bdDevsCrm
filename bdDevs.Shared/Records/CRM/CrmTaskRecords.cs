namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM task.</summary>
public record CreateCrmTaskRecord(
    string TaskTitle,
    string? TaskDescription,
    DateTime? DueDate,
    int? AssignedTo,
    string? RelatedEntityType,
    int? RelatedEntityId,
    string? Priority,
    bool IsCompleted,
    DateTime? CompletedDate,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM task.</summary>
public record UpdateCrmTaskRecord(
    int TaskId,
    string TaskTitle,
    string? TaskDescription,
    DateTime? DueDate,
    int? AssignedTo,
    string? RelatedEntityType,
    int? RelatedEntityId,
    string? Priority,
    bool IsCompleted,
    DateTime? CompletedDate,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM task.</summary>
/// <param name="TaskId">ID of the task to delete.</param>
public record DeleteCrmTaskRecord(int TaskId);
