namespace bdDevs.Shared.Records.CRM;

public record CreateCrmVisaStatusHistoryRecord(int VisaApplicationId, byte OldStatus, byte NewStatus, int ChangedBy, DateTime ChangedDate, string? Notes);
public record UpdateCrmVisaStatusHistoryRecord(int VisaStatusHistoryId, int VisaApplicationId, byte OldStatus, byte NewStatus, int ChangedBy, DateTime ChangedDate, string? Notes);
public record DeleteCrmVisaStatusHistoryRecord(int VisaStatusHistoryId);
