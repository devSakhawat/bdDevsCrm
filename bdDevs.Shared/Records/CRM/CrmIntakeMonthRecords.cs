namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM intake month.
/// </summary>
public record CreateCrmIntakeMonthRecord(
    string MonthName,
    string? MonthCode,
    int MonthNumber,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM intake month.
/// </summary>
public record UpdateCrmIntakeMonthRecord(
    int IntakeMonthId,
    string MonthName,
    string? MonthCode,
    int MonthNumber,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM intake month.
/// </summary>
/// <param name="IntakeMonthId">ID of the intake month to delete.</param>
public record DeleteCrmIntakeMonthRecord(int IntakeMonthId);
