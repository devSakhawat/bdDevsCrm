namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new competency.
/// </summary>
public record CreateCompetenciesRecord(
    string? CompetencyName,
    int? CompetencyType,
    int? IsDepartmentHead,
    int? IsActive);

/// <summary>
/// Record for updating an existing competency.
/// </summary>
public record UpdateCompetenciesRecord(
    int Id,
    string? CompetencyName,
    int? CompetencyType,
    int? IsDepartmentHead,
    int? IsActive);

/// <summary>
/// Record for deleting a competency.
/// </summary>
public record DeleteCompetenciesRecord(int Id);
