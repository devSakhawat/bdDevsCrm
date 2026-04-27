namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmStudentDto
{
    public int StudentId { get; init; }
    public string StudentName { get; init; } = string.Empty;
    public string? StudentCode { get; init; }
    public string? Email { get; init; }
    public string? Phone { get; init; }
    public int? LeadId { get; init; }
    public int? StudentStatusId { get; init; }
    public int? AgentId { get; init; }
    public int? CounselorId { get; init; }
    public DateTime? DateOfBirth { get; init; }
    public string? PassportNumber { get; init; }
    public int? VisaTypeId { get; init; }
    public string? Nationality { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
