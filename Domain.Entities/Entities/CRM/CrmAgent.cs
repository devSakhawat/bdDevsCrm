using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmAgent
{
    public int AgentId { get; set; }

    public string AgentName { get; set; } = null!;

    public string? AgentCode { get; set; }

    public int AgentTypeId { get; set; }

    public string? ContactPerson { get; set; }

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? Country { get; set; }

    public decimal? CommissionRate { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual CrmAgentType? AgentType { get; set; }
}
