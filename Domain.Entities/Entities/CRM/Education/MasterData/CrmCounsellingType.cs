using System;

namespace Domain.Entities.Entities.CRM;

public partial class CrmCounsellingType
{
    public int CounsellingTypeId { get; set; }

    public string CounsellingTypeName { get; set; } = string.Empty;

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
