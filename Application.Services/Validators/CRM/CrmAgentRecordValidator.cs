using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmAgentRecord.</summary>
public class CreateCrmAgentRecordValidator : BaseRecordValidator<CreateCrmAgentRecord>
{
    public CreateCrmAgentRecordValidator()
    {
        RuleFor(x => x.AgentName)
            .NotEmpty().WithMessage("AgentName is required")
            .MaximumLength(MaxNameLength).WithMessage($"AgentName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.AgentCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.AgentCode))
            .WithMessage("AgentCode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmAgentRecord.</summary>
public class UpdateCrmAgentRecordValidator : BaseRecordValidator<UpdateCrmAgentRecord>
{
    public UpdateCrmAgentRecordValidator()
    {
        RuleFor(x => x.AgentId)
            .GreaterThan(0).WithMessage("AgentId must be greater than 0");

        RuleFor(x => x.AgentName)
            .NotEmpty().WithMessage("AgentName is required")
            .MaximumLength(MaxNameLength).WithMessage($"AgentName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.AgentCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.AgentCode))
            .WithMessage("AgentCode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmAgentRecord.</summary>
public class DeleteCrmAgentRecordValidator : BaseRecordValidator<DeleteCrmAgentRecord>
{
    public DeleteCrmAgentRecordValidator()
    {
        RuleFor(x => x.AgentId)
            .GreaterThan(0).WithMessage("AgentId must be greater than 0");
    }
}
