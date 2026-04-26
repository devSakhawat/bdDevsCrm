    namespace bdDevs.Shared.Records.CRM;

    public record CreateCrmDocumentTypeRecord(
        string DocumentTypeName,
string? Code,
bool IsMandatoryForApplication,
bool IsActive,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record UpdateCrmDocumentTypeRecord(
        int DocumentTypeId,
string DocumentTypeName,
string? Code,
bool IsMandatoryForApplication,
bool IsActive,
DateTime CreatedDate,
int CreatedBy,
DateTime? UpdatedDate,
int? UpdatedBy);

    public record DeleteCrmDocumentTypeRecord(int DocumentTypeId);
