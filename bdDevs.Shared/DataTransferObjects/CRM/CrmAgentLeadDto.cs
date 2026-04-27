namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmAgentLeadDto
{
    public int AgentLeadId { get; init; }
    public int AgentId { get; init; }
    public int LeadId { get; init; }
    public DateTime AssignedDate { get; init; }
    public int AssignedBy { get; init; }
    public string? Notes { get; init; }
    public bool IsActive { get; init; }
    public int CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
}
