namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new audit trail entry.
/// </summary>
public record CreateAuditTrailRecord(
    int UserId,
    string? ClientUser,
    string? ClientIp,
    string? Shortdescription,
    string? AuditType,
    string? AuditDescription,
    DateTime? ActionDate,
    string? RequestedUrl,
    string? AuditStatus);

/// <summary>
/// Record for updating an existing audit trail entry.
/// </summary>
public record UpdateAuditTrailRecord(
    int AuditId,
    int UserId,
    string? ClientUser,
    string? ClientIp,
    string? Shortdescription,
    string? AuditType,
    string? AuditDescription,
    DateTime? ActionDate,
    string? RequestedUrl,
    string? AuditStatus);

/// <summary>
/// Record for deleting an audit trail entry.
/// </summary>
public record DeleteAuditTrailRecord(int AuditId);
