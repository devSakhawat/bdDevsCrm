namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCounsellingSessionRecord(int LeadId, int BranchId, int CounselorId, DateTime SessionDate, int Duration, byte SessionType, string? NeedsAssessmentNotes, decimal? BudgetDiscussed, string? TargetIntake, byte Outcome, string? OutcomeNotes, string? NextSteps, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmCounsellingSessionRecord(int CounsellingSessionId, int LeadId, int BranchId, int CounselorId, DateTime SessionDate, int Duration, byte SessionType, string? NeedsAssessmentNotes, decimal? BudgetDiscussed, string? TargetIntake, byte Outcome, string? OutcomeNotes, string? NextSteps, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmCounsellingSessionRecord(int CounsellingSessionId);
