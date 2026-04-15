namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new DMS document type.
/// </summary>
/// <param name="Name">Name of the document type.</param>
/// <param name="DocumentType">Type of the document.</param>
/// <param name="IsMandatory">Whether the document type is mandatory.</param>
/// <param name="AcceptedExtensions">Accepted file extensions for this document type.</param>
/// <param name="MaxFileSizeMb">Maximum file size in megabytes.</param>
public record CreateDmsDocumentTypeRecord(
    string Name,
    string DocumentType,
    bool IsMandatory,
    string? AcceptedExtensions,
    int? MaxFileSizeMb);

/// <summary>
/// Record for updating an existing DMS document type.
/// </summary>
/// <param name="DocumentTypeId">ID of the document type to update.</param>
/// <param name="Name">Updated name of the document type.</param>
/// <param name="DocumentType">Updated document type.</param>
/// <param name="IsMandatory">Updated mandatory flag.</param>
/// <param name="AcceptedExtensions">Updated accepted file extensions.</param>
/// <param name="MaxFileSizeMb">Updated maximum file size.</param>
public record UpdateDmsDocumentTypeRecord(
    int DocumentTypeId,
    string Name,
    string DocumentType,
    bool IsMandatory,
    string? AcceptedExtensions,
    int? MaxFileSizeMb);

/// <summary>
/// Record for deleting a DMS document type.
/// </summary>
/// <param name="DocumentTypeId">ID of the document type to delete.</param>
public record DeleteDmsDocumentTypeRecord(int DocumentTypeId);
