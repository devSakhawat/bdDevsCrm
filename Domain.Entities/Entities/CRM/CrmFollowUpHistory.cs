namespace Domain.Entities.Entities.CRM;

public partial class CrmFollowUpHistory
{
    public int FollowUpHistoryId { get; set; }
    public int FollowUpId { get; set; }
    public byte OldStatus { get; set; }
    public byte NewStatus { get; set; }
    public int ChangedBy { get; set; }
    public DateTime ChangedDate { get; set; }
    public string? Remarks { get; set; }
}
