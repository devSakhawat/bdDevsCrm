namespace bdDevs.Shared.DataTransferObjects.CRM;

public class CrmApplicationDocumentDto
{
    public int ApplicationDocumentId { get; set; }
    public int ApplicationId { get; set; }
    public int DocumentId { get; set; }
    public bool IsRequired { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
