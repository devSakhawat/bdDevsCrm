namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCountryDocumentRequirementDto
{
    public int RequirementId { get; init; }
    public int CountryId { get; init; }
    public int DegreeLevelId { get; init; }
    public string DocumentTypeName { get; init; } = string.Empty;
    public bool IsMandatory { get; init; }
    public string? Notes { get; init; }
    public int CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
}
