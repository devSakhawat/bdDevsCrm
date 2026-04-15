using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAccesscontrolRecord.
/// </summary>
public class CreateAccesscontrolRecordValidator : BaseRecordValidator<CreateAccesscontrolRecord>
{
    public CreateAccesscontrolRecordValidator()
    {
        RuleFor(x => x.AccessName)
            .NotEmpty()
            .WithMessage("AccessName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AccessName cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAccesscontrolRecord.
/// </summary>
public class UpdateAccesscontrolRecordValidator : BaseRecordValidator<UpdateAccesscontrolRecord>
{
    public UpdateAccesscontrolRecordValidator()
    {
        RuleFor(x => x.AccessId)
            .GreaterThan(0)
            .WithMessage("AccessId must be greater than 0");

        RuleFor(x => x.AccessName)
            .NotEmpty()
            .WithMessage("AccessName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"AccessName cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAccesscontrolRecord.
/// </summary>
public class DeleteAccesscontrolRecordValidator : BaseRecordValidator<DeleteAccesscontrolRecord>
{
    public DeleteAccesscontrolRecordValidator()
    {
        RuleFor(x => x.AccessId)
            .GreaterThan(0)
            .WithMessage("AccessId must be greater than 0");

    }
}