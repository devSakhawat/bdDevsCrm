namespace bdDevs.Shared.Records.Core.SystemAdmin;

public record CreateTimesheetRecord(
    int? Hrrecordid,
    int? Projectid,
    int? Taskid,
    DateTime? WorkingLogDate,
    double? WorkedLogHour,
    double? BillableLogHour,
    double? NoBillableLogHour,
    int? Isapprove);

public record UpdateTimesheetRecord(
    int Timesheetid,
    int? Hrrecordid,
    int? Projectid,
    int? Taskid,
    DateTime? WorkingLogDate,
    double? WorkedLogHour,
    double? BillableLogHour,
    double? NoBillableLogHour,
    int? Isapprove);

public record DeleteTimesheetRecord(int Timesheetid);
