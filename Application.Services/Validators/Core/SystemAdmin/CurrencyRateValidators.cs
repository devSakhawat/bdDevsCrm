using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

public class CreateCurrencyRateRecordValidator : BaseRecordValidator<CreateCurrencyRateRecord>
{
    public CreateCurrencyRateRecordValidator()
    {
        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID is required");

        RuleFor(x => x.CurrencyRateRation)
            .GreaterThan(0)
            .When(x => x.CurrencyRateRation.HasValue)
            .WithMessage("Currency rate must be greater than 0");
    }
}

public class UpdateCurrencyRateRecordValidator : BaseRecordValidator<UpdateCurrencyRateRecord>
{
    public UpdateCurrencyRateRecordValidator()
    {
        RuleFor(x => x.CurencyRateId)
            .GreaterThan(0)
            .WithMessage("Currency Rate ID must be greater than 0");

        RuleFor(x => x.CurrencyId)
            .GreaterThan(0)
            .WithMessage("Currency ID is required");

        RuleFor(x => x.CurrencyRateRation)
            .GreaterThan(0)
            .When(x => x.CurrencyRateRation.HasValue)
            .WithMessage("Currency rate must be greater than 0");
    }
}

public class DeleteCurrencyRateRecordValidator : BaseRecordValidator<DeleteCurrencyRateRecord>
{
    public DeleteCurrencyRateRecordValidator()
    {
        RuleFor(x => x.CurencyRateId)
            .GreaterThan(0)
            .WithMessage("Currency Rate ID must be greater than 0");
    }
}
