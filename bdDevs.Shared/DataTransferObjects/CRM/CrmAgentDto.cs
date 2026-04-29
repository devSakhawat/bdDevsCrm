namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmAgentDto
{
    public int AgentId { get; init; }

    public string AgentName { get; init; } = string.Empty;

    public string? AgencyName { get; init; }

    public string PrimaryPhone { get; init; } = string.Empty;

    public string? PrimaryEmail { get; init; }

    public int CommissionTypeId { get; init; }

    public decimal? DefaultCommissionValue { get; init; }

    public int CountryId { get; init; }

    public string? CommissionTypeName { get; init; }

    public string? CountryName { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedDate { get; init; }

    public int CreatedBy { get; init; }

    public DateTime? UpdatedDate { get; init; }

    public int? UpdatedBy { get; init; }
}

public record CrmAgentDDLDto
{
    public int AgentId { get; init; }

    public string AgentName { get; init; } = string.Empty;
}
