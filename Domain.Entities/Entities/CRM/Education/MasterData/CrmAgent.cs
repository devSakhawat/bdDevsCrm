using System;

namespace Domain.Entities.Entities.CRM;

public partial class CrmAgent
{
    public int AgentId { get; set; }

    public string AgentName { get; set; } = string.Empty;

    public string? AgencyName { get; set; }

    public string PrimaryPhone { get; set; } = string.Empty;

    public string? PrimaryEmail { get; set; }

    public int CommissionTypeId { get; set; }

    public decimal? DefaultCommissionValue { get; set; }

    public int CountryId { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedBy { get; set; }
}
