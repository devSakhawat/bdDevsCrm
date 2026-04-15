using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCurrencyRecord.
/// </summary>
public class CreateCurrencyRecordValidator : BaseRecordValidator<CreateCurrencyRecord>
{
    public CreateCurrencyRecordValidator()
    {
        RuleFor(x => x.CurrencyName)
            .NotEmpty()
            .WithMessage("CurrencyName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CurrencyName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .WithMessage("CurrencyCode is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CurrencyCode cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCurrencyRecord.
/// </summary>
public class UpdateCurrencyRecordValidator : BaseRecordValidator<UpdateCurrencyRecord>
{
    public UpdateCurrencyRecordValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("CurrencyId must be greater than 0");

        RuleFor(x => x.CurrencyName)
            .NotEmpty()
            .WithMessage("CurrencyName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CurrencyName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CurrencyCode)
            .NotEmpty()
            .WithMessage("CurrencyCode is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CurrencyCode cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCurrencyRecord.
/// </summary>
public class DeleteCurrencyRecordValidator : BaseRecordValidator<DeleteCurrencyRecord>
{
    public DeleteCurrencyRecordValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("CurrencyId must be greater than 0");

    }
}