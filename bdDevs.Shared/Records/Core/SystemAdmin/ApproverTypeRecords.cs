namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new approver type.
/// </summary>
public record CreateApproverTypeRecord(
    string? ApproverTypeName);

/// <summary>
/// Record for updating an existing approver type.
/// </summary>
public record UpdateApproverTypeRecord(
    int ApproverTypeId,
    string? ApproverTypeName);

/// <summary>
/// Record for deleting an approver type.
/// </summary>
public record DeleteApproverTypeRecord(int ApproverTypeId);
