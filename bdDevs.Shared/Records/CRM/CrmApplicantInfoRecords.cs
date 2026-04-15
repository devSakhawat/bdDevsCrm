namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM applicant info.
/// </summary>
public record CreateCrmApplicantInfoRecord(
    int ApplicationId,
    int GenderId,
    string? TitleValue,
    string? TitleText,
    string? FirstName,
    string? LastName,
    DateTime? DateOfBirth,
    int MaritalStatusId,
    string? Nationality,
    bool? HasValidPassport,
    string? PassportNumber,
    DateTime? PassportIssueDate,
    DateTime? PassportExpiryDate,
    string? PhoneCountryCode,
    string? PhoneAreaCode,
    string? PhoneNumber,
    string? Mobile,
    string? EmailAddress,
    string? SkypeId,
    string? ApplicantImagePath,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for updating an existing CRM applicant info.
/// </summary>
public record UpdateCrmApplicantInfoRecord(
    int ApplicantId,
    int ApplicationId,
    int GenderId,
    string? TitleValue,
    string? TitleText,
    string? FirstName,
    string? LastName,
    DateTime? DateOfBirth,
    int MaritalStatusId,
    string? Nationality,
    bool? HasValidPassport,
    string? PassportNumber,
    DateTime? PassportIssueDate,
    DateTime? PassportExpiryDate,
    string? PhoneCountryCode,
    string? PhoneAreaCode,
    string? PhoneNumber,
    string? Mobile,
    string? EmailAddress,
    string? SkypeId,
    string? ApplicantImagePath,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy);

/// <summary>
/// Record for deleting a CRM applicant info.
/// </summary>
/// <param name="ApplicantId">ID of the applicant to delete.</param>
public record DeleteCrmApplicantInfoRecord(int ApplicantId);
