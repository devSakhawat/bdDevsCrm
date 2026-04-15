namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new document parameter mapping.
/// </summary>
public record CreateDocumentParameterMappingRecord(
    int? DocumentTypeId,
    int? ParameterId,
    bool? IsVisible);

/// <summary>
/// Record for updating an existing document parameter mapping.
/// </summary>
public record UpdateDocumentParameterMappingRecord(
    int MappingId,
    int? DocumentTypeId,
    int? ParameterId,
    bool? IsVisible);

/// <summary>
/// Record for deleting a document parameter mapping.
/// </summary>
public record DeleteDocumentParameterMappingRecord(int MappingId);
