namespace Domain.Entities.Entities.CRM;

public partial class CrmSessionProgramShortlist
{
    public int SessionProgramShortlistId { get; set; }
    public int SessionId { get; set; }
    public int UniversityId { get; set; }
    public int ProgramId { get; set; }
    public int IntakeId { get; set; }
    public byte Priority { get; set; }
    public string? CounsellorNotes { get; set; }
    public byte EligibilityStatus { get; set; }
    public bool IsRecommended { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
