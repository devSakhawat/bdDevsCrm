namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmOfficeDto
{
    public int OfficeId { get; init; }
    public string OfficeName { get; init; } = string.Empty;
    public string? OfficeCode { get; init; }
    public string? Address { get; init; }
    public string? City { get; init; }
    public string? Phone { get; init; }
    public string? Email { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}

public record CrmOfficeDDL
{
    public int OfficeId { get; init; }
    public string OfficeName { get; init; } = string.Empty;
    public string? OfficeCode { get; init; }
}
