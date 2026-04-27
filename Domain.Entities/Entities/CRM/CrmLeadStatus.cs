using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmLeadStatus
{
    public int LeadStatusId { get; set; }

    public string StatusName { get; set; } = null!;

    public string? StatusCode { get; set; }

    public string? ColorCode { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
