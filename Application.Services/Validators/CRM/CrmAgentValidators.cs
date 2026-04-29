using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

public class CreateCrmAgentRecordValidator : BaseRecordValidator<CreateCrmAgentRecord>
{
    public CreateCrmAgentRecordValidator()
    {
        RuleFor(x => x.AgentName)
            .NotEmpty()
            .WithMessage("AgentName is required")
            .MaximumLength(200)
            .WithMessage("AgentName cannot exceed 200 characters");

        RuleFor(x => x.AgencyName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.AgencyName))
            .WithMessage("AgencyName cannot exceed 200 characters");

        RuleFor(x => x.PrimaryPhone)
            .NotEmpty()
            .WithMessage("PrimaryPhone is required")
            .MaximumLength(30)
            .WithMessage("PrimaryPhone cannot exceed 30 characters");

        RuleFor(x => x.PrimaryEmail)
            .MaximumLength(150)
            .When(x => !string.IsNullOrEmpty(x.PrimaryEmail))
            .WithMessage("PrimaryEmail cannot exceed 150 characters");

        RuleFor(x => x.CommissionTypeId)
            .GreaterThan(0)
            .WithMessage("CommissionTypeId must be greater than 0");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class UpdateCrmAgentRecordValidator : BaseRecordValidator<UpdateCrmAgentRecord>
{
    public UpdateCrmAgentRecordValidator()
    {
        RuleFor(x => x.AgentId)
            .GreaterThan(0)
            .WithMessage("AgentId must be greater than 0");

        RuleFor(x => x.AgentName)
            .NotEmpty()
            .WithMessage("AgentName is required")
            .MaximumLength(200)
            .WithMessage("AgentName cannot exceed 200 characters");

        RuleFor(x => x.AgencyName)
            .MaximumLength(200)
            .When(x => !string.IsNullOrEmpty(x.AgencyName))
            .WithMessage("AgencyName cannot exceed 200 characters");

        RuleFor(x => x.PrimaryPhone)
            .NotEmpty()
            .WithMessage("PrimaryPhone is required")
            .MaximumLength(30)
            .WithMessage("PrimaryPhone cannot exceed 30 characters");

        RuleFor(x => x.PrimaryEmail)
            .MaximumLength(150)
            .When(x => !string.IsNullOrEmpty(x.PrimaryEmail))
            .WithMessage("PrimaryEmail cannot exceed 150 characters");

        RuleFor(x => x.CommissionTypeId)
            .GreaterThan(0)
            .WithMessage("CommissionTypeId must be greater than 0");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class DeleteCrmAgentRecordValidator : BaseRecordValidator<DeleteCrmAgentRecord>
{
    public DeleteCrmAgentRecordValidator()
    {
        RuleFor(x => x.AgentId)
            .GreaterThan(0)
            .WithMessage("AgentId must be greater than 0");
    }
}
