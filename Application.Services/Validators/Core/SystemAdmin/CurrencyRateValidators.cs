using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCurrencyRateRecord.
/// </summary>
public class CreateCurrencyRateRecordValidator : BaseRecordValidator<CreateCurrencyRateRecord>
{
    public CreateCurrencyRateRecordValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID must be greater than 0");

        RuleFor(x => x.CurrencyRateRation)
            .GreaterThan(0)
            .When(x => x.CurrencyRateRation.HasValue)
            .WithMessage("Currency rate must be greater than 0");
    }
}

/// <summary>
/// Validator for UpdateCurrencyRateRecord.
/// </summary>
public class UpdateCurrencyRateRecordValidator : BaseRecordValidator<UpdateCurrencyRateRecord>
{
    public UpdateCurrencyRateRecordValidator()
    {
        RuleFor(x => x.CurrencyRateId)
            .GreaterThan(0)
            .WithMessage("Currency rate ID must be greater than 0");

        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID must be greater than 0");

        RuleFor(x => x.CurrencyRateRation)
            .GreaterThan(0)
            .When(x => x.CurrencyRateRation.HasValue)
            .WithMessage("Currency rate must be greater than 0");
    }
}

/// <summary>
/// Validator for DeleteCurrencyRateRecord.
/// </summary>
public class DeleteCurrencyRateRecordValidator : BaseRecordValidator<DeleteCurrencyRateRecord>
{
    public DeleteCurrencyRateRecordValidator()
    {
        RuleFor(x => x.CurrencyRateId)
            .GreaterThan(0)
            .WithMessage("Currency rate ID must be greater than 0");
    }
}
