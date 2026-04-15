using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmInstituteRecord.
/// </summary>
public class CreateCrmInstituteRecordValidator : BaseRecordValidator<CreateCrmInstituteRecord>
{
    public CreateCrmInstituteRecordValidator()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.InstituteName)
            .NotEmpty()
            .WithMessage("InstituteName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"InstituteName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Campus)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Campus))
            .WithMessage($"Campus cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Website)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Website))
            .WithMessage($"Website cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FundsRequirementforVisa)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.FundsRequirementforVisa))
            .WithMessage($"FundsRequirementforVisa cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LanguagesRequirement)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LanguagesRequirement))
            .WithMessage($"LanguagesRequirement cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionalBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionalBenefits))
            .WithMessage($"InstitutionalBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PartTimeWorkDetails)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PartTimeWorkDetails))
            .WithMessage($"PartTimeWorkDetails cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ScholarshipsPolicy)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ScholarshipsPolicy))
            .WithMessage($"ScholarshipsPolicy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionStatusNotes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionStatusNotes))
            .WithMessage($"InstitutionStatusNotes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionLogo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionLogo))
            .WithMessage($"InstitutionLogo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionProspectus)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionProspectus))
            .WithMessage($"InstitutionProspectus cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstituteCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteCode))
            .WithMessage($"InstituteCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.InstituteEmail)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteEmail))
            .WithMessage($"InstituteEmail cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.InstituteAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteAddress))
            .WithMessage($"InstituteAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.InstitutePhoneNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutePhoneNo))
            .WithMessage($"InstitutePhoneNo cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.InstituteMobileNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteMobileNo))
            .WithMessage($"InstituteMobileNo cannot exceed {MaxPhoneLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCrmInstituteRecord.
/// </summary>
public class UpdateCrmInstituteRecordValidator : BaseRecordValidator<UpdateCrmInstituteRecord>
{
    public UpdateCrmInstituteRecordValidator()
    {
        RuleFor(x => x.InstituteId)
            .GreaterThan(0)
            .WithMessage("InstituteId must be greater than 0");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.InstituteName)
            .NotEmpty()
            .WithMessage("InstituteName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"InstituteName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Campus)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Campus))
            .WithMessage($"Campus cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Website)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Website))
            .WithMessage($"Website cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FundsRequirementforVisa)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.FundsRequirementforVisa))
            .WithMessage($"FundsRequirementforVisa cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LanguagesRequirement)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LanguagesRequirement))
            .WithMessage($"LanguagesRequirement cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionalBenefits)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionalBenefits))
            .WithMessage($"InstitutionalBenefits cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PartTimeWorkDetails)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PartTimeWorkDetails))
            .WithMessage($"PartTimeWorkDetails cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ScholarshipsPolicy)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ScholarshipsPolicy))
            .WithMessage($"ScholarshipsPolicy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionStatusNotes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionStatusNotes))
            .WithMessage($"InstitutionStatusNotes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionLogo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionLogo))
            .WithMessage($"InstitutionLogo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstitutionProspectus)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutionProspectus))
            .WithMessage($"InstitutionProspectus cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.InstituteCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteCode))
            .WithMessage($"InstituteCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.InstituteEmail)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteEmail))
            .WithMessage($"InstituteEmail cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.InstituteAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteAddress))
            .WithMessage($"InstituteAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.InstitutePhoneNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.InstitutePhoneNo))
            .WithMessage($"InstitutePhoneNo cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.InstituteMobileNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.InstituteMobileNo))
            .WithMessage($"InstituteMobileNo cannot exceed {MaxPhoneLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCrmInstituteRecord.
/// </summary>
public class DeleteCrmInstituteRecordValidator : BaseRecordValidator<DeleteCrmInstituteRecord>
{
    public DeleteCrmInstituteRecordValidator()
    {
        RuleFor(x => x.InstituteId)
            .GreaterThan(0)
            .WithMessage("InstituteId must be greater than 0");

    }
}
