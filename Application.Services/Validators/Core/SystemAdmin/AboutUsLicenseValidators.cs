using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAboutUsLicenseRecord.
/// </summary>
public class CreateAboutUsLicenseRecordValidator : BaseRecordValidator<CreateAboutUsLicenseRecord>
{
    public CreateAboutUsLicenseRecordValidator()
    {
        RuleFor(x => x.LicenseFor)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LicenseFor))
            .WithMessage($"LicenseFor cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProductCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ProductCode))
            .WithMessage($"ProductCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CodeBaseVersion)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.CodeBaseVersion))
            .WithMessage($"CodeBaseVersion cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.LicenseNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LicenseNumber))
            .WithMessage($"LicenseNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LicenseType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LicenseType))
            .WithMessage($"LicenseType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Sbulicense)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Sbulicense))
            .WithMessage($"Sbulicense cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LocationLicense)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LocationLicense))
            .WithMessage($"LocationLicense cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UserLicense)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UserLicense))
            .WithMessage($"UserLicense cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ServerId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ServerId))
            .WithMessage($"ServerId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAboutUsLicenseRecord.
/// </summary>
public class UpdateAboutUsLicenseRecordValidator : BaseRecordValidator<UpdateAboutUsLicenseRecord>
{
    public UpdateAboutUsLicenseRecordValidator()
    {
        RuleFor(x => x.AboutUsLicenseId)
            .GreaterThan(0)
            .WithMessage("AboutUsLicenseId must be greater than 0");

        RuleFor(x => x.LicenseFor)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LicenseFor))
            .WithMessage($"LicenseFor cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProductCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ProductCode))
            .WithMessage($"ProductCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CodeBaseVersion)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.CodeBaseVersion))
            .WithMessage($"CodeBaseVersion cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.LicenseNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LicenseNumber))
            .WithMessage($"LicenseNumber cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LicenseType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LicenseType))
            .WithMessage($"LicenseType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Sbulicense)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Sbulicense))
            .WithMessage($"Sbulicense cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.LocationLicense)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LocationLicense))
            .WithMessage($"LocationLicense cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UserLicense)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UserLicense))
            .WithMessage($"UserLicense cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ServerId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ServerId))
            .WithMessage($"ServerId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAboutUsLicenseRecord.
/// </summary>
public class DeleteAboutUsLicenseRecordValidator : BaseRecordValidator<DeleteAboutUsLicenseRecord>
{
    public DeleteAboutUsLicenseRecordValidator()
    {
        RuleFor(x => x.AboutUsLicenseId)
            .GreaterThan(0)
            .WithMessage("AboutUsLicenseId must be greater than 0");

    }
}