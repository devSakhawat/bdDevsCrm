namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM course intake.
/// </summary>
public record CreateCrmCourseIntakeRecord(
    int CourseId,
    int? MonthId,
    string? IntakeTitile);

/// <summary>
/// Record for updating an existing CRM course intake.
/// </summary>
public record UpdateCrmCourseIntakeRecord(
    int CourseIntakeId,
    int CourseId,
    int? MonthId,
    string? IntakeTitile);

/// <summary>
/// Record for deleting a CRM course intake.
/// </summary>
/// <param name="CourseIntakeId">ID of the course intake to delete.</param>
public record DeleteCrmCourseIntakeRecord(int CourseIntakeId);
