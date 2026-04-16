namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCourseIntakeRecord(
    int CourseId,
    int? MonthId,
    string? IntakeTitile);

public record UpdateCrmCourseIntakeRecord(
    int CourseIntakeId,
    int CourseId,
    int? MonthId,
    string? IntakeTitile);

public record DeleteCrmCourseIntakeRecord(int CourseIntakeId);
