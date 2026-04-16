namespace bdDevs.Shared.DataTransferObjects.Core.SystemAdmin;

public class TimesheetDto
{
    public int Timesheetid { get; set; }
    public int? Hrrecordid { get; set; }
    public int? Projectid { get; set; }
    public int? Taskid { get; set; }
    public DateTime? WorkingLogDate { get; set; }
    public double? WorkedLogHour { get; set; }
    public DateTime? LogEntryDate { get; set; }
    public double? BillableLogHour { get; set; }
    public double? NoBillableLogHour { get; set; }
    public int? Isapprove { get; set; }
    public int? ApproveRhRrecordid { get; set; }
    public DateTime? ApproveDate { get; set; }
    public int? BillStatus { get; set; }
}

public class TimesheetDDLDto
{
    public int Timesheetid { get; set; }
    public DateTime? WorkingLogDate { get; set; }
    public double? WorkedLogHour { get; set; }
}
