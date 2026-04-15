namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new marital status.
/// </summary>
public record CreateMaritalStatusRecord(
    string? MaritalStatusName,
    int? IsActive);

/// <summary>
/// Record for updating an existing marital status.
/// </summary>
public record UpdateMaritalStatusRecord(
    int MaritalStatusId,
    string? MaritalStatusName,
    int? IsActive);

/// <summary>
/// Record for deleting a marital status.
/// </summary>
public record DeleteMaritalStatusRecord(int MaritalStatusId);
