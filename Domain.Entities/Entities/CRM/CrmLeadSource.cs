using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmLeadSource
{
    public int LeadSourceId { get; set; }

    public string SourceName { get; set; } = null!;

    public string? SourceCode { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
