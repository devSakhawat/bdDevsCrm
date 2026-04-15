namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM additional document.
/// </summary>
public record CreateCrmAdditionalDocumentRecord(
    int? ApplicantId,
    string DocumentTitle,
    string DocumentPath,
    string DocumentName,
    string RecordType,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM additional document.
/// </summary>
public record UpdateCrmAdditionalDocumentRecord(
    int AdditionalDocumentId,
    int? ApplicantId,
    string DocumentTitle,
    string DocumentPath,
    string DocumentName,
    string RecordType,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM additional document.
/// </summary>
/// <param name="AdditionalDocumentId">ID of the additional document to delete.</param>
public record DeleteCrmAdditionalDocumentRecord(int AdditionalDocumentId);
