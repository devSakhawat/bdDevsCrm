namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmAgentTypeDto
{
    public int AgentTypeId { get; init; }
    public string AgentTypeName { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}

public record CrmAgentTypeDDL
{
    public int AgentTypeId { get; init; }
    public string AgentTypeName { get; init; } = string.Empty;
}
