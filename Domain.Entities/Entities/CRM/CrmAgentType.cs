using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmAgentType
{
    public int AgentTypeId { get; set; }

    public string AgentTypeName { get; set; } = null!;

    public string? Description { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
