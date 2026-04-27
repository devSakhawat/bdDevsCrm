namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmTaskDto
{
    public int TaskId { get; init; }
    public string TaskTitle { get; init; } = string.Empty;
    public string? TaskDescription { get; init; }
    public DateTime? DueDate { get; init; }
    public int? AssignedTo { get; init; }
    public string? RelatedEntityType { get; init; }
    public int? RelatedEntityId { get; init; }
    public string? Priority { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime? CompletedDate { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
