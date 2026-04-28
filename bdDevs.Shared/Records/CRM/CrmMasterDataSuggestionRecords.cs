namespace bdDevs.Shared.Records.CRM;

public record CreateCrmMasterDataSuggestionRecord(string EntityType, string SuggestedValue, string? Notes, byte Status, int? ReviewedBy, DateTime? ReviewedDate, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmMasterDataSuggestionRecord(int SuggestionId, string EntityType, string SuggestedValue, string? Notes, byte Status, int? ReviewedBy, DateTime? ReviewedDate, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmMasterDataSuggestionRecord(int SuggestionId);
