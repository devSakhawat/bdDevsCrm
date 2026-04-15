namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new document type.
/// </summary>
public record CreateDocumentTypeRecord(
    string? Documentname,
    DateTime? Initiationdate,
    string? Description,
    int? UseType);

/// <summary>
/// Record for updating an existing document type.
/// </summary>
public record UpdateDocumentTypeRecord(
    int Documenttypeid,
    string? Documentname,
    DateTime? Initiationdate,
    string? Description,
    int? UseType);

/// <summary>
/// Record for deleting a document type.
/// </summary>
public record DeleteDocumentTypeRecord(int Documenttypeid);
