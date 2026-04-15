namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new holiday.
/// </summary>
public record CreateHolidayRecord(
    int? HolidayType,
    DateOnly? HolidayDate,
    int? Shiftid,
    int? Month,
    string? MonthName,
    int? YearName,
    string? DayName,
    string? Description,
    int? RosterReschedule);

/// <summary>
/// Record for updating an existing holiday.
/// </summary>
public record UpdateHolidayRecord(
    int HolidayId,
    int? HolidayType,
    DateOnly? HolidayDate,
    int? Shiftid,
    int? Month,
    string? MonthName,
    int? YearName,
    string? DayName,
    string? Description,
    int? RosterReschedule);

/// <summary>
/// Record for deleting a holiday.
/// </summary>
public record DeleteHolidayRecord(int HolidayId);
