namespace bdDevs.Shared.Records.CRM;

public record CreateCrmDegreeLevelRecord(string Name, int SortOrder, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmDegreeLevelRecord(int DegreeLevelId, string Name, int SortOrder, bool IsActive, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmDegreeLevelRecord(int DegreeLevelId);
