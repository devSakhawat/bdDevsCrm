using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateSystemSettingsRecord.
/// </summary>
public class CreateSystemSettingsRecordValidator : BaseRecordValidator<CreateSystemSettingsRecord>
{
    public CreateSystemSettingsRecordValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.Theme)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Theme))
            .WithMessage($"Theme cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Language)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Language))
            .WithMessage($"Language cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ResetPass)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ResetPass))
            .WithMessage($"ResetPass cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CustomStatusForNoOutPunch)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CustomStatusForNoOutPunch))
            .WithMessage($"CustomStatusForNoOutPunch cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateSystemSettingsRecord.
/// </summary>
public class UpdateSystemSettingsRecordValidator : BaseRecordValidator<UpdateSystemSettingsRecord>
{
    public UpdateSystemSettingsRecordValidator()
    {
        RuleFor(x => x.SettingsId)
            .GreaterThan(0)
            .WithMessage("SettingsId must be greater than 0");

        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.Theme)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Theme))
            .WithMessage($"Theme cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Language)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Language))
            .WithMessage($"Language cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ResetPass)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ResetPass))
            .WithMessage($"ResetPass cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CustomStatusForNoOutPunch)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CustomStatusForNoOutPunch))
            .WithMessage($"CustomStatusForNoOutPunch cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteSystemSettingsRecord.
/// </summary>
public class DeleteSystemSettingsRecordValidator : BaseRecordValidator<DeleteSystemSettingsRecord>
{
    public DeleteSystemSettingsRecordValidator()
    {
        RuleFor(x => x.SettingsId)
            .GreaterThan(0)
            .WithMessage("SettingsId must be greater than 0");

    }
}