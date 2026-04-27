namespace Domain.Entities.Entities.CRM;

public partial class CrmStudentDocument
{
    public int StudentDocumentId { get; set; }
    public int StudentId { get; set; }
    public int DocumentTypeId { get; set; }
    public int BranchId { get; set; }
    public string OriginalFileName { get; set; } = null!;
    public string StoredFileName { get; set; } = null!;
    public decimal FileSizeKb { get; set; }
    public string MimeType { get; set; } = null!;
    public byte Status { get; set; }
    public string? RejectionReason { get; set; }
    public int? VerifiedBy { get; set; }
    public DateTime? VerifiedDate { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
