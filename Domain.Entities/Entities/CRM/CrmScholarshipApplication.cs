namespace Domain.Entities.Entities.CRM;

public partial class CrmScholarshipApplication
{
    public int ScholarshipApplicationId { get; set; }
    public int ApplicationId { get; set; }
    public string ScholarshipName { get; set; } = null!;
    public string ScholarshipType { get; set; } = null!;
    public decimal GrantedAmount { get; set; }
    public string Currency { get; set; } = "BDT";
    public decimal? ScholarshipPercentage { get; set; }
    public DateTime? ConfirmedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public byte Status { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
