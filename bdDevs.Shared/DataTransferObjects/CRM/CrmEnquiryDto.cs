namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmEnquiryDto
{
    public int EnquiryId { get; init; }
    public int? LeadId { get; init; }
    public int? StudentId { get; init; }
    public int? CourseId { get; init; }
    public int? InstituteId { get; init; }
    public int? CountryId { get; init; }
    public DateTime EnquiryDate { get; init; }
    public int? ExpectedIntakeMonth { get; init; }
    public int? ExpectedIntakeYear { get; init; }
    public string? Notes { get; init; }
    public bool IsActive { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
