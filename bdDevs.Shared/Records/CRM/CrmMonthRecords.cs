namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM month.
/// </summary>
public record CreateCrmMonthRecord(
    string MonthName,
    string? MonthCode,
    bool? Status);

/// <summary>
/// Record for updating an existing CRM month.
/// </summary>
public record UpdateCrmMonthRecord(
    int MonthId,
    string MonthName,
    string? MonthCode,
    bool? Status);

/// <summary>
/// Record for deleting a CRM month.
/// </summary>
/// <param name="MonthId">ID of the month to delete.</param>
public record DeleteCrmMonthRecord(int MonthId);
