namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmBranchTargetDto
{
    public int BranchTargetId { get; init; }
    public int BranchId { get; init; }
    public int Year { get; init; }
    public int Month { get; init; }
    public int LeadTarget { get; init; }
    public int ConversionTarget { get; init; }
    public int ApplicationTarget { get; init; }
    public int EnrolmentTarget { get; init; }
    public decimal RevenueTarget { get; init; }
    public int CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
}
