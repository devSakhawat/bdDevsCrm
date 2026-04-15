namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM intake year.
/// </summary>
public record CreateCrmIntakeYearRecord(
    string YearName,
    string? YearCode,
    int YearValue,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM intake year.
/// </summary>
public record UpdateCrmIntakeYearRecord(
    int IntakeYearId,
    string YearName,
    string? YearCode,
    int YearValue,
    string? Description,
    bool IsActive,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM intake year.
/// </summary>
/// <param name="IntakeYearId">ID of the intake year to delete.</param>
public record DeleteCrmIntakeYearRecord(int IntakeYearId);
