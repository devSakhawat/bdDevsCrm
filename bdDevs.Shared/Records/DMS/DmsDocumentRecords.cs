namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new DMS document.
/// </summary>
/// <param name="Title">Title of the document.</param>
/// <param name="Description">Description of the document.</param>
/// <param name="FileName">Name of the file.</param>
/// <param name="FileExtension">File extension.</param>
/// <param name="FileSize">Size of the file in bytes.</param>
/// <param name="FilePath">Path to the file.</param>
/// <param name="UploadDate">Date the document was uploaded.</param>
/// <param name="UploadedByUserId">ID of the user who uploaded the document.</param>
/// <param name="DocumentTypeId">ID of the document type.</param>
/// <param name="ReferenceEntityType">Type of the referenced entity.</param>
/// <param name="ReferenceEntityId">ID of the referenced entity.</param>
/// <param name="CurrentEntityId">ID of the current entity.</param>
/// <param name="FolderId">ID of the folder.</param>
/// <param name="SystemTag">System tag for the document.</param>
public record CreateDmsDocumentRecord(
    string Title,
    string? Description,
    string FileName,
    string FileExtension,
    long FileSize,
    string FilePath,
    DateTime? UploadDate,
    string? UploadedByUserId,
    int DocumentTypeId,
    string? ReferenceEntityType,
    string? ReferenceEntityId,
    int? CurrentEntityId,
    int? FolderId,
    string? SystemTag);

/// <summary>
/// Record for updating an existing DMS document.
/// </summary>
/// <param name="DocumentId">ID of the document to update.</param>
/// <param name="Title">Updated title of the document.</param>
/// <param name="Description">Updated description of the document.</param>
/// <param name="FileName">Updated file name.</param>
/// <param name="FileExtension">Updated file extension.</param>
/// <param name="FileSize">Updated file size in bytes.</param>
/// <param name="FilePath">Updated file path.</param>
/// <param name="UploadDate">Updated upload date.</param>
/// <param name="UploadedByUserId">Updated uploader user ID.</param>
/// <param name="DocumentTypeId">Updated document type ID.</param>
/// <param name="ReferenceEntityType">Updated reference entity type.</param>
/// <param name="ReferenceEntityId">Updated reference entity ID.</param>
/// <param name="CurrentEntityId">Updated current entity ID.</param>
/// <param name="FolderId">Updated folder ID.</param>
/// <param name="SystemTag">Updated system tag.</param>
public record UpdateDmsDocumentRecord(
    int DocumentId,
    string Title,
    string? Description,
    string FileName,
    string FileExtension,
    long FileSize,
    string FilePath,
    DateTime? UploadDate,
    string? UploadedByUserId,
    int DocumentTypeId,
    string? ReferenceEntityType,
    string? ReferenceEntityId,
    int? CurrentEntityId,
    int? FolderId,
    string? SystemTag);

/// <summary>
/// Record for deleting a DMS document.
/// </summary>
/// <param name="DocumentId">ID of the document to delete.</param>
public record DeleteDmsDocumentRecord(int DocumentId);
