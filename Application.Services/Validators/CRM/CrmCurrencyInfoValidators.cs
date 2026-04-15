using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmCurrencyInfoRecord.
/// </summary>
public class CreateCrmCurrencyInfoRecordValidator : BaseRecordValidator<CreateCrmCurrencyInfoRecord>
{
    public CreateCrmCurrencyInfoRecordValidator()
    {
        RuleFor(x => x.CurrencyName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.CurrencyName))
            .WithMessage($"CurrencyName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCrmCurrencyInfoRecord.
/// </summary>
public class UpdateCrmCurrencyInfoRecordValidator : BaseRecordValidator<UpdateCrmCurrencyInfoRecord>
{
    public UpdateCrmCurrencyInfoRecordValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("CurrencyId must be greater than 0");

        RuleFor(x => x.CurrencyName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.CurrencyName))
            .WithMessage($"CurrencyName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCrmCurrencyInfoRecord.
/// </summary>
public class DeleteCrmCurrencyInfoRecordValidator : BaseRecordValidator<DeleteCrmCurrencyInfoRecord>
{
    public DeleteCrmCurrencyInfoRecordValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("CurrencyId must be greater than 0");

    }
}
