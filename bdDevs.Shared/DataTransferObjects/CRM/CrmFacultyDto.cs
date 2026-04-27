namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmFacultyDto
{
    public int FacultyId { get; init; }
    public int InstituteId { get; init; }
    public string FacultyName { get; init; } = string.Empty;
    public bool IsActive { get; init; }
    public int CreatedBy { get; init; }
    public DateTime CreatedDate { get; init; }
    public int? UpdatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public string? InstituteName { get; init; }
}
