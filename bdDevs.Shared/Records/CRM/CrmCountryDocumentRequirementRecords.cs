namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCountryDocumentRequirementRecord(int CountryId, int DegreeLevelId, string DocumentTypeName, bool IsMandatory, string? Notes, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmCountryDocumentRequirementRecord(int RequirementId, int CountryId, int DegreeLevelId, string DocumentTypeName, bool IsMandatory, string? Notes, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmCountryDocumentRequirementRecord(int RequirementId);
