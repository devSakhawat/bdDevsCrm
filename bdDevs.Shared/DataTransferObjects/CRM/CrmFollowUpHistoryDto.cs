namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmFollowUpHistoryDto
{
    public int FollowUpHistoryId { get; init; }
    public int FollowUpId { get; init; }
    public byte OldStatus { get; init; }
    public byte NewStatus { get; init; }
    public int ChangedBy { get; init; }
    public DateTime ChangedDate { get; init; }
    public string? Remarks { get; init; }
}
