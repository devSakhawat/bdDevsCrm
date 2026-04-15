namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new assigned approver.
/// </summary>
public record CreateAssignApproverRecord(
    int ApproverId,
    int HrRecordId,
    int ModuleId,
    int Type,
    int IsNew,
    int? SortOrder,
    bool? IsActive,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate);

/// <summary>
/// Record for updating an existing assigned approver.
/// </summary>
public record UpdateAssignApproverRecord(
    int AssignApproverId,
    int ApproverId,
    int HrRecordId,
    int ModuleId,
    int Type,
    int IsNew,
    int? SortOrder,
    bool? IsActive,
    int? CreatedBy,
    DateTime? CreatedDate,
    int? UpdatedBy,
    DateTime? UpdatedDate);

/// <summary>
/// Record for deleting an assigned approver.
/// </summary>
public record DeleteAssignApproverRecord(int AssignApproverId);
