namespace Domain.Entities.Entities.CRM;

public partial class CrmCounsellingSession
{
    public int CounsellingSessionId { get; set; }
    public int LeadId { get; set; }
    public int BranchId { get; set; }
    public int CounselorId { get; set; }
    public DateTime SessionDate { get; set; }
    public int Duration { get; set; }
    public byte SessionType { get; set; }
    public string? NeedsAssessmentNotes { get; set; }
    public decimal? BudgetDiscussed { get; set; }
    public string? TargetIntake { get; set; }
    public byte Outcome { get; set; }
    public string? OutcomeNotes { get; set; }
    public string? NextSteps { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
