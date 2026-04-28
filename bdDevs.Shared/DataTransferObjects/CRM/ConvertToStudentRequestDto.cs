namespace bdDevs.Shared.DataTransferObjects.CRM;

public class ConvertToStudentRequestDto
{
    public int LeadId { get; set; }
    public int? ProcessingOfficerId { get; set; }
    public int? PreferredCountryId { get; set; }
    public int? PreferredDegreeLevelId { get; set; }
    public string? DesiredIntake { get; set; }
    public string? PassportNumber { get; set; }
    public int? RequestedBy { get; set; }
    public bool ForceProceed { get; set; }
}
