namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new apps transaction log.
/// </summary>
public record CreateAppsTransactionLogRecord(
    DateTime TransactionDate,
    string TransactionType,
    int? ResponseCode,
    string? Request,
    string? Response,
    string? Remarks,
    string? AppsUserId,
    string? EmployeeId);

/// <summary>
/// Record for updating an existing apps transaction log.
/// </summary>
public record UpdateAppsTransactionLogRecord(
    int TransactionLogId,
    DateTime TransactionDate,
    string TransactionType,
    int? ResponseCode,
    string? Request,
    string? Response,
    string? Remarks,
    string? AppsUserId,
    string? EmployeeId);

/// <summary>
/// Record for deleting an apps transaction log.
/// </summary>
public record DeleteAppsTransactionLogRecord(int TransactionLogId);
