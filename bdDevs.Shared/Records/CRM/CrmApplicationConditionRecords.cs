namespace bdDevs.Shared.Records.CRM;

public record CreateCrmApplicationConditionRecord(int ApplicationId, string ConditionText, byte ConditionType, byte Status, DateTime? DueDate, DateTime? MetDate, int? MetBy, string? Notes, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmApplicationConditionRecord(int ApplicationConditionId, int ApplicationId, string ConditionText, byte ConditionType, byte Status, DateTime? DueDate, DateTime? MetDate, int? MetBy, string? Notes, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmApplicationConditionRecord(int ApplicationConditionId);
public record ChangeCrmApplicationConditionStatusRecord(int ApplicationConditionId, byte Status, int ChangedBy, string? Notes);
