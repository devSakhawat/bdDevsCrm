namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmApplicationDto
{
    public int ApplicationId { get; set; }
    public int StudentId { get; set; }
    public int BranchId { get; set; }
    public int? ProcessingOfficerId { get; set; }
    public int CountryId { get; set; }
    public int UniversityId { get; set; }
    public int ProgramId { get; set; }
    public int IntakeId { get; set; }
    public string? StudentSnapshotJson { get; set; }
    public string? ProgramSnapshotJson { get; set; }
    public string? OfferSnapshotJson { get; set; }
    public string? ConditionSnapshotJson { get; set; }
    public string? MetaSnapshotJson { get; set; }
    public string? InternalRefNo { get; set; }
    public byte Status { get; set; }
    public byte Priority { get; set; }
    public DateTime? AppliedDate { get; set; }
    public DateTime? OfferReceivedDate { get; set; }
    public DateTime? EnrollmentDate { get; set; }
    public DateTime? WithdrawnDate { get; set; }
    public string? OfferDetails { get; set; }
    public string? WithdrawalReason { get; set; }
    public string? RejectionReason { get; set; }
    public string? PortalUsername { get; set; }
    public string? PortalPassword { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
