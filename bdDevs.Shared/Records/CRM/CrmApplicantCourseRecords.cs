namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM applicant course.
/// </summary>
public record CreateCrmApplicantCourseRecord(
    int ApplicantId,
    int CountryId,
    string? CountryName,
    int InstituteId,
    string? CourseTitle,
    int IntakeMonthId,
    string? IntakeMonth,
    int IntakeYearId,
    string? IntakeYear,
    string? ApplicationFee,
    int CurrencyId,
    int PaymentMethodId,
    string? PaymentMethod,
    string? PaymentReferenceNumber,
    DateTime? PaymentDate,
    string? Remarks,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy,
    int? CourseId);

/// <summary>
/// Record for updating an existing CRM applicant course.
/// </summary>
public record UpdateCrmApplicantCourseRecord(
    int ApplicantCourseId,
    int ApplicantId,
    int CountryId,
    string? CountryName,
    int InstituteId,
    string? CourseTitle,
    int IntakeMonthId,
    string? IntakeMonth,
    int IntakeYearId,
    string? IntakeYear,
    string? ApplicationFee,
    int CurrencyId,
    int PaymentMethodId,
    string? PaymentMethod,
    string? PaymentReferenceNumber,
    DateTime? PaymentDate,
    string? Remarks,
    DateTime CreatedDate,
    int CreatedBy,
    DateTime? UpdatedDate,
    int? UpdatedBy,
    int? CourseId);

/// <summary>
/// Record for deleting a CRM applicant course.
/// </summary>
/// <param name="ApplicantCourseId">ID of the applicant course to delete.</param>
public record DeleteCrmApplicantCourseRecord(int ApplicantCourseId);
