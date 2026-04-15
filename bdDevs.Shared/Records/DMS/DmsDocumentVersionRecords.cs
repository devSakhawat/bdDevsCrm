namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new DMS document version.
/// </summary>
/// <param name="DocumentId">ID of the document.</param>
/// <param name="VersionNumber">Version number.</param>
/// <param name="FileName">Name of the file.</param>
/// <param name="FilePath">Path to the file.</param>
/// <param name="UploadedDate">Date the version was uploaded.</param>
/// <param name="UploadedBy">User who uploaded the version.</param>
/// <param name="IsCurrentVersion">Whether this is the current version.</param>
/// <param name="VersionNotes">Notes about the version.</param>
/// <param name="PreviousVersionId">ID of the previous version.</param>
/// <param name="FileSize">Size of the file in bytes.</param>
public record CreateDmsDocumentVersionRecord(
    int DocumentId,
    int VersionNumber,
    string FileName,
    string FilePath,
    DateTime? UploadedDate,
    string? UploadedBy,
    bool? IsCurrentVersion,
    string? VersionNotes,
    int? PreviousVersionId,
    long? FileSize);

/// <summary>
/// Record for updating an existing DMS document version.
/// </summary>
/// <param name="VersionId">ID of the version to update.</param>
/// <param name="DocumentId">Updated document ID.</param>
/// <param name="VersionNumber">Updated version number.</param>
/// <param name="FileName">Updated file name.</param>
/// <param name="FilePath">Updated file path.</param>
/// <param name="UploadedDate">Updated upload date.</param>
/// <param name="UploadedBy">Updated uploader.</param>
/// <param name="IsCurrentVersion">Updated current version flag.</param>
/// <param name="VersionNotes">Updated version notes.</param>
/// <param name="PreviousVersionId">Updated previous version ID.</param>
/// <param name="FileSize">Updated file size.</param>
public record UpdateDmsDocumentVersionRecord(
    int VersionId,
    int DocumentId,
    int VersionNumber,
    string FileName,
    string FilePath,
    DateTime? UploadedDate,
    string? UploadedBy,
    bool? IsCurrentVersion,
    string? VersionNotes,
    int? PreviousVersionId,
    long? FileSize);

/// <summary>
/// Record for deleting a DMS document version.
/// </summary>
/// <param name="VersionId">ID of the version to delete.</param>
public record DeleteDmsDocumentVersionRecord(int VersionId);
