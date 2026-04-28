namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCommissionDashboardDto
{
    public int PendingCount { get; init; }
    public int DueCount { get; init; }
    public int InvoicedCount { get; init; }
    public int PaidCount { get; init; }
    public int DisputedCount { get; init; }
    public int WrittenOffCount { get; init; }
    public decimal TotalNetAmount { get; init; }
    public decimal TotalNetAmountBdt { get; init; }
    public decimal TotalPaidAmount { get; init; }
    public decimal TotalOutstandingAmount { get; init; }
}
