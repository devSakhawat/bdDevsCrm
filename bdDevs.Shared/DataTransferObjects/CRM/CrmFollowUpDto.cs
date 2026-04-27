namespace bdDevs.Shared.DataTransferObjects.CRM;

public record CrmFollowUpDto
{
    public int FollowUpId { get; init; }
    public int? LeadId { get; init; }
    public int? EnquiryId { get; init; }
    public DateTime FollowUpDate { get; init; }
    public string? FollowUpType { get; init; }
    public string? Notes { get; init; }
    public DateTime? NextFollowUpDate { get; init; }
    public bool IsCompleted { get; init; }
    public int? CounselorId { get; init; }
    public DateTime CreatedDate { get; init; }
    public int CreatedBy { get; init; }
    public DateTime? UpdatedDate { get; init; }
    public int? UpdatedBy { get; init; }
}
