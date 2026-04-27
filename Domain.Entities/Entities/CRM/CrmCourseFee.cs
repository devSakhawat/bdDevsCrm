namespace Domain.Entities.Entities.CRM;

/// <summary>
/// Represents fee configuration for a course/intake combination.
/// FeeType: 1=Tuition, 2=Registration, 3=Accommodation, 4=Insurance, 5=Materials
/// PaymentSchedule: 1=PerYear, 2=PerSemester, 3=OneTime
/// </summary>
public partial class CrmCourseFee
{
    public int CourseFeeId { get; set; }
    public int CourseId { get; set; }
    public int IntakeId { get; set; }
    public byte FeeType { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "BDT";
    public byte PaymentSchedule { get; set; }
    public string? Notes { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
