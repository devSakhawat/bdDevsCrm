namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCounsellingTypeDto
{
    public int CounsellingTypeId { get; init; }

    public string CounsellingTypeName { get; init; } = string.Empty;

    public bool IsActive { get; init; }

    public DateTime CreatedDate { get; init; }

    public int CreatedBy { get; init; }

    public DateTime? UpdatedDate { get; init; }

    public int? UpdatedBy { get; init; }
}

public record CrmCounsellingTypeDDLDto
{
    public int CounsellingTypeId { get; init; }

    public string CounsellingTypeName { get; init; } = string.Empty;
}
