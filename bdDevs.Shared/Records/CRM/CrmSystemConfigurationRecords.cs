namespace bdDevs.Shared.Records.CRM;

public record CreateCrmSystemConfigurationRecord(string ConfigKey, string ConfigValue, string? Description, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmSystemConfigurationRecord(int ConfigId, string ConfigKey, string ConfigValue, string? Description, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmSystemConfigurationRecord(int ConfigId);
