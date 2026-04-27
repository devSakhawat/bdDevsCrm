namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmStudentStatusDto
{
    public int StudentStatusId { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public string? StatusCode { get; init; }
    public string? ColorCode { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}

public record CrmStudentStatusDDL
{
    public int StudentStatusId { get; init; }
    public string StatusName { get; init; } = string.Empty;
    public string? ColorCode { get; init; }
}
