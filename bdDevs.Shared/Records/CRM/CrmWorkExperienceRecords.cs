namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM work experience.
/// </summary>
public record CreateCrmWorkExperienceRecord(
    int ApplicantId,
    string? NameOfEmployer,
    string? Position,
    DateTime? StartDate,
    DateTime? EndDate,
    decimal? Period,
    string? MainResponsibility,
    string? ScannedCopyPath,
    string? DocumentName,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM work experience.
/// </summary>
public record UpdateCrmWorkExperienceRecord(
    int WorkExperienceId,
    int ApplicantId,
    string? NameOfEmployer,
    string? Position,
    DateTime? StartDate,
    DateTime? EndDate,
    decimal? Period,
    string? MainResponsibility,
    string? ScannedCopyPath,
    string? DocumentName,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM work experience.
/// </summary>
/// <param name="WorkExperienceId">ID of the work experience to delete.</param>
public record DeleteCrmWorkExperienceRecord(int WorkExperienceId);
