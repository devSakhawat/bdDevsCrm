namespace bdDevs.Shared.Records.CRM;

public record CreateCrmScholarshipApplicationRecord(int ApplicationId, string ScholarshipName, string ScholarshipType, decimal GrantedAmount, string Currency, decimal? ScholarshipPercentage, DateTime? ConfirmedDate, DateTime? ExpiryDate, byte Status, string? Notes, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record UpdateCrmScholarshipApplicationRecord(int ScholarshipApplicationId, int ApplicationId, string ScholarshipName, string ScholarshipType, decimal GrantedAmount, string Currency, decimal? ScholarshipPercentage, DateTime? ConfirmedDate, DateTime? ExpiryDate, byte Status, string? Notes, DateTime CreatedDate, int CreatedBy, DateTime? UpdatedDate, int? UpdatedBy);
public record DeleteCrmScholarshipApplicationRecord(int ScholarshipApplicationId);
