namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmCounselorDto
{
    public int CounselorId { get; init; }
    public string CounselorName { get; init; } = string.Empty;
    public string? CounselorCode { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public int? OfficeId { get; init; }
    public int? UserId { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
