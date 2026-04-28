namespace bdDevs.Shared.Records.CRM;

public record CreateCrmStudentStatusHistoryRecord(int StudentId, int? OldStatus, int NewStatus, int ChangedBy, DateTime ChangedDate, string? Notes);
public record UpdateCrmStudentStatusHistoryRecord(int StudentStatusHistoryId, int StudentId, int? OldStatus, int NewStatus, int ChangedBy, DateTime ChangedDate, string? Notes);
public record DeleteCrmStudentStatusHistoryRecord(int StudentStatusHistoryId);
