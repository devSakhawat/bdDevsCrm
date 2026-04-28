namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCommunicationLogRecord(byte EntityType, int EntityId, int BranchId, byte CommunicationType, string Direction, string? Subject, string? BodyOrNotes, int? DurationSeconds, byte OutcomeStatus, int LoggedBy, DateTime LoggedDate, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmCommunicationLogRecord(int CommunicationLogId, byte EntityType, int EntityId, int BranchId, byte CommunicationType, string Direction, string? Subject, string? BodyOrNotes, int? DurationSeconds, byte OutcomeStatus, int LoggedBy, DateTime LoggedDate, bool IsDeleted, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmCommunicationLogRecord(int CommunicationLogId);
