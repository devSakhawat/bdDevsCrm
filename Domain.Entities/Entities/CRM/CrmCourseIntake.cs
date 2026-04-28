using System.ComponentModel.DataAnnotations;

namespace Domain.Entities.Entities.CRM;

public partial class CrmCourseIntake
{
    [Key]
    public int CourseIntakeId { get; set; }

    public int CourseId { get; set; }

    public int? MonthId { get; set; }

    public string? IntakeTitile { get; set; }

    // Phase 1 upgrade — intake scheduling
    public DateTime? ApplicationOpenDate { get; set; }
    public DateTime? ApplicationDeadline { get; set; }
    public DateTime? CourseStartDate { get; set; }
    public int? AvailableSeats { get; set; }
    /// <summary>Status: 1=Open, 2=Closed, 3=Waitlisted, 4=Cancelled</summary>
    public byte IntakeStatus { get; set; } = 1;
}
