namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmSystemConfigurationDto
{
    public int ConfigId { get; init; }
    public string ConfigKey { get; init; } = string.Empty;
    public string ConfigValue { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsActive { get; init; }
    public int CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
}
