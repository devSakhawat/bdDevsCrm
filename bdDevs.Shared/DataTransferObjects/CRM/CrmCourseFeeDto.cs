namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCourseFeeDto
{
    public int CourseFeeId { get; init; }
    public int CourseId { get; init; }
    public int IntakeId { get; init; }
    public byte FeeType { get; init; }
    public decimal Amount { get; init; }
    public string Currency { get; init; } = "BDT";
    public byte PaymentSchedule { get; init; }
    public string? Notes { get; init; }
    public int CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
}
