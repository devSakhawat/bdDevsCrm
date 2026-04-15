namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new group permission.
/// </summary>
public record CreateGroupPermissionRecord(
    string? Permissiontablename,
    int Groupid,
    int? Referenceid,
    int? Parentpermission);

/// <summary>
/// Record for updating an existing group permission.
/// </summary>
public record UpdateGroupPermissionRecord(
    int PermissionId,
    string? Permissiontablename,
    int Groupid,
    int? Referenceid,
    int? Parentpermission);

/// <summary>
/// Record for deleting a group permission.
/// </summary>
public record DeleteGroupPermissionRecord(int PermissionId);
