namespace Domain.Entities.Entities.CRM;

/// <summary>User-submitted master data suggestions for review.</summary>
public partial class CrmMasterDataSuggestion
{
    public int SuggestionId { get; set; }
    public string EntityType { get; set; } = null!;
    public string SuggestedValue { get; set; } = null!;
    public string? Notes { get; set; }
    /// <summary>Status: 1=Pending, 2=Approved, 3=Rejected</summary>
    public byte Status { get; set; } = 1;
    public int? ReviewedBy { get; set; }
    public DateTime? ReviewedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
