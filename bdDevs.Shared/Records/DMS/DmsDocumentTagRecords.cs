namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new DMS document tag.
/// </summary>
/// <param name="DocumentTagName">Name of the document tag.</param>
public record CreateDmsDocumentTagRecord(
    string DocumentTagName);

/// <summary>
/// Record for updating an existing DMS document tag.
/// </summary>
/// <param name="TagId">ID of the tag to update.</param>
/// <param name="DocumentTagName">Updated tag name.</param>
public record UpdateDmsDocumentTagRecord(
    int TagId,
    string DocumentTagName);

/// <summary>
/// Record for deleting a DMS document tag.
/// </summary>
/// <param name="TagId">ID of the tag to delete.</param>
public record DeleteDmsDocumentTagRecord(int TagId);
