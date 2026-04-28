namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmVisaStatusHistoryDto
{
    public int VisaStatusHistoryId { get; set; }
    public int VisaApplicationId { get; set; }
    public byte OldStatus { get; set; }
    public byte NewStatus { get; set; }
    public int ChangedBy { get; set; }
    public DateTime ChangedDate { get; set; }
    public string? Notes { get; set; }
}
