namespace bdDevs.Shared.Records.CRM;

/// <summary>
/// Record for creating a new CRM course.
/// </summary>
public record CreateCrmCourseRecord(
    int InstituteId,
    string? CourseTitle,
    string? CourseLevel,
    decimal? CourseFee,
    decimal? ApplicationFee,
    int? CurrencyId,
    decimal? MonthlyLivingCost,
    string? PartTimeWorkDetails,
    DateTime? StartDate,
    DateTime? EndDate,
    string? CourseBenefits,
    string? LanguagesRequirement,
    string? CourseDuration,
    string? CourseCategory,
    string? AwardingBody,
    string? AdditionalInformationOfCourse,
    string? GeneralEligibility,
    string? FundsRequirementforVisa,
    string? InstitutionalBenefits,
    string? VisaRequirement,
    string? CountryBenefits,
    string? KeyModules,
    bool? Status,
    string? After2YearsPswcompletingCourse,
    string? DocumentId);

/// <summary>
/// Record for updating an existing CRM course.
/// </summary>
public record UpdateCrmCourseRecord(
    int CourseId,
    int InstituteId,
    string? CourseTitle,
    string? CourseLevel,
    decimal? CourseFee,
    decimal? ApplicationFee,
    int? CurrencyId,
    decimal? MonthlyLivingCost,
    string? PartTimeWorkDetails,
    DateTime? StartDate,
    DateTime? EndDate,
    string? CourseBenefits,
    string? LanguagesRequirement,
    string? CourseDuration,
    string? CourseCategory,
    string? AwardingBody,
    string? AdditionalInformationOfCourse,
    string? GeneralEligibility,
    string? FundsRequirementforVisa,
    string? InstitutionalBenefits,
    string? VisaRequirement,
    string? CountryBenefits,
    string? KeyModules,
    bool? Status,
    string? After2YearsPswcompletingCourse,
    string? DocumentId);

/// <summary>
/// Record for deleting a CRM course.
/// </summary>
/// <param name="CourseId">ID of the course to delete.</param>
public record DeleteCrmCourseRecord(int CourseId);
