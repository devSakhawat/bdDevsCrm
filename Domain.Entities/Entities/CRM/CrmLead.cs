using System;
using System.Collections.Generic;

namespace Domain.Entities.Entities.CRM;

public partial class CrmLead
{
    public int LeadId { get; set; }

    public string LeadName { get; set; } = null!;

    public string? Email { get; set; }

    public string? Phone { get; set; }

    public int? LeadSourceId { get; set; }

    public int? LeadStatusId { get; set; }

    public int? AssignedCounselorId { get; set; }

    public int? AgentId { get; set; }

    public string? CountryOfInterest { get; set; }

    public string? CourseOfInterest { get; set; }

    public decimal? Budget { get; set; }

    public string? Notes { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }

    public virtual CrmLeadSource? LeadSource { get; set; }

    public virtual CrmLeadStatus? LeadStatus { get; set; }

    public virtual CrmCounselor? AssignedCounselor { get; set; }

    public virtual CrmAgent? Agent { get; set; }
}
