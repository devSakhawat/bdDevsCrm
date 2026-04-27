namespace bdDevs.Shared.Records.CRM;

public record CreateCrmFollowUpRecord(
    int? LeadId,
    int? EnquiryId,
    DateTime FollowUpDate,
    string? ScheduledTime,
    string? FollowUpType,
    byte ContactMethod,
    string? Notes,
    DateTime? NextFollowUpDate,
    byte Status,
    string? MissedReason,
    int? OverriddenById,
    int? CancelledById,
    DateTime? CancelledDate,
    int? CounselorId,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record UpdateCrmFollowUpRecord(
    int FollowUpId,
    int? LeadId,
    int? EnquiryId,
    DateTime FollowUpDate,
    string? ScheduledTime,
    string? FollowUpType,
    byte ContactMethod,
    string? Notes,
    DateTime? NextFollowUpDate,
    byte Status,
    string? MissedReason,
    int? OverriddenById,
    int? CancelledById,
    DateTime? CancelledDate,
    int? CounselorId,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

public record DeleteCrmFollowUpRecord(int FollowUpId);
public record ChangeCrmFollowUpStatusRecord(int FollowUpId, byte NewStatus, int ChangedBy, string? Remarks, string? MissedReason, int? OverriddenById, int? CancelledById);
