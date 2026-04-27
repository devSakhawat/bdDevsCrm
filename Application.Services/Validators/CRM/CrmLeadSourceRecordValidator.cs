using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmLeadSourceRecord.</summary>
public class CreateCrmLeadSourceRecordValidator : BaseRecordValidator<CreateCrmLeadSourceRecord>
{
    public CreateCrmLeadSourceRecordValidator()
    {
        RuleFor(x => x.SourceName)
            .NotEmpty().WithMessage("SourceName is required")
            .MaximumLength(MaxNameLength).WithMessage($"SourceName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.SourceCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.SourceCode))
            .WithMessage($"SourceCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmLeadSourceRecord.</summary>
public class UpdateCrmLeadSourceRecordValidator : BaseRecordValidator<UpdateCrmLeadSourceRecord>
{
    public UpdateCrmLeadSourceRecordValidator()
    {
        RuleFor(x => x.LeadSourceId)
            .GreaterThan(0).WithMessage("LeadSourceId must be greater than 0");

        RuleFor(x => x.SourceName)
            .NotEmpty().WithMessage("SourceName is required")
            .MaximumLength(MaxNameLength).WithMessage($"SourceName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.SourceCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.SourceCode))
            .WithMessage($"SourceCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmLeadSourceRecord.</summary>
public class DeleteCrmLeadSourceRecordValidator : BaseRecordValidator<DeleteCrmLeadSourceRecord>
{
    public DeleteCrmLeadSourceRecordValidator()
    {
        RuleFor(x => x.LeadSourceId)
            .GreaterThan(0).WithMessage("LeadSourceId must be greater than 0");
    }
}
