namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCommunicationLogDto
{
    public int CommunicationLogId { get; init; }
    public byte EntityType { get; init; }
    public int EntityId { get; init; }
    public int BranchId { get; init; }
    public byte CommunicationType { get; init; }
    public string Direction { get; init; } = "Outbound";
    public string? Subject { get; init; }
    public string? BodyOrNotes { get; init; }
    public int? DurationSeconds { get; init; }
    public byte OutcomeStatus { get; init; }
    public int LoggedBy { get; init; }
    public DateTime LoggedDate { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
