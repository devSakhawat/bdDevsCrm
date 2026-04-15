namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating new approver history.
/// </summary>
public record CreateApproverHistoryRecord(
    int ApproverId,
    int HrRecordId,
    int ModuleId,
    int Type,
    int IsNew,
    int? SortOrder,
    bool? IsActive,
    int? IsParallel,
    int? DeleteBy,
    DateTime? DeleteDate,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate);

/// <summary>
/// Record for updating existing approver history.
/// </summary>
public record UpdateApproverHistoryRecord(
    int AssignApproverId,
    int ApproverId,
    int HrRecordId,
    int ModuleId,
    int Type,
    int IsNew,
    int? SortOrder,
    bool? IsActive,
    int? IsParallel,
    int? DeleteBy,
    DateTime? DeleteDate,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate);

/// <summary>
/// Record for deleting approver history.
/// </summary>
public record DeleteApproverHistoryRecord(int AssignApproverId);
