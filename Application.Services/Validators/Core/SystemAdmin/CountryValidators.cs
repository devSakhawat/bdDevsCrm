using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCountryRecord.
/// </summary>
public class CreateCountryRecordValidator : BaseRecordValidator<CreateCountryRecord>
{
    public CreateCountryRecordValidator()
    {
        RuleFor(x => x.CountryName)
            .NotEmpty()
            .WithMessage("CountryName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CountryName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CountryCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.CountryCode))
            .WithMessage($"CountryCode cannot exceed {MaxCodeLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCountryRecord.
/// </summary>
public class UpdateCountryRecordValidator : BaseRecordValidator<UpdateCountryRecord>
{
    public UpdateCountryRecordValidator()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.CountryName)
            .NotEmpty()
            .WithMessage("CountryName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"CountryName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CountryCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.CountryCode))
            .WithMessage($"CountryCode cannot exceed {MaxCodeLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCountryRecord.
/// </summary>
public class DeleteCountryRecordValidator : BaseRecordValidator<DeleteCountryRecord>
{
    public DeleteCountryRecordValidator()
    {
        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

    }
}