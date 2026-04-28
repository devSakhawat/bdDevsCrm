namespace bdDevs.Shared.Records.CRM;

public record CreateCrmVisaApplicationRecord(int ApplicationId, int StudentId, int BranchId, int VisaCountryId, string? EmbassyName, string? ApplicationRefNo, byte Status, DateTime? SubmittedDate, DateTime? BiometricDate, DateTime? InterviewDate, DateTime? DecisionDate, DateTime? ExpiryDate, string? RefusalReason, string? Notes, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmVisaApplicationRecord(int VisaApplicationId, int ApplicationId, int StudentId, int BranchId, int VisaCountryId, string? EmbassyName, string? ApplicationRefNo, byte Status, DateTime? SubmittedDate, DateTime? BiometricDate, DateTime? InterviewDate, DateTime? DecisionDate, DateTime? ExpiryDate, string? RefusalReason, string? Notes, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmVisaApplicationRecord(int VisaApplicationId);
public record ChangeCrmVisaApplicationStatusRecord(int VisaApplicationId, byte NewStatus, int ChangedBy, string? Notes, string? RefusalReason);
