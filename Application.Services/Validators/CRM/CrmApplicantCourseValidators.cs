using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmApplicantCourseRecord.
/// </summary>
public class CreateCrmApplicantCourseRecordValidator : BaseRecordValidator<CreateCrmApplicantCourseRecord>
{
    public CreateCrmApplicantCourseRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.CountryName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.CountryName))
            .WithMessage($"CountryName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.InstituteId)
            .GreaterThan(0)
            .WithMessage("InstituteId must be greater than 0");

        RuleFor(x => x.CourseTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseTitle))
            .WithMessage($"CourseTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IntakeMonthId)
            .GreaterThan(0)
            .WithMessage("IntakeMonthId must be greater than 0");

        RuleFor(x => x.IntakeMonth)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeMonth))
            .WithMessage($"IntakeMonth cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IntakeYearId)
            .GreaterThan(0)
            .WithMessage("IntakeYearId must be greater than 0");

        RuleFor(x => x.IntakeYear)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeYear))
            .WithMessage($"IntakeYear cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ApplicationFee)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ApplicationFee))
            .WithMessage($"ApplicationFee cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("CurrencyId must be greater than 0");

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0)
            .WithMessage("PaymentMethodId must be greater than 0");

        RuleFor(x => x.PaymentMethod)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentMethod))
            .WithMessage($"PaymentMethod cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PaymentReferenceNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentReferenceNumber))
            .WithMessage($"PaymentReferenceNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmApplicantCourseRecord.
/// </summary>
public class UpdateCrmApplicantCourseRecordValidator : BaseRecordValidator<UpdateCrmApplicantCourseRecord>
{
    public UpdateCrmApplicantCourseRecordValidator()
    {
        RuleFor(x => x.ApplicantCourseId)
            .GreaterThan(0)
            .WithMessage("ApplicantCourseId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.CountryName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.CountryName))
            .WithMessage($"CountryName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.InstituteId)
            .GreaterThan(0)
            .WithMessage("InstituteId must be greater than 0");

        RuleFor(x => x.CourseTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CourseTitle))
            .WithMessage($"CourseTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IntakeMonthId)
            .GreaterThan(0)
            .WithMessage("IntakeMonthId must be greater than 0");

        RuleFor(x => x.IntakeMonth)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeMonth))
            .WithMessage($"IntakeMonth cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IntakeYearId)
            .GreaterThan(0)
            .WithMessage("IntakeYearId must be greater than 0");

        RuleFor(x => x.IntakeYear)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeYear))
            .WithMessage($"IntakeYear cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ApplicationFee)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ApplicationFee))
            .WithMessage($"ApplicationFee cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("CurrencyId must be greater than 0");

        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0)
            .WithMessage("PaymentMethodId must be greater than 0");

        RuleFor(x => x.PaymentMethod)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentMethod))
            .WithMessage($"PaymentMethod cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PaymentReferenceNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentReferenceNumber))
            .WithMessage($"PaymentReferenceNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmApplicantCourseRecord.
/// </summary>
public class DeleteCrmApplicantCourseRecordValidator : BaseRecordValidator<DeleteCrmApplicantCourseRecord>
{
    public DeleteCrmApplicantCourseRecordValidator()
    {
        RuleFor(x => x.ApplicantCourseId)
            .GreaterThan(0)
            .WithMessage("ApplicantCourseId must be greater than 0");

    }
}
