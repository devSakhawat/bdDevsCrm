using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmFollowUpRecord.</summary>
public class CreateCrmFollowUpRecordValidator : BaseRecordValidator<CreateCrmFollowUpRecord>
{
    public CreateCrmFollowUpRecordValidator()
    {
        RuleFor(x => x.FollowUpDate)
            .NotEmpty().WithMessage("FollowUpDate is required");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmFollowUpRecord.</summary>
public class UpdateCrmFollowUpRecordValidator : BaseRecordValidator<UpdateCrmFollowUpRecord>
{
    public UpdateCrmFollowUpRecordValidator()
    {
        RuleFor(x => x.FollowUpId)
            .GreaterThan(0).WithMessage("FollowUpId must be greater than 0");

        RuleFor(x => x.FollowUpDate)
            .NotEmpty().WithMessage("FollowUpDate is required");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmFollowUpRecord.</summary>
public class DeleteCrmFollowUpRecordValidator : BaseRecordValidator<DeleteCrmFollowUpRecord>
{
    public DeleteCrmFollowUpRecordValidator()
    {
        RuleFor(x => x.FollowUpId)
            .GreaterThan(0).WithMessage("FollowUpId must be greater than 0");
    }
}
