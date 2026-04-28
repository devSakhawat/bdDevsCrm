using System;

namespace Domain.Entities.Entities.CRM;

public partial class CrmCommissionType
{
    public int CommissionTypeId { get; set; }

    public string CommissionTypeName { get; set; } = string.Empty;

    public string? CalculationMode { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
