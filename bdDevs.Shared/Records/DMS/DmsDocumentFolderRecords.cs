namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new DMS document folder.
/// </summary>
/// <param name="ParentFolderId">ID of the parent folder.</param>
/// <param name="FolderName">Name of the folder.</param>
/// <param name="OwnerId">ID of the folder owner.</param>
/// <param name="ReferenceEntityType">Type of the referenced entity.</param>
/// <param name="ReferenceEntityId">ID of the referenced entity.</param>
/// <param name="DocumentId">ID of the associated document.</param>
public record CreateDmsDocumentFolderRecord(
    int? ParentFolderId,
    string FolderName,
    string? OwnerId,
    string? ReferenceEntityType,
    string? ReferenceEntityId,
    int? DocumentId);

/// <summary>
/// Record for updating an existing DMS document folder.
/// </summary>
/// <param name="FolderId">ID of the folder to update.</param>
/// <param name="ParentFolderId">Updated parent folder ID.</param>
/// <param name="FolderName">Updated folder name.</param>
/// <param name="OwnerId">Updated owner ID.</param>
/// <param name="ReferenceEntityType">Updated reference entity type.</param>
/// <param name="ReferenceEntityId">Updated reference entity ID.</param>
/// <param name="DocumentId">Updated document ID.</param>
public record UpdateDmsDocumentFolderRecord(
    int FolderId,
    int? ParentFolderId,
    string FolderName,
    string? OwnerId,
    string? ReferenceEntityType,
    string? ReferenceEntityId,
    int? DocumentId);

/// <summary>
/// Record for deleting a DMS document folder.
/// </summary>
/// <param name="FolderId">ID of the folder to delete.</param>
public record DeleteDmsDocumentFolderRecord(int FolderId);
