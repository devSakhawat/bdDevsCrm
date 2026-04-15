using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateWfActionRecord.
/// </summary>
public class CreateWfActionRecordValidator : BaseRecordValidator<CreateWfActionRecord>
{
    public CreateWfActionRecordValidator()
    {
        RuleFor(x => x.WfStateId)
            .GreaterThan(0)
            .WithMessage("WfStateId must be greater than 0");

        RuleFor(x => x.ActionName)
            .NotEmpty()
            .WithMessage("ActionName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ActionName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.NextStateId)
            .GreaterThan(0)
            .WithMessage("NextStateId must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateWfActionRecord.
/// </summary>
public class UpdateWfActionRecordValidator : BaseRecordValidator<UpdateWfActionRecord>
{
    public UpdateWfActionRecordValidator()
    {
        RuleFor(x => x.WfActionId)
            .GreaterThan(0)
            .WithMessage("WfActionId must be greater than 0");

        RuleFor(x => x.WfStateId)
            .GreaterThan(0)
            .WithMessage("WfStateId must be greater than 0");

        RuleFor(x => x.ActionName)
            .NotEmpty()
            .WithMessage("ActionName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ActionName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.NextStateId)
            .GreaterThan(0)
            .WithMessage("NextStateId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteWfActionRecord.
/// </summary>
public class DeleteWfActionRecordValidator : BaseRecordValidator<DeleteWfActionRecord>
{
    public DeleteWfActionRecordValidator()
    {
        RuleFor(x => x.WfActionId)
            .GreaterThan(0)
            .WithMessage("WfActionId must be greater than 0");

    }
}