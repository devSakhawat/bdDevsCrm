using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCompanyRecord.
/// </summary>
public class CreateCompanyRecordValidator : BaseRecordValidator<CreateCompanyRecord>
{
    public CreateCompanyRecordValidator()
    {
        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("CompanyName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CompanyName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CompanyCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.CompanyCode))
            .WithMessage($"CompanyCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Address)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage($"Address cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.Phone)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage($"Phone cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.Email)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage($"Email cannot exceed {MaxEmailLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCompanyRecord.
/// </summary>
public class UpdateCompanyRecordValidator : BaseRecordValidator<UpdateCompanyRecord>
{
    public UpdateCompanyRecordValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.CompanyName)
            .NotEmpty()
            .WithMessage("CompanyName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CompanyName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CompanyCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.CompanyCode))
            .WithMessage($"CompanyCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Address)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage($"Address cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.Phone)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.Phone))
            .WithMessage($"Phone cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.Email)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.Email))
            .WithMessage($"Email cannot exceed {MaxEmailLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCompanyRecord.
/// </summary>
public class DeleteCompanyRecordValidator : BaseRecordValidator<DeleteCompanyRecord>
{
    public DeleteCompanyRecordValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

    }
}