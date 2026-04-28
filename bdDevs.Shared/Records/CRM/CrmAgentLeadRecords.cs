namespace bdDevs.Shared.Records.CRM;

public record CreateCrmAgentLeadRecord(int AgentId, int LeadId, DateTime AssignedDate, int AssignedBy, string? Notes, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmAgentLeadRecord(int AgentLeadId, int AgentId, int LeadId, DateTime AssignedDate, int AssignedBy, string? Notes, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmAgentLeadRecord(int AgentLeadId);
