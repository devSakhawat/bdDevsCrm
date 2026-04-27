namespace Domain.Entities.Entities.CRM;

public partial class CrmCountryDocumentRequirement
{
    public int RequirementId { get; set; }
    public int CountryId { get; set; }
    public int DegreeLevelId { get; set; }
    public string DocumentTypeName { get; set; } = null!;
    public bool IsMandatory { get; set; }
    public string? Notes { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
