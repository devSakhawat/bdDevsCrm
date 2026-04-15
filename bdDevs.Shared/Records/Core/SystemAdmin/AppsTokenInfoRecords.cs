namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new apps token info.
/// </summary>
public record CreateAppsTokenInfoRecord(
    string? AppsUserId,
    string? EmployeeId,
    string? TokenNumber,
    DateTime? IssueDate,
    DateTime? ExpiredDate);

/// <summary>
/// Record for updating an existing apps token info.
/// </summary>
public record UpdateAppsTokenInfoRecord(
    int AppsTokenInfoId,
    string? AppsUserId,
    string? EmployeeId,
    string? TokenNumber,
    DateTime? IssueDate,
    DateTime? ExpiredDate);

/// <summary>
/// Record for deleting an apps token info.
/// </summary>
public record DeleteAppsTokenInfoRecord(int AppsTokenInfoId);
