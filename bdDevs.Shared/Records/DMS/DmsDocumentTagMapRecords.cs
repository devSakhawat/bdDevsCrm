namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new DMS document tag mapping.
/// </summary>
/// <param name="DocumentId">ID of the document.</param>
/// <param name="TagId">ID of the tag.</param>
public record CreateDmsDocumentTagMapRecord(
    int DocumentId,
    int TagId);

/// <summary>
/// Record for updating an existing DMS document tag mapping.
/// </summary>
/// <param name="TagMapId">ID of the tag mapping to update.</param>
/// <param name="DocumentId">Updated document ID.</param>
/// <param name="TagId">Updated tag ID.</param>
public record UpdateDmsDocumentTagMapRecord(
    int TagMapId,
    int DocumentId,
    int TagId);

/// <summary>
/// Record for deleting a DMS document tag mapping.
/// </summary>
/// <param name="TagMapId">ID of the tag mapping to delete.</param>
public record DeleteDmsDocumentTagMapRecord(int TagMapId);
