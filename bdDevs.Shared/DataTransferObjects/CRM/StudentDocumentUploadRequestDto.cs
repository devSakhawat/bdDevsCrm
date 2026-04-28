namespace bdDevs.Shared.DataTransferObjects.CRM;

public class StudentDocumentUploadRequestDto
{
    public int StudentId { get; set; }
    public int DocumentTypeId { get; set; }
    public int BranchId { get; set; }
    public DateTime? ExpiryDate { get; set; }
    public int RequestedBy { get; set; }
}
