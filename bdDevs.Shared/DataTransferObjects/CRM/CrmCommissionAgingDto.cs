namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCommissionAgingDto
{
    public int CommissionId { get; init; }
    public int ApplicationId { get; init; }
    public int? AgentId { get; init; }
    public string StudentNameSnapshot { get; init; } = string.Empty;
    public string UniversityNameSnapshot { get; init; } = string.Empty;
    public string? InvoiceNo { get; init; }
    public DateTime? DueDate { get; init; }
    public int AgingDays { get; init; }
    public decimal NetAmount { get; init; }
    public decimal PaidAmount { get; init; }
    public decimal OutstandingAmount { get; init; }
    public byte Status { get; init; }
}
