namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmAgentCommissionSummaryDto
{
    public int AgentId { get; init; }
    public string AgentName { get; init; } = string.Empty;
    public int TotalCommissions { get; init; }
    public int EnrolledStudentCount { get; init; }
    public int PendingCommissions { get; init; }
    public decimal GrossAmount { get; init; }
    public decimal NetAmount { get; init; }
    public decimal PaidAmount { get; init; }
    public decimal OutstandingAmount { get; init; }
}
