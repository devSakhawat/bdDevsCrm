namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCourseFeeRecord(int CourseId, int IntakeId, byte FeeType, decimal Amount, string Currency, byte PaymentSchedule, string? Notes, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmCourseFeeRecord(int CourseFeeId, int CourseId, int IntakeId, byte FeeType, decimal Amount, string Currency, byte PaymentSchedule, string? Notes, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmCourseFeeRecord(int CourseFeeId);
