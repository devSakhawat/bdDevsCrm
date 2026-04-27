namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCounsellingSessionDto
{
    public int CounsellingSessionId { get; init; }
    public int LeadId { get; init; }
    public int BranchId { get; init; }
    public int CounselorId { get; init; }
    public DateTime SessionDate { get; init; }
    public int Duration { get; init; }
    public byte SessionType { get; init; }
    public string? NeedsAssessmentNotes { get; init; }
    public decimal? BudgetDiscussed { get; init; }
    public string? TargetIntake { get; init; }
    public byte Outcome { get; init; }
    public string? OutcomeNotes { get; init; }
    public string? NextSteps { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
