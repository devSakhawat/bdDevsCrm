namespace bdDevs.Shared.Records.DMS;

/// <summary>
/// Record for creating a new DMS document access log entry.
/// </summary>
/// <param name="DocumentId">ID of the accessed document.</param>
/// <param name="AccessedByUserId">ID of the user who accessed the document.</param>
/// <param name="AccessDateTime">Date and time of access.</param>
/// <param name="Action">Action performed on the document.</param>
/// <param name="IpAddress">IP address of the accessing device.</param>
/// <param name="DeviceInfo">Information about the accessing device.</param>
/// <param name="MacAddress">MAC address of the accessing device.</param>
/// <param name="Notes">Additional notes about the access.</param>
public record CreateDmsDocumentAccessLogRecord(
    int DocumentId,
    string? AccessedByUserId,
    DateTime? AccessDateTime,
    string Action,
    string? IpAddress,
    string? DeviceInfo,
    string? MacAddress,
    string? Notes);

/// <summary>
/// Record for updating an existing DMS document access log entry.
/// </summary>
/// <param name="LogId">ID of the log entry to update.</param>
/// <param name="DocumentId">Updated document ID.</param>
/// <param name="AccessedByUserId">Updated user ID who accessed the document.</param>
/// <param name="AccessDateTime">Updated access date and time.</param>
/// <param name="Action">Updated action performed.</param>
/// <param name="IpAddress">Updated IP address.</param>
/// <param name="DeviceInfo">Updated device information.</param>
/// <param name="MacAddress">Updated MAC address.</param>
/// <param name="Notes">Updated notes.</param>
public record UpdateDmsDocumentAccessLogRecord(
    long LogId,
    int DocumentId,
    string? AccessedByUserId,
    DateTime? AccessDateTime,
    string Action,
    string? IpAddress,
    string? DeviceInfo,
    string? MacAddress,
    string? Notes);

/// <summary>
/// Record for deleting a DMS document access log entry.
/// </summary>
/// <param name="LogId">ID of the log entry to delete.</param>
public record DeleteDmsDocumentAccessLogRecord(long LogId);
