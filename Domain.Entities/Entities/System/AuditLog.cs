using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities.Entities.System;

[Table("AuditLogs", Schema = "dbo")]
public class AuditLog
{
    [Key]
    public long AuditId { get; set; }

    // Who
    public int? UserId { get; set; }

    [MaxLength(100)]
    public string? Username { get; set; }

    [MaxLength(50)]
    public string? IpAddress { get; set; }

    [MaxLength(500)]
    public string? UserAgent { get; set; }

    // What
    [Required]
    [MaxLength(50)]
    public string Action { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string EntityType { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? EntityId { get; set; }

    [MaxLength(200)]
    public string? Endpoint { get; set; }

    [MaxLength(100)]
    public string? Module { get; set; }

    // Details
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public string? Changes { get; set; }

    // When
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;

    // Context
    [MaxLength(100)]
    public string? CorrelationId { get; set; }

    [MaxLength(100)]
    public string? SessionId { get; set; }

    [MaxLength(100)]
    public string? RequestId { get; set; }

    // Result
    public bool Success { get; set; } = true;
    public int? StatusCode { get; set; }

    [MaxLength(2000)]
    public string? ErrorMessage { get; set; }

    public int? DurationMs { get; set; }

    // Relationships
    [ForeignKey(nameof(UserId))]
    public virtual Users? User { get; set; }
}
