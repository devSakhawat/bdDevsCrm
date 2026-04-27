namespace bdDevs.Shared.Records.CRM;

/// <summary>Record for creating a new CRM follow-up.</summary>
public record CreateCrmFollowUpRecord(
    int? LeadId,
    int? EnquiryId,
    DateTime FollowUpDate,
    string? FollowUpType,
    string? Notes,
    DateTime? NextFollowUpDate,
    bool IsCompleted,
    int? CounselorId,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for updating an existing CRM follow-up.</summary>
public record UpdateCrmFollowUpRecord(
    int FollowUpId,
    int? LeadId,
    int? EnquiryId,
    DateTime FollowUpDate,
    string? FollowUpType,
    string? Notes,
    DateTime? NextFollowUpDate,
    bool IsCompleted,
    int? CounselorId,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>Record for deleting a CRM follow-up.</summary>
/// <param name="FollowUpId">ID of the follow-up to delete.</param>
public record DeleteCrmFollowUpRecord(int FollowUpId);
