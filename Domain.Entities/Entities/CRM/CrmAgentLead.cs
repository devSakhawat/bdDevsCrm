namespace Domain.Entities.Entities.CRM;

/// <summary>Assigns one agent per lead (unique constraint on LeadId).</summary>
public partial class CrmAgentLead
{
    public int AgentLeadId { get; set; }
    public int AgentId { get; set; }
    public int LeadId { get; set; }
    public DateTime AssignedDate { get; set; }
    public int AssignedBy { get; set; }
    public string? Notes { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
