namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new audit type.
/// </summary>
public record CreateAuditTypeRecord(
    int? AuditTypeId,
    string? AuditType1);

/// <summary>
/// Record for updating an existing audit type.
/// </summary>
public record UpdateAuditTypeRecord(
    int? AuditTypeId,
    string? AuditType1);

/// <summary>
/// Record for deleting an audit type.
/// </summary>
public record DeleteAuditTypeRecord(int AuditTypeId);
