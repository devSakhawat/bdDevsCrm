namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new document template.
/// </summary>
public record CreateDocumentTemplateRecord(
    string DocumentTitle,
    string? DocumentText,
    string TemplateName,
    int? DocumentTypeId);

/// <summary>
/// Record for updating an existing document template.
/// </summary>
public record UpdateDocumentTemplateRecord(
    int DocumentId,
    string DocumentTitle,
    string? DocumentText,
    string TemplateName,
    int? DocumentTypeId);

/// <summary>
/// Record for deleting a document template.
/// </summary>
public record DeleteDocumentTemplateRecord(int DocumentId);
