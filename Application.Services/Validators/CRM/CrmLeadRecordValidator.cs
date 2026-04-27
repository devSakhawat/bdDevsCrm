using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmLeadRecord.</summary>
public class CreateCrmLeadRecordValidator : BaseRecordValidator<CreateCrmLeadRecord>
{
    public CreateCrmLeadRecordValidator()
    {
        RuleFor(x => x.LeadName)
            .NotEmpty().WithMessage("LeadName is required")
            .MaximumLength(MaxNameLength).WithMessage($"LeadName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmLeadRecord.</summary>
public class UpdateCrmLeadRecordValidator : BaseRecordValidator<UpdateCrmLeadRecord>
{
    public UpdateCrmLeadRecordValidator()
    {
        RuleFor(x => x.LeadId)
            .GreaterThan(0).WithMessage("LeadId must be greater than 0");

        RuleFor(x => x.LeadName)
            .NotEmpty().WithMessage("LeadName is required")
            .MaximumLength(MaxNameLength).WithMessage($"LeadName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmLeadRecord.</summary>
public class DeleteCrmLeadRecordValidator : BaseRecordValidator<DeleteCrmLeadRecord>
{
    public DeleteCrmLeadRecordValidator()
    {
        RuleFor(x => x.LeadId)
            .GreaterThan(0).WithMessage("LeadId must be greater than 0");
    }
}
