using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmApplicantInfoRecord.
/// </summary>
public class CreateCrmApplicantInfoRecordValidator : BaseRecordValidator<CreateCrmApplicantInfoRecord>
{
    public CreateCrmApplicantInfoRecordValidator()
    {
        RuleFor(x => x.ApplicationId)
            .GreaterThan(0)
            .WithMessage("ApplicationId must be greater than 0");

        RuleFor(x => x.GenderId)
            .GreaterThan(0)
            .WithMessage("GenderId must be greater than 0");

        RuleFor(x => x.TitleValue)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TitleValue))
            .WithMessage($"TitleValue cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TitleText)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TitleText))
            .WithMessage($"TitleText cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FirstName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.FirstName))
            .WithMessage($"FirstName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.LastName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage($"LastName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.MaritalStatusId)
            .GreaterThan(0)
            .WithMessage("MaritalStatusId must be greater than 0");

        RuleFor(x => x.Nationality)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Nationality))
            .WithMessage($"Nationality cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PassportNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PassportNumber))
            .WithMessage($"PassportNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PhoneCountryCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneCountryCode))
            .WithMessage($"PhoneCountryCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PhoneAreaCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneAreaCode))
            .WithMessage($"PhoneAreaCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage($"PhoneNumber cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.Mobile)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.Mobile))
            .WithMessage($"Mobile cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.EmailAddress)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.EmailAddress))
            .WithMessage($"EmailAddress cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.SkypeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SkypeId))
            .WithMessage($"SkypeId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ApplicantImagePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ApplicantImagePath))
            .WithMessage($"ApplicantImagePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmApplicantInfoRecord.
/// </summary>
public class UpdateCrmApplicantInfoRecordValidator : BaseRecordValidator<UpdateCrmApplicantInfoRecord>
{
    public UpdateCrmApplicantInfoRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.ApplicationId)
            .GreaterThan(0)
            .WithMessage("ApplicationId must be greater than 0");

        RuleFor(x => x.GenderId)
            .GreaterThan(0)
            .WithMessage("GenderId must be greater than 0");

        RuleFor(x => x.TitleValue)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TitleValue))
            .WithMessage($"TitleValue cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TitleText)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TitleText))
            .WithMessage($"TitleText cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FirstName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.FirstName))
            .WithMessage($"FirstName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.LastName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.LastName))
            .WithMessage($"LastName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.MaritalStatusId)
            .GreaterThan(0)
            .WithMessage("MaritalStatusId must be greater than 0");

        RuleFor(x => x.Nationality)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Nationality))
            .WithMessage($"Nationality cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PassportNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PassportNumber))
            .WithMessage($"PassportNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PhoneCountryCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneCountryCode))
            .WithMessage($"PhoneCountryCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PhoneAreaCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneAreaCode))
            .WithMessage($"PhoneAreaCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneNumber))
            .WithMessage($"PhoneNumber cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.Mobile)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.Mobile))
            .WithMessage($"Mobile cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.EmailAddress)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.EmailAddress))
            .WithMessage($"EmailAddress cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.SkypeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SkypeId))
            .WithMessage($"SkypeId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ApplicantImagePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ApplicantImagePath))
            .WithMessage($"ApplicantImagePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmApplicantInfoRecord.
/// </summary>
public class DeleteCrmApplicantInfoRecordValidator : BaseRecordValidator<DeleteCrmApplicantInfoRecord>
{
    public DeleteCrmApplicantInfoRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

    }
}
