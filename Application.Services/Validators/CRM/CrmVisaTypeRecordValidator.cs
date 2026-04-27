using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmVisaTypeRecord.</summary>
public class CreateCrmVisaTypeRecordValidator : BaseRecordValidator<CreateCrmVisaTypeRecord>
{
    public CreateCrmVisaTypeRecordValidator()
    {
        RuleFor(x => x.VisaTypeName)
            .NotEmpty().WithMessage("VisaTypeName is required")
            .MaximumLength(MaxNameLength).WithMessage($"VisaTypeName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.VisaCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.VisaCode))
            .WithMessage($"VisaCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmVisaTypeRecord.</summary>
public class UpdateCrmVisaTypeRecordValidator : BaseRecordValidator<UpdateCrmVisaTypeRecord>
{
    public UpdateCrmVisaTypeRecordValidator()
    {
        RuleFor(x => x.VisaTypeId)
            .GreaterThan(0).WithMessage("VisaTypeId must be greater than 0");

        RuleFor(x => x.VisaTypeName)
            .NotEmpty().WithMessage("VisaTypeName is required")
            .MaximumLength(MaxNameLength).WithMessage($"VisaTypeName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.VisaCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.VisaCode))
            .WithMessage($"VisaCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmVisaTypeRecord.</summary>
public class DeleteCrmVisaTypeRecordValidator : BaseRecordValidator<DeleteCrmVisaTypeRecord>
{
    public DeleteCrmVisaTypeRecordValidator()
    {
        RuleFor(x => x.VisaTypeId)
            .GreaterThan(0).WithMessage("VisaTypeId must be greater than 0");
    }
}
