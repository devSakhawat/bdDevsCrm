namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmStudentDto
{
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public string? StudentCode { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public int? LeadId { get; init; }
    public int? StudentStatusId { get; init; }
    public int? AgentId { get; init; }
    public int? CounselorId { get; init; }
    public int? BranchId { get; init; }
    public int? ProcessingOfficerId { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public byte? Gender { get; init; }
    public string? PassportNumber { get; init; }
    public DateTime? PassportExpiryDate { get; init; }
    public DateTime? PassportIssueDate { get; init; }
    public int? PassportIssueCountryId { get; init; }
    public int? VisaTypeId { get; init; }
    public string? Nationality { get; init; }
    public int? NationalityCountryId { get; init; }
    public string? EmergencyContactName { get; init; }
    public string? EmergencyContactPhone { get; init; }
    public string? EmergencyContactRelation { get; init; }
    public int? PreferredCountryId { get; init; }
    public int? PreferredDegreeLevelId { get; init; }
    public string? DesiredIntake { get; init; }
    public byte? IeltsStatus { get; init; }
    public decimal? IeltsScore { get; init; }
    public DateTime? IeltsExamDate { get; init; }
    public bool IsApplicationReady { get; init; }
    public DateTime? ApplicationReadyDate { get; init; }
    public int? ApplicationReadySetBy { get; init; }
    public bool ConsentPersonalData { get; init; }
    public bool ConsentMarketing { get; init; }
    public bool ConsentDocumentProcessing { get; init; }
    public bool ConsentInternationalSharing { get; init; }
    public bool ConsentTermsAccepted { get; init; }
    public bool IsDeleted { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
