    using System;

    namespace Domain.Entities.Entities.CRM;

    public partial class CrmLeadStage
    {
        public int LeadStageId { get; set; }

public string LeadStageName { get; set; }

public string? StageType { get; set; }

public bool IsClosedStage { get; set; }

public DateTime CreatedDate { get; set; }

public int CreatedBy { get; set; }

public DateTime? UpdatedDate { get; set; }

public int? UpdatedBy { get; set; }
    }
