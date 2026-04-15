namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new document parameter.
/// </summary>
public record CreateDocumentParameterRecord(
    string ParameterName,
    string ParameterKey,
    string? ControlRole,
    string? DataSource,
    int? ControlSequence,
    string? DataTextField,
    string? DataValueField,
    string? CaseCading);

/// <summary>
/// Record for updating an existing document parameter.
/// </summary>
public record UpdateDocumentParameterRecord(
    int ParameterId,
    string ParameterName,
    string ParameterKey,
    string? ControlRole,
    string? DataSource,
    int? ControlSequence,
    string? DataTextField,
    string? DataValueField,
    string? CaseCading);

/// <summary>
/// Record for deleting a document parameter.
/// </summary>
public record DeleteDocumentParameterRecord(int ParameterId);
