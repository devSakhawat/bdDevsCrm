namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmMasterDataSuggestionDto
{
    public int SuggestionId { get; init; }
    public string EntityType { get; init; } = string.Empty;
    public string SuggestedValue { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public byte Status { get; init; }
    public int? ReviewedBy { get; init; }
    public DateTime? ReviewedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
}
