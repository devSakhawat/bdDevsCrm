namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new document query mapping.
/// </summary>
public record CreateDocumentQueryMappingRecord(
    int ReportHeaderId,
    int DocumentTypeId,
    string? ParameterDefination);

/// <summary>
/// Record for updating an existing document query mapping.
/// </summary>
public record UpdateDocumentQueryMappingRecord(
    int DocumentQueryId,
    int ReportHeaderId,
    int DocumentTypeId,
    string? ParameterDefination);

/// <summary>
/// Record for deleting a document query mapping.
/// </summary>
public record DeleteDocumentQueryMappingRecord(int DocumentQueryId);
