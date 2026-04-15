using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateMenuRecord.
/// </summary>
public class CreateMenuRecordValidator : BaseRecordValidator<CreateMenuRecord>
{
    public CreateMenuRecordValidator()
    {
        RuleFor(x => x.ModuleId)
            .GreaterThan(0)
            .WithMessage("ModuleId must be greater than 0");

        RuleFor(x => x.MenuName)
            .NotEmpty()
            .WithMessage("MenuName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"MenuName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MenuPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.MenuPath))
            .WithMessage($"MenuPath cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateMenuRecord.
/// </summary>
public class UpdateMenuRecordValidator : BaseRecordValidator<UpdateMenuRecord>
{
    public UpdateMenuRecordValidator()
    {
        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId must be greater than 0");

        RuleFor(x => x.ModuleId)
            .GreaterThan(0)
            .WithMessage("ModuleId must be greater than 0");

        RuleFor(x => x.MenuName)
            .NotEmpty()
            .WithMessage("MenuName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"MenuName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MenuPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.MenuPath))
            .WithMessage($"MenuPath cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteMenuRecord.
/// </summary>
public class DeleteMenuRecordValidator : BaseRecordValidator<DeleteMenuRecord>
{
    public DeleteMenuRecordValidator()
    {
        RuleFor(x => x.MenuId)
            .GreaterThan(0)
            .WithMessage("MenuId must be greater than 0");

    }
}