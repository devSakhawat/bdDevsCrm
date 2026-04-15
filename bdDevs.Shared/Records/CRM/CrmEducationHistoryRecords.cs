namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM education history.
/// </summary>
public record CreateCrmEducationHistoryRecord(
    int ApplicantId,
    string? Institution,
    string? Qualification,
    int? PassingYear,
    string? Grade,
    string? DocumentPath,
    string? DocumentName,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM education history.
/// </summary>
public record UpdateCrmEducationHistoryRecord(
    int EducationHistoryId,
    int ApplicantId,
    string? Institution,
    string? Qualification,
    int? PassingYear,
    string? Grade,
    string? DocumentPath,
    string? DocumentName,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM education history.
/// </summary>
/// <param name="EducationHistoryId">ID of the education history to delete.</param>
public record DeleteCrmEducationHistoryRecord(int EducationHistoryId);
