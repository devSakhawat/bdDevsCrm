namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmAgentPerformanceDto
{
    public int AgentId { get; init; }
    public string AgentName { get; init; } = string.Empty;
    public decimal CommissionRate { get; init; }
    public int AssignedLeadCount { get; init; }
    public int TotalCommissions { get; init; }
    public int EnrolledStudentCount { get; init; }
    public int PendingCommissions { get; init; }
    public decimal TotalNetAmount { get; init; }
    public decimal TotalPaidAmount { get; init; }
    public decimal OutstandingAmount { get; init; }
}
