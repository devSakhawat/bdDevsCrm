using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmLeadStatusRecord.</summary>
public class CreateCrmLeadStatusRecordValidator : BaseRecordValidator<CreateCrmLeadStatusRecord>
{
    public CreateCrmLeadStatusRecordValidator()
    {
        RuleFor(x => x.StatusName)
            .NotEmpty().WithMessage("StatusName is required")
            .MaximumLength(MaxNameLength).WithMessage($"StatusName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.StatusCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.StatusCode))
            .WithMessage($"StatusCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.ColorCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ColorCode))
            .WithMessage($"ColorCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmLeadStatusRecord.</summary>
public class UpdateCrmLeadStatusRecordValidator : BaseRecordValidator<UpdateCrmLeadStatusRecord>
{
    public UpdateCrmLeadStatusRecordValidator()
    {
        RuleFor(x => x.LeadStatusId)
            .GreaterThan(0).WithMessage("LeadStatusId must be greater than 0");

        RuleFor(x => x.StatusName)
            .NotEmpty().WithMessage("StatusName is required")
            .MaximumLength(MaxNameLength).WithMessage($"StatusName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.StatusCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.StatusCode))
            .WithMessage($"StatusCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.ColorCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ColorCode))
            .WithMessage($"ColorCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmLeadStatusRecord.</summary>
public class DeleteCrmLeadStatusRecordValidator : BaseRecordValidator<DeleteCrmLeadStatusRecord>
{
    public DeleteCrmLeadStatusRecordValidator()
    {
        RuleFor(x => x.LeadStatusId)
            .GreaterThan(0).WithMessage("LeadStatusId must be greater than 0");
    }
}
