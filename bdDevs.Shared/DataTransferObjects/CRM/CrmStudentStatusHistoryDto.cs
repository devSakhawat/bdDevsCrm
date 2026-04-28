namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmStudentStatusHistoryDto
{
    public int StudentStatusHistoryId { get; init; }
    public int StudentId { get; init; }
    public int? OldStatus { get; init; }
    public int NewStatus { get; init; }
    public int ChangedBy { get; init; }
    public DateTime ChangedDate { get; init; }
    public string? Notes { get; init; }
}
