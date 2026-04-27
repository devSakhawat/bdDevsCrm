using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmAgentTypeRecord.</summary>
public class CreateCrmAgentTypeRecordValidator : BaseRecordValidator<CreateCrmAgentTypeRecord>
{
    public CreateCrmAgentTypeRecordValidator()
    {
        RuleFor(x => x.AgentTypeName)
            .NotEmpty().WithMessage("AgentTypeName is required")
            .MaximumLength(MaxNameLength).WithMessage($"AgentTypeName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmAgentTypeRecord.</summary>
public class UpdateCrmAgentTypeRecordValidator : BaseRecordValidator<UpdateCrmAgentTypeRecord>
{
    public UpdateCrmAgentTypeRecordValidator()
    {
        RuleFor(x => x.AgentTypeId)
            .GreaterThan(0).WithMessage("AgentTypeId must be greater than 0");

        RuleFor(x => x.AgentTypeName)
            .NotEmpty().WithMessage("AgentTypeName is required")
            .MaximumLength(MaxNameLength).WithMessage($"AgentTypeName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmAgentTypeRecord.</summary>
public class DeleteCrmAgentTypeRecordValidator : BaseRecordValidator<DeleteCrmAgentTypeRecord>
{
    public DeleteCrmAgentTypeRecordValidator()
    {
        RuleFor(x => x.AgentTypeId)
            .GreaterThan(0).WithMessage("AgentTypeId must be greater than 0");
    }
}
