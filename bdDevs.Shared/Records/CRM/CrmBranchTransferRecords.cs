namespace bdDevs.Shared.Records.CRM;

public record CreateCrmBranchTransferRecord(
    byte EntityType,
    int EntityId,
    int FromBranchId,
    int ToBranchId,
    string? TransferReason,
    byte TransferStatus,
    int RequestedBy,
    int? ApprovedBy,
    DateTime RequestedDate,
    DateTime? ApprovedDate,
    string? Notes,
    bool IsDeleted,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record UpdateCrmBranchTransferRecord(
    int TransferId,
    byte EntityType,
    int EntityId,
    int FromBranchId,
    int ToBranchId,
    string? TransferReason,
    byte TransferStatus,
    int RequestedBy,
    int? ApprovedBy,
    DateTime RequestedDate,
    DateTime? ApprovedDate,
    string? Notes,
    bool IsDeleted,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record DeleteCrmBranchTransferRecord(int TransferId);

public record ApproveCrmBranchTransferRecord(int TransferId, int ApprovedBy, DateTime ApprovedDate, string? Notes);
