namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmAgentDto
{
    public int AgentId { get; init; }
    public string AgentName { get; init; } = string.Empty;
    public string? AgentCode { get; init; }
    public int AgentTypeId { get; init; }
    public string? ContactPerson { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public string? Address { get; init; }
    public string? Country { get; init; }
    public decimal? CommissionRate { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
