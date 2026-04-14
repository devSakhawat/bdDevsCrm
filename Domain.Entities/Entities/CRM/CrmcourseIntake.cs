using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Entities.CRM;

public partial class CrmCourseIntake
{
    [Key]
    public int CourseIntakeId { get; set; }

    public int CourseId { get; set; }

    public int? MonthId { get; set; }

    public string? IntakeTitile { get; set; }
}
