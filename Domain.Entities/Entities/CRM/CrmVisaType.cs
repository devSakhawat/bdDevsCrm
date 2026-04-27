using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmVisaType
{
    public int VisaTypeId { get; set; }

    public string VisaTypeName { get; set; } = null!;

    public string? VisaCode { get; set; }

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
