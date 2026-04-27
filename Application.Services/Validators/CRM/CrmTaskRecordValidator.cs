using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmTaskRecord.</summary>
public class CreateCrmTaskRecordValidator : BaseRecordValidator<CreateCrmTaskRecord>
{
    public CreateCrmTaskRecordValidator()
    {
        RuleFor(x => x.TaskTitle)
            .NotEmpty().WithMessage("TaskTitle is required")
            .MaximumLength(MaxNameLength).WithMessage($"TaskTitle cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.TaskDescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TaskDescription))
            .WithMessage($"TaskDescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmTaskRecord.</summary>
public class UpdateCrmTaskRecordValidator : BaseRecordValidator<UpdateCrmTaskRecord>
{
    public UpdateCrmTaskRecordValidator()
    {
        RuleFor(x => x.TaskId)
            .GreaterThan(0).WithMessage("TaskId must be greater than 0");

        RuleFor(x => x.TaskTitle)
            .NotEmpty().WithMessage("TaskTitle is required")
            .MaximumLength(MaxNameLength).WithMessage($"TaskTitle cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.TaskDescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TaskDescription))
            .WithMessage($"TaskDescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmTaskRecord.</summary>
public class DeleteCrmTaskRecordValidator : BaseRecordValidator<DeleteCrmTaskRecord>
{
    public DeleteCrmTaskRecordValidator()
    {
        RuleFor(x => x.TaskId)
            .GreaterThan(0).WithMessage("TaskId must be greater than 0");
    }
}
