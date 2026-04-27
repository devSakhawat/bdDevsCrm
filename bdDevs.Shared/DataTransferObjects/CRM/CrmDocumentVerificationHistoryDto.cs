namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmDocumentVerificationHistoryDto
{
    public int DocumentVerificationHistoryId { get; set; }
    public int DocumentId { get; set; }
    public byte OldStatus { get; set; }
    public byte NewStatus { get; set; }
    public int ChangedBy { get; set; }
    public DateTime ChangedDate { get; set; }
    public string? Notes { get; set; }
}
