namespace Domain.Entities.Entities.CRM;

public partial class CrmVisaApplication
{
    public int VisaApplicationId { get; set; }
    public int ApplicationId { get; set; }
    public int StudentId { get; set; }
    public int BranchId { get; set; }
    public int VisaCountryId { get; set; }
    public string? EmbassyName { get; set; }
    public string? ApplicationRefNo { get; set; }
    public byte Status { get; set; }
    public DateTime? SubmittedDate { get; set; }
    public DateTime? BiometricDate { get; set; }
    public DateTime? InterviewDate { get; set; }
    public DateTime? DecisionDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public string? RefusalReason { get; set; }
    public string? Notes { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
