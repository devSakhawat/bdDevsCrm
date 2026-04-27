namespace Domain.Entities.Entities.CRM;

public partial class CrmCommission
{
    public int CommissionId { get; set; }
    public int ApplicationId { get; set; }
    public int UniversityId { get; set; }
    public int? AgentId { get; set; }
    public int BranchId { get; set; }
    public string StudentNameSnapshot { get; set; } = string.Empty;
    public string UniversityNameSnapshot { get; set; } = string.Empty;
    public decimal TuitionFeeBase { get; set; }
    public decimal CommissionRate { get; set; }
    public byte CommissionType { get; set; }
    public decimal GrossAmount { get; set; }
    public decimal ScholarshipDeduction { get; set; }
    public decimal NetAmount { get; set; }
    public string Currency { get; set; } = "BDT";
    public decimal ExchangeRate { get; set; }
    public decimal NetAmountBdt { get; set; }
    public byte Status { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? PaidDate { get; set; }
    public decimal? PaidAmount { get; set; }
    public string? InvoiceNo { get; set; }
    public string? Notes { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
