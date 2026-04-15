using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmPermanentAddressRecord.
/// </summary>
public class CreateCrmPermanentAddressRecordValidator : BaseRecordValidator<CreateCrmPermanentAddressRecord>
{
    public CreateCrmPermanentAddressRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.Address)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage($"Address cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.City)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.City))
            .WithMessage($"City cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.State)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.State))
            .WithMessage($"State cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.PostalCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PostalCode))
            .WithMessage($"PostalCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmPermanentAddressRecord.
/// </summary>
public class UpdateCrmPermanentAddressRecordValidator : BaseRecordValidator<UpdateCrmPermanentAddressRecord>
{
    public UpdateCrmPermanentAddressRecordValidator()
    {
        RuleFor(x => x.PermanentAddressId)
            .GreaterThan(0)
            .WithMessage("PermanentAddressId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.Address)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.Address))
            .WithMessage($"Address cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.City)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.City))
            .WithMessage($"City cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.State)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.State))
            .WithMessage($"State cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CountryId)
            .GreaterThan(0)
            .WithMessage("CountryId must be greater than 0");

        RuleFor(x => x.PostalCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PostalCode))
            .WithMessage($"PostalCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmPermanentAddressRecord.
/// </summary>
public class DeleteCrmPermanentAddressRecordValidator : BaseRecordValidator<DeleteCrmPermanentAddressRecord>
{
    public DeleteCrmPermanentAddressRecordValidator()
    {
        RuleFor(x => x.PermanentAddressId)
            .GreaterThan(0)
            .WithMessage("PermanentAddressId must be greater than 0");

    }
}
