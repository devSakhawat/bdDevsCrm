namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmBranchTransferDto
{
    public int TransferId { get; init; }
    public byte EntityType { get; init; }
    public int EntityId { get; init; }
    public int FromBranchId { get; init; }
    public int ToBranchId { get; init; }
    public string? TransferReason { get; init; }
    public byte TransferStatus { get; init; }
    public int RequestedBy { get; init; }
    public int? ApprovedBy { get; init; }
    public DateTime RequestedDate { get; init; }
    public DateTime? ApprovedDate { get; init; }
    public string? Notes { get; init; }
    public bool IsDeleted { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
