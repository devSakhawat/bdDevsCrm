namespace bdDevs.Shared.Records.CRM;

public record CreateCrmStudentDocumentRecord(
    int StudentId,
    int DocumentTypeId,
    int BranchId,
    string OriginalFileName,
    string StoredFileName,
    decimal FileSizeKb,
    string MimeType,
    byte Status,
    string? RejectionReason,
    int? VerifiedBy,
    DateTime? VerifiedDate,
    DateTime? ExpiryDate,
    bool IsDeleted,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record UpdateCrmStudentDocumentRecord(
    int StudentDocumentId,
    int StudentId,
    int DocumentTypeId,
    int BranchId,
    string OriginalFileName,
    string StoredFileName,
    decimal FileSizeKb,
    string MimeType,
    byte Status,
    string? RejectionReason,
    int? VerifiedBy,
    DateTime? VerifiedDate,
    DateTime? ExpiryDate,
    bool IsDeleted,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record DeleteCrmStudentDocumentRecord(int StudentDocumentId);
public record ChangeCrmStudentDocumentStatusRecord(int StudentDocumentId, byte NewStatus, int ChangedBy, string? Notes, string? RejectionReason);
