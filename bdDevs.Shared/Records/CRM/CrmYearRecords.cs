namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM year.
/// </summary>
public record CreateCrmYearRecord(
    string YearName,
    string? YearCode,
    bool? Status);

/// <summary>
/// Record for updating an existing CRM year.
/// </summary>
public record UpdateCrmYearRecord(
    int YearId,
    string YearName,
    string? YearCode,
    bool? Status);

/// <summary>
/// Record for deleting a CRM year.
/// </summary>
/// <param name="YearId">ID of the year to delete.</param>
public record DeleteCrmYearRecord(int YearId);
