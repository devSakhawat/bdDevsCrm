namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmVisaTypeDto
{
    public int VisaTypeId { get; init; }
    public string VisaTypeName { get; init; } = string.Empty;
    public string? VisaCode { get; init; }
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}

public record CrmVisaTypeDDL
{
    public int VisaTypeId { get; init; }
    public string VisaTypeName { get; init; } = string.Empty;
    public string? VisaCode { get; init; }
}
