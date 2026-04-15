namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new audit log entry.
/// </summary>
public record CreateAuditLogRecord(
    int? UserId,
    string? Username,
    string? IpAddress,
    string? UserAgent,
    string Action,
    string EntityType,
    string? EntityId,
    string? Endpoint,
    string? Module,
    string? OldValue,
    string? NewValue,
    string? Changes,
    DateTime Timestamp,
    string? CorrelationId,
    string? SessionId,
    string? RequestId,
    bool Success,
    int? StatusCode,
    string? ErrorMessage,
    int? DurationMs);

/// <summary>
/// Record for updating an existing audit log entry.
/// </summary>
public record UpdateAuditLogRecord(
    long AuditId,
    int? UserId,
    string? Username,
    string? IpAddress,
    string? UserAgent,
    string Action,
    string EntityType,
    string? EntityId,
    string? Endpoint,
    string? Module,
    string? OldValue,
    string? NewValue,
    string? Changes,
    DateTime Timestamp,
    string? CorrelationId,
    string? SessionId,
    string? RequestId,
    bool Success,
    int? StatusCode,
    string? ErrorMessage,
    int? DurationMs);

/// <summary>
/// Record for deleting an audit log entry.
/// </summary>
public record DeleteAuditLogRecord(long AuditId);
