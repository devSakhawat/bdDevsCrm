namespace bdDevs.Shared.Records.CRM;

public record CreateCrmApplicationDocumentRecord(int ApplicationId, int DocumentId, bool IsRequired, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmApplicationDocumentRecord(int ApplicationDocumentId, int ApplicationId, int DocumentId, bool IsRequired, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmApplicationDocumentRecord(int ApplicationDocumentId);
