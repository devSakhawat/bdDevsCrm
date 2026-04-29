namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCommissionTypeDto
{
    public int CommissionTypeId { get; init; }

    public string CommissionTypeName { get; init; } = string.Empty;

    public string? CalculationMode { get; init; }

    public bool IsActive { get; init; }

    public DateTime CreatedDate { get; init; }

    public int CreatedBy { get; init; }

    public DateTime? UpdatedDate { get; init; }

    public int? UpdatedBy { get; init; }
}

public record CrmCommissionTypeDDLDto
{
    public int CommissionTypeId { get; init; }

    public string CommissionTypeName { get; init; } = string.Empty;
}
