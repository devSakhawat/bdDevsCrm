namespace Domain.Entities.Entities.CRM;

public partial class CrmCommunicationLog
{
    public int CommunicationLogId { get; set; }
    public byte EntityType { get; set; }
    public int EntityId { get; set; }
    public int BranchId { get; set; }
    public byte CommunicationType { get; set; }
    public string Direction { get; set; } = "Outbound";
    public string? Subject { get; set; }
    public string? BodyOrNotes { get; set; }
    public int? DurationSeconds { get; set; }
    public byte OutcomeStatus { get; set; }
    public int LoggedBy { get; set; }
    public DateTime LoggedDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
