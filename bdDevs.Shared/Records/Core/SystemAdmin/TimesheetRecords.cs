namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new timesheet entry.
/// </summary>
public record CreateTimesheetRecord(
    int? Hrrecordid,
    int? Projectid,
    int? Taskid,
    DateTime? WorkingLogDate,
    double? WorkedLogHour,
    DateTime? LogEntryDate,
    double? BillableLogHour,
    double? NoBillableLogHour,
    int? Isapprove,
    int? ApproveRhRrecordid,
    DateTime? ApproveDate,
    int? BillStatus);

/// <summary>
/// Record for updating an existing timesheet entry.
/// </summary>
public record UpdateTimesheetRecord(
    int Timesheetid,
    int? Hrrecordid,
    int? Projectid,
    int? Taskid,
    DateTime? WorkingLogDate,
    double? WorkedLogHour,
    DateTime? LogEntryDate,
    double? BillableLogHour,
    double? NoBillableLogHour,
    int? Isapprove,
    int? ApproveRhRrecordid,
    DateTime? ApproveDate,
    int? BillStatus);

/// <summary>
/// Record for deleting a timesheet entry.
/// </summary>
public record DeleteTimesheetRecord(int Timesheetid);
