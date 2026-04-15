namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM applicant reference.
/// </summary>
public record CreateCrmApplicantReferenceRecord(
    int ApplicantId,
    string Name,
    string? Designation,
    string? Institution,
    string? EmailId,
    string? PhoneNo,
    string? FaxNo,
    string? Address,
    string? City,
    string? State,
    int? CountryId,
    string? PostOrZipCode,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM applicant reference.
/// </summary>
public record UpdateCrmApplicantReferenceRecord(
    int ApplicantReferenceId,
    int ApplicantId,
    string Name,
    string? Designation,
    string? Institution,
    string? EmailId,
    string? PhoneNo,
    string? FaxNo,
    string? Address,
    string? City,
    string? State,
    int? CountryId,
    string? PostOrZipCode,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM applicant reference.
/// </summary>
/// <param name="ApplicantReferenceId">ID of the applicant reference to delete.</param>
public record DeleteCrmApplicantReferenceRecord(int ApplicantReferenceId);
