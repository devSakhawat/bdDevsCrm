using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateWfStateRecord.
/// </summary>
public class CreateWfStateRecordValidator : BaseRecordValidator<CreateWfStateRecord>
{
    public CreateWfStateRecordValidator()
    {
        RuleFor(x => x.StateName)
            .NotEmpty()
            .WithMessage("StateName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"StateName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateWfStateRecord.
/// </summary>
public class UpdateWfStateRecordValidator : BaseRecordValidator<UpdateWfStateRecord>
{
    public UpdateWfStateRecordValidator()
    {
        RuleFor(x => x.WfStateId)
            .GreaterThan(0)
            .WithMessage("WfStateId must be greater than 0");

        RuleFor(x => x.StateName)
            .NotEmpty()
            .WithMessage("StateName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"StateName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteWfStateRecord.
/// </summary>
public class DeleteWfStateRecordValidator : BaseRecordValidator<DeleteWfStateRecord>
{
    public DeleteWfStateRecordValidator()
    {
        RuleFor(x => x.WfStateId)
            .GreaterThan(0)
            .WithMessage("WfStateId must be greater than 0");

    }
}