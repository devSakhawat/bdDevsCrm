namespace bdDevs.Shared.Records.CRM;

public record CreateCrmCommissionTypeRecord(
    string CommissionTypeName,
    string? CalculationMode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record UpdateCrmCommissionTypeRecord(
    int CommissionTypeId,
    string CommissionTypeName,
    string? CalculationMode,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record DeleteCrmCommissionTypeRecord(int CommissionTypeId);
