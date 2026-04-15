namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new approver order.
/// </summary>
public record CreateApproverOrderRecord(
    string? OrderTitle,
    int? ModuleId,
    int? ApproverTypeId,
    bool? IsEditable);

/// <summary>
/// Record for updating an existing approver order.
/// </summary>
public record UpdateApproverOrderRecord(
    int ApproverOrderId,
    string? OrderTitle,
    int? ModuleId,
    int? ApproverTypeId,
    bool? IsEditable);

/// <summary>
/// Record for deleting an approver order.
/// </summary>
public record DeleteApproverOrderRecord(int ApproverOrderId);
