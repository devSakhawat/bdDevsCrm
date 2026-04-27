namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmLeadStatusDto
{
    public int LeadStatusId { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public string? StatusCode { get; init; }
    public string? ColorCode { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}

public record CrmLeadStatusDDL
{
    public int LeadStatusId { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public string? ColorCode { get; init; }
}
