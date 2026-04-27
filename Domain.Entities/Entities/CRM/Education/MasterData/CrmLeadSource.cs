    using System;

    namespace Domain.Entities.Entities.CRM;

    public partial class CrmLeadSource
    {
        public int LeadSourceId { get; set; }

public string LeadSourceName { get; set; }

public int SortOrder { get; set; }

public bool IsActive { get; set; }

public DateTime CreatedDate { get; set; }

public int CreatedBy { get; set; }

public DateTime? UpdatedDate { get; set; }

public int? UpdatedBy { get; set; }
    }
