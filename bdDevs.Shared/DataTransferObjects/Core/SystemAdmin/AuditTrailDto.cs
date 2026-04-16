namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class AuditTrailDto
{
    public int AuditId { get; set; }
    public int UserId { get; set; }
    public string? ClientUser { get; set; }
    public string? ClientIp { get; set; }
    public string? Shortdescription { get; set; }
    public string? AuditType { get; set; }
    public string? AuditDescription { get; set; }
    public DateTime? ActionDate { get; set; }
    public string? RequestedUrl { get; set; }
    public string? AuditStatus { get; set; }
}

public class AuditTrailDDLDto
{
    public int AuditId { get; set; }
    public string? Shortdescription { get; set; }
    public string? AuditType { get; set; }
}
