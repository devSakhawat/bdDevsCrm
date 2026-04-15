namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new group member.
/// </summary>
public record CreateGroupMemberRecord(
    int GroupId,
    int UserId,
    string? GroupOption);

/// <summary>
/// Record for updating an existing group member.
/// </summary>
public record UpdateGroupMemberRecord(
    int GroupId,
    int UserId,
    string? GroupOption);

/// <summary>
/// Record for deleting a group member.
/// </summary>
public record DeleteGroupMemberRecord(int GroupId, int UserId);
