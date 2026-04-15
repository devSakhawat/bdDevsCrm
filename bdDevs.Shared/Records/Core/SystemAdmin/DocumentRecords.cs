namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new document.
/// </summary>
public record CreateDocumentRecord(
    int? Hrrecordid,
    int? Documenttypeid,
    string? Titleofdocument,
    string? Attacheddocument,
    string? Summary);

/// <summary>
/// Record for updating an existing document.
/// </summary>
public record UpdateDocumentRecord(
    int Documentid,
    int? Hrrecordid,
    int? Documenttypeid,
    string? Titleofdocument,
    string? Attacheddocument,
    string? Summary);

/// <summary>
/// Record for deleting a document.
/// </summary>
public record DeleteDocumentRecord(int Documentid);
