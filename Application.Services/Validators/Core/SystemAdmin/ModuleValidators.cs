using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateModuleRecord.
/// </summary>
public class CreateModuleRecordValidator : BaseRecordValidator<CreateModuleRecord>
{
    public CreateModuleRecordValidator()
    {
        RuleFor(x => x.ModuleName)
            .NotEmpty()
            .WithMessage("ModuleName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ModuleName cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateModuleRecord.
/// </summary>
public class UpdateModuleRecordValidator : BaseRecordValidator<UpdateModuleRecord>
{
    public UpdateModuleRecordValidator()
    {
        RuleFor(x => x.ModuleId)
            .GreaterThan(0)
            .WithMessage("ModuleId must be greater than 0");

        RuleFor(x => x.ModuleName)
            .NotEmpty()
            .WithMessage("ModuleName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ModuleName cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteModuleRecord.
/// </summary>
public class DeleteModuleRecordValidator : BaseRecordValidator<DeleteModuleRecord>
{
    public DeleteModuleRecordValidator()
    {
        RuleFor(x => x.ModuleId)
            .GreaterThan(0)
            .WithMessage("ModuleId must be greater than 0");

    }
}