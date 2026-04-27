namespace Domain.Entities.Entities.CRM;

public partial class CrmStudentDocumentChecklist
{
    public int StudentDocumentChecklistId { get; set; }
    public int StudentId { get; set; }
    public int DocumentTypeId { get; set; }
    public bool IsMandatory { get; set; }
    public bool IsSubmitted { get; set; }
    public bool IsVerified { get; set; }
    public int? RequiredByApplicationId { get; set; }
    public DateTime CreatedDate { get; set; }
    public int CreatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int? UpdatedBy { get; set; }
}
