using System;

namespace Domain.Entities.Entities.CRM;

public partial class CrmBranchTransfer
{
    public int TransferId { get; set; }

    public byte EntityType { get; set; }

    public int EntityId { get; set; }

    public int FromBranchId { get; set; }

    public int ToBranchId { get; set; }

    public string? TransferReason { get; set; }

    public byte TransferStatus { get; set; }

    public int RequestedBy { get; set; }

    public int? ApprovedBy { get; set; }

    public DateTime RequestedDate { get; set; }

    public DateTime? ApprovedDate { get; set; }

    public string? Notes { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
