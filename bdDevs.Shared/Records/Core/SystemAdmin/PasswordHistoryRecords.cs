namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new password history entry.
/// </summary>
public record CreatePasswordHistoryRecord(
    int? UserId,
    string? OldPassword,
    DateTime? PasswordChangeDate);

/// <summary>
/// Record for updating an existing password history entry.
/// </summary>
public record UpdatePasswordHistoryRecord(
    int HistoryId,
    int? UserId,
    string? OldPassword,
    DateTime? PasswordChangeDate);

/// <summary>
/// Record for deleting a password history entry.
/// </summary>
public record DeletePasswordHistoryRecord(int HistoryId);
