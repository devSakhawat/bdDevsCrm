using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateApproverDetailsRecord.
/// </summary>
public class CreateApproverDetailsRecordValidator : BaseRecordValidator<CreateApproverDetailsRecord>
{
    public CreateApproverDetailsRecordValidator()
    {
        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId must be greater than 0");

        RuleFor(x => x.ApplicationId)
            .GreaterThan(0)
            .WithMessage("ApplicationId must be greater than 0");

        RuleFor(x => x.Comments)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Comments))
            .WithMessage($"Comments cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateApproverDetailsRecord.
/// </summary>
public class UpdateApproverDetailsRecordValidator : BaseRecordValidator<UpdateApproverDetailsRecord>
{
    public UpdateApproverDetailsRecordValidator()
    {
        RuleFor(x => x.RemarksId)
            .GreaterThan(0)
            .WithMessage("RemarksId must be greater than 0");

        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId must be greater than 0");

        RuleFor(x => x.ApplicationId)
            .GreaterThan(0)
            .WithMessage("ApplicationId must be greater than 0");

        RuleFor(x => x.Comments)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Comments))
            .WithMessage($"Comments cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteApproverDetailsRecord.
/// </summary>
public class DeleteApproverDetailsRecordValidator : BaseRecordValidator<DeleteApproverDetailsRecord>
{
    public DeleteApproverDetailsRecordValidator()
    {
        RuleFor(x => x.RemarksId)
            .GreaterThan(0)
            .WithMessage("RemarksId must be greater than 0");

    }
}