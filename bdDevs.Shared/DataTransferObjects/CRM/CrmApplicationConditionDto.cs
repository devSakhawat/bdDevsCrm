namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmApplicationConditionDto
{
    public int ApplicationConditionId { get; set; }
    public int ApplicationId { get; set; }
    public string ConditionText { get; set; } = string.Empty;
    public byte ConditionType { get; set; }
    public byte Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? MetDate { get; set; }
    public int? MetBy { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
