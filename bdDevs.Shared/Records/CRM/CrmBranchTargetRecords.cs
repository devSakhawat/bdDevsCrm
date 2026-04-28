namespace bdDevs.Shared.Records.CRM;

public record CreateCrmBranchTargetRecord(int BranchId, int Year, int Month, int LeadTarget, int ConversionTarget, int ApplicationTarget, int EnrolmentTarget, decimal RevenueTarget, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record UpdateCrmBranchTargetRecord(int BranchTargetId, int BranchId, int Year, int Month, int LeadTarget, int ConversionTarget, int ApplicationTarget, int EnrolmentTarget, decimal RevenueTarget, int CreatedBy, DateTime CreatedDate, int? UpdatedBy, DateTime? UpdatedDate);
public record DeleteCrmBranchTargetRecord(int BranchTargetId);
