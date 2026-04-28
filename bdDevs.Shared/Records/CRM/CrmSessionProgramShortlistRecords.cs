namespace bdDevs.Shared.Records.CRM;

public record CreateCrmSessionProgramShortlistRecord(int SessionId, int UniversityId, int ProgramId, int IntakeId, byte Priority, string? CounsellorNotes, byte EligibilityStatus, bool IsRecommended, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmSessionProgramShortlistRecord(int SessionProgramShortlistId, int SessionId, int UniversityId, int ProgramId, int IntakeId, byte Priority, string? CounsellorNotes, byte EligibilityStatus, bool IsRecommended, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmSessionProgramShortlistRecord(int SessionProgramShortlistId);
