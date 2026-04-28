namespace bdDevs.Shared.Records.CRM;

public record CreateCrmFollowUpHistoryRecord(int FollowUpId, byte OldStatus, byte NewStatus, int ChangedBy, DateTime ChangedDate, string? Remarks);
public record UpdateCrmFollowUpHistoryRecord(int FollowUpHistoryId, int FollowUpId, byte OldStatus, byte NewStatus, int ChangedBy, DateTime ChangedDate, string? Remarks);
public record DeleteCrmFollowUpHistoryRecord(int FollowUpHistoryId);
