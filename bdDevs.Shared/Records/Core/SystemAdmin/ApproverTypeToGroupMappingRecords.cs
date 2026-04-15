namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new approver type to group mapping.
/// </summary>
public record CreateApproverTypeToGroupMappingRecord(
    int? ApproverTypeId,
    int? ModuleId,
    int? GroupId);

/// <summary>
/// Record for updating an existing approver type to group mapping.
/// </summary>
public record UpdateApproverTypeToGroupMappingRecord(
    int ApproverTypeMapId,
    int? ApproverTypeId,
    int? ModuleId,
    int? GroupId);

/// <summary>
/// Record for deleting an approver type to group mapping.
/// </summary>
public record DeleteApproverTypeToGroupMappingRecord(int ApproverTypeMapId);
