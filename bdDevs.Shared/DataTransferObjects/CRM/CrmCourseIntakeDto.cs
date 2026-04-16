namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmCourseIntakeDto
{
    public int CourseIntakeId { get; set; }
    public int CourseId { get; set; }
    public int? MonthId { get; set; }
    public string? IntakeTitile { get; set; }
}

public class CrmCourseIntakeDDLDto
{
    public int CourseIntakeId { get; set; }
    public string? IntakeTitile { get; set; }
}
