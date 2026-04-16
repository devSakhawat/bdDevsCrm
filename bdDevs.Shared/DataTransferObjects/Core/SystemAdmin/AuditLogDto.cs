namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class AuditLogDto
{
    public long AuditId { get; set; }
    public int? UserId { get; set; }
    public string? Username { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? Endpoint { get; set; }
    public string? Module { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Changes { get; set; }
    public DateTime Timestamp { get; set; }
    public string? CorrelationId { get; set; }
    public string? SessionId { get; set; }
    public string? RequestId { get; set; }
    public bool Success { get; set; }
    public int? StatusCode { get; set; }
    public string? ErrorMessage { get; set; }
    public int? DurationMs { get; set; }
}

public class AuditLogDDLDto
{
    public long AuditId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityType { get; set; } = string.Empty;
}
