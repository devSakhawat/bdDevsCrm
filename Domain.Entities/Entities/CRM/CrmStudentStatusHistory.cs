namespace Domain.Entities.Entities.CRM;

public partial class CrmStudentStatusHistory
{
    public int StudentStatusHistoryId { get; set; }
    public int StudentId { get; set; }
    public int? OldStatus { get; set; }
    public int NewStatus { get; set; }
    public int ChangedBy { get; set; }
    public DateTime ChangedDate { get; set; }
    public string? Notes { get; set; }
}
