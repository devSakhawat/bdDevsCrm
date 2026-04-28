namespace Domain.Entities.Entities.CRM;

/// <summary>Key-value system configuration store (e.g. RenewalReminderDays, MaxFollowUpAge).</summary>
public partial class CrmSystemConfiguration
{
    public int ConfigId { get; set; }
    public string ConfigKey { get; set; } = null!;
    public string ConfigValue { get; set; } = null!;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public int? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
