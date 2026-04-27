namespace bdDevs.Shared.Records.CRM;

public record CreateCrmStudentDocumentChecklistRecord(int StudentId, int DocumentTypeId, bool IsMandatory, bool IsSubmitted, bool IsVerified, int? RequiredByApplicationId, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmStudentDocumentChecklistRecord(int StudentDocumentChecklistId, int StudentId, int DocumentTypeId, bool IsMandatory, bool IsSubmitted, bool IsVerified, int? RequiredByApplicationId, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmStudentDocumentChecklistRecord(int StudentDocumentChecklistId);
