using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmOfficeRecord.</summary>
public class CreateCrmOfficeRecordValidator : BaseRecordValidator<CreateCrmOfficeRecord>
{
    public CreateCrmOfficeRecordValidator()
    {
        RuleFor(x => x.OfficeName)
            .NotEmpty().WithMessage("OfficeName is required")
            .MaximumLength(MaxNameLength).WithMessage($"OfficeName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.OfficeCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.OfficeCode))
            .WithMessage($"OfficeCode cannot exceed {MaxCodeLength} characters");

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

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmOfficeRecord.</summary>
public class UpdateCrmOfficeRecordValidator : BaseRecordValidator<UpdateCrmOfficeRecord>
{
    public UpdateCrmOfficeRecordValidator()
    {
        RuleFor(x => x.OfficeId)
            .GreaterThan(0).WithMessage("OfficeId must be greater than 0");

        RuleFor(x => x.OfficeName)
            .NotEmpty().WithMessage("OfficeName is required")
            .MaximumLength(MaxNameLength).WithMessage($"OfficeName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.OfficeCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.OfficeCode))
            .WithMessage($"OfficeCode cannot exceed {MaxCodeLength} characters");

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

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmOfficeRecord.</summary>
public class DeleteCrmOfficeRecordValidator : BaseRecordValidator<DeleteCrmOfficeRecord>
{
    public DeleteCrmOfficeRecordValidator()
    {
        RuleFor(x => x.OfficeId)
            .GreaterThan(0).WithMessage("OfficeId must be greater than 0");
    }
}
