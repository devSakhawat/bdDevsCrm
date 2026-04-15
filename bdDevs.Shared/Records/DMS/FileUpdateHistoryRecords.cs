namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new file update history entry.
/// </summary>
/// <param name="EntityId">ID of the entity.</param>
/// <param name="EntityType">Type of the entity.</param>
/// <param name="DocumentType">Type of the document.</param>
/// <param name="OldFilePath">Path to the old file.</param>
/// <param name="NewFilePath">Path to the new file.</param>
/// <param name="VersionNumber">Version number.</param>
/// <param name="UpdatedBy">User who updated the file.</param>
/// <param name="UpdatedDate">Date the file was updated.</param>
/// <param name="UpdateReason">Reason for the update.</param>
/// <param name="Notes">Additional notes about the update.</param>
public record CreateFileUpdateHistoryRecord(
    string? EntityId,
    string? EntityType,
    string? DocumentType,
    string? OldFilePath,
    string? NewFilePath,
    int? VersionNumber,
    string? UpdatedBy,
    DateTime? UpdatedDate,
    string? UpdateReason,
    string? Notes);

/// <summary>
/// Record for updating an existing file update history entry.
/// </summary>
/// <param name="Id">ID of the history entry to update.</param>
/// <param name="EntityId">Updated entity ID.</param>
/// <param name="EntityType">Updated entity type.</param>
/// <param name="DocumentType">Updated document type.</param>
/// <param name="OldFilePath">Updated old file path.</param>
/// <param name="NewFilePath">Updated new file path.</param>
/// <param name="VersionNumber">Updated version number.</param>
/// <param name="UpdatedBy">Updated user who updated the file.</param>
/// <param name="UpdatedDate">Updated update date.</param>
/// <param name="UpdateReason">Updated update reason.</param>
/// <param name="Notes">Updated notes.</param>
public record UpdateFileUpdateHistoryRecord(
    int Id,
    string? EntityId,
    string? EntityType,
    string? DocumentType,
    string? OldFilePath,
    string? NewFilePath,
    int? VersionNumber,
    string? UpdatedBy,
    DateTime? UpdatedDate,
    string? UpdateReason,
    string? Notes);

/// <summary>
/// Record for deleting a file update history entry.
/// </summary>
/// <param name="Id">ID of the history entry to delete.</param>
public record DeleteFileUpdateHistoryRecord(int Id);
