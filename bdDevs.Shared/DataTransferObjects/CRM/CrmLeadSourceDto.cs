namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmLeadSourceDto
{
    public int LeadSourceId { get; init; }
    public string SourceName { get; init; } = string.Empty;
    public string? SourceCode { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}

public record CrmLeadSourceDDL
{
    public int LeadSourceId { get; init; }
    public string SourceName { get; init; } = string.Empty;
}
