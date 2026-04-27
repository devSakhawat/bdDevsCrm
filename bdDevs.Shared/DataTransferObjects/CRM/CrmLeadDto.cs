namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmLeadDto
{
    public int LeadId { get; init; }
    public string LeadName { get; init; } = string.Empty;
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public int? LeadSourceId { get; init; }
    public int? LeadStatusId { get; init; }
    public int? AssignedCounselorId { get; init; }
    public int? AgentId { get; init; }
    public string? CountryOfInterest { get; init; }
    public string? CourseOfInterest { get; init; }
    public decimal? Budget { get; init; }
    public string? Notes { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
