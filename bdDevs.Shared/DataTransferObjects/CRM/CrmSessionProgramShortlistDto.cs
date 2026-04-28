namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmSessionProgramShortlistDto
{
    public int SessionProgramShortlistId { get; init; }
    public int SessionId { get; init; }
    public int UniversityId { get; init; }
    public int ProgramId { get; init; }
    public int IntakeId { get; init; }
    public byte Priority { get; init; }
    public string? CounsellorNotes { get; init; }
    public byte EligibilityStatus { get; init; }
    public bool IsRecommended { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
