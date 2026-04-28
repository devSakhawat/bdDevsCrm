namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCommissionDto
{
    public int CommissionId { get; init; }
    public int ApplicationId { get; init; }
    public int UniversityId { get; init; }
    public int? AgentId { get; init; }
    public int BranchId { get; init; }
    public string StudentNameSnapshot { get; init; } = string.Empty;
    public string UniversityNameSnapshot { get; init; } = string.Empty;
    public decimal TuitionFeeBase { get; init; }
    public decimal CommissionRate { get; init; }
    public byte CommissionType { get; init; }
    public decimal GrossAmount { get; init; }
    public decimal ScholarshipDeduction { get; init; }
    public decimal NetAmount { get; init; }
    public string Currency { get; init; } = "BDT";
    public decimal ExchangeRate { get; init; }
    public decimal NetAmountBdt { get; init; }
    public byte Status { get; init; }
    public DateTime? DueDate { get; init; }
    public DateTime? PaidDate { get; init; }
    public decimal? PaidAmount { get; init; }
    public string? InvoiceNo { get; init; }
    public string? Notes { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
