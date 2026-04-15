using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmPresentAddressRecord.
/// </summary>
public class CreateCrmPresentAddressRecordValidator : BaseRecordValidator<CreateCrmPresentAddressRecord>
{
    public CreateCrmPresentAddressRecordValidator()
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
/// Validator for UpdateCrmPresentAddressRecord.
/// </summary>
public class UpdateCrmPresentAddressRecordValidator : BaseRecordValidator<UpdateCrmPresentAddressRecord>
{
    public UpdateCrmPresentAddressRecordValidator()
    {
        RuleFor(x => x.PresentAddressId)
            .GreaterThan(0)
            .WithMessage("PresentAddressId must be greater than 0");

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
/// Validator for DeleteCrmPresentAddressRecord.
/// </summary>
public class DeleteCrmPresentAddressRecordValidator : BaseRecordValidator<DeleteCrmPresentAddressRecord>
{
    public DeleteCrmPresentAddressRecordValidator()
    {
        RuleFor(x => x.PresentAddressId)
            .GreaterThan(0)
            .WithMessage("PresentAddressId must be greater than 0");

    }
}
