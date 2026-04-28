namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmApplicationGridDto
{
    public int ApplicationId { get; set; }
    public string InternalRefNo { get; set; } = string.Empty;
    public int StudentId { get; set; }
    public int BranchId { get; set; }
    public int CountryId { get; set; }
    public int UniversityId { get; set; }
    public int ProgramId { get; set; }
    public int IntakeId { get; set; }
    public byte Status { get; set; }
    public byte Priority { get; set; }
    public DateTime? AppliedDate { get; set; }
    public DateTime? OfferReceivedDate { get; set; }
    public DateTime? EnrollmentDate { get; set; }
    public bool IsDeleted { get; set; }
}
