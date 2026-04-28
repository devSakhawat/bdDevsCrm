namespace Domain.Entities.Entities.CRM;

/// <summary>Branch monthly/yearly target tracking.</summary>
public partial class CrmBranchTarget
{
    public int BranchTargetId { get; set; }
    public int BranchId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int LeadTarget { get; set; }
    public int ConversionTarget { get; set; }
    public int ApplicationTarget { get; set; }
    public int EnrolmentTarget { get; set; }
    public decimal RevenueTarget { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
