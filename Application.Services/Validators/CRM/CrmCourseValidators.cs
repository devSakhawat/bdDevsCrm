using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmCourseRecord.
/// </summary>
public class CreateCrmCourseRecordValidator : BaseRecordValidator<CreateCrmCourseRecord>
{
    public CreateCrmCourseRecordValidator()
    {
        RuleFor(x => x.InstituteId)
            .GreaterThan(0)
            .WithMessage("InstituteId must be greater than 0");

        RuleFor(x => x.CourseTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseTitle))
            .WithMessage($"CourseTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseLevel)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseLevel))
            .WithMessage($"CourseLevel cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PartTimeWorkDetails)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PartTimeWorkDetails))
            .WithMessage($"PartTimeWorkDetails cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseBenefits))
            .WithMessage($"CourseBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LanguagesRequirement)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LanguagesRequirement))
            .WithMessage($"LanguagesRequirement cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseDuration)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseDuration))
            .WithMessage($"CourseDuration cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseCategory)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseCategory))
            .WithMessage($"CourseCategory cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AwardingBody)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AwardingBody))
            .WithMessage($"AwardingBody cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AdditionalInformationOfCourse)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInformationOfCourse))
            .WithMessage($"AdditionalInformationOfCourse cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.GeneralEligibility)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GeneralEligibility))
            .WithMessage($"GeneralEligibility cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FundsRequirementforVisa)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.FundsRequirementforVisa))
            .WithMessage($"FundsRequirementforVisa cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionalBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionalBenefits))
            .WithMessage($"InstitutionalBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.VisaRequirement)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.VisaRequirement))
            .WithMessage($"VisaRequirement cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CountryBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CountryBenefits))
            .WithMessage($"CountryBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.KeyModules)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.KeyModules))
            .WithMessage($"KeyModules cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.After2YearsPswcompletingCourse)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.After2YearsPswcompletingCourse))
            .WithMessage($"After2YearsPswcompletingCourse cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentId))
            .WithMessage($"DocumentId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCrmCourseRecord.
/// </summary>
public class UpdateCrmCourseRecordValidator : BaseRecordValidator<UpdateCrmCourseRecord>
{
    public UpdateCrmCourseRecordValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("CourseId must be greater than 0");

        RuleFor(x => x.InstituteId)
            .GreaterThan(0)
            .WithMessage("InstituteId must be greater than 0");

        RuleFor(x => x.CourseTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseTitle))
            .WithMessage($"CourseTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseLevel)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseLevel))
            .WithMessage($"CourseLevel cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PartTimeWorkDetails)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PartTimeWorkDetails))
            .WithMessage($"PartTimeWorkDetails cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseBenefits))
            .WithMessage($"CourseBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LanguagesRequirement)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LanguagesRequirement))
            .WithMessage($"LanguagesRequirement cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseDuration)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseDuration))
            .WithMessage($"CourseDuration cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CourseCategory)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseCategory))
            .WithMessage($"CourseCategory cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AwardingBody)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AwardingBody))
            .WithMessage($"AwardingBody cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AdditionalInformationOfCourse)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInformationOfCourse))
            .WithMessage($"AdditionalInformationOfCourse cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.GeneralEligibility)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GeneralEligibility))
            .WithMessage($"GeneralEligibility cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FundsRequirementforVisa)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.FundsRequirementforVisa))
            .WithMessage($"FundsRequirementforVisa cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionalBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionalBenefits))
            .WithMessage($"InstitutionalBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.VisaRequirement)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.VisaRequirement))
            .WithMessage($"VisaRequirement cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CountryBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CountryBenefits))
            .WithMessage($"CountryBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.KeyModules)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.KeyModules))
            .WithMessage($"KeyModules cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.After2YearsPswcompletingCourse)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.After2YearsPswcompletingCourse))
            .WithMessage($"After2YearsPswcompletingCourse cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentId))
            .WithMessage($"DocumentId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCrmCourseRecord.
/// </summary>
public class DeleteCrmCourseRecordValidator : BaseRecordValidator<DeleteCrmCourseRecord>
{
    public DeleteCrmCourseRecordValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("CourseId must be greater than 0");

    }
}
