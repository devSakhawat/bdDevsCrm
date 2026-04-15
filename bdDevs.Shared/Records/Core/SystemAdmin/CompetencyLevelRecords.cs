namespace bdDevs.Shared.Records.Core.SystemAdmin;

/// <summary>
/// Record for creating a new competency level.
/// </summary>
public record CreateCompetencyLevelRecord(
    string? LevelTitle,
    string? Remarks);

/// <summary>
/// Record for updating an existing competency level.
/// </summary>
public record UpdateCompetencyLevelRecord(
    int LevelId,
    string? LevelTitle,
    string? Remarks);

/// <summary>
/// Record for deleting a competency level.
/// </summary>
public record DeleteCompetencyLevelRecord(int LevelId);
