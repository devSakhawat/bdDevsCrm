namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new group.
/// </summary>
/// <param name="GroupName">Name of the group.</param>
/// <param name="IsDefault">Whether this is the default group (1 = default, 0 = not).</param>
public record CreateGroupRecord(
    string? GroupName,
    int? IsDefault);

/// <summary>
/// Record for updating an existing group.
/// </summary>
/// <param name="GroupId">ID of the group to update.</param>
/// <param name="GroupName">Updated group name.</param>
/// <param name="IsDefault">Updated default flag.</param>
public record UpdateGroupRecord(
    int GroupId,
    string? GroupName,
    int? IsDefault);

/// <summary>
/// Record for deleting a group.
/// </summary>
/// <param name="GroupId">ID of the group to delete.</param>
public record DeleteGroupRecord(int GroupId);
