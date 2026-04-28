namespace bdDevs.Shared.Records.CRM;

public record CreateCrmDocumentVerificationHistoryRecord(int DocumentId, byte OldStatus, byte NewStatus, int ChangedBy, DateTime ChangedDate, string? Notes);
public record UpdateCrmDocumentVerificationHistoryRecord(int DocumentVerificationHistoryId, int DocumentId, byte OldStatus, byte NewStatus, int ChangedBy, DateTime ChangedDate, string? Notes);
public record DeleteCrmDocumentVerificationHistoryRecord(int DocumentVerificationHistoryId);
