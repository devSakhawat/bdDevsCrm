using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmApplicantReferenceRecord.
/// </summary>
public class CreateCrmApplicantReferenceRecordValidator : BaseRecordValidator<CreateCrmApplicantReferenceRecord>
{
    public CreateCrmApplicantReferenceRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Designation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Designation))
            .WithMessage($"Designation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Institution)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Institution))
            .WithMessage($"Institution cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EmailId)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.EmailId))
            .WithMessage($"EmailId cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.PhoneNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneNo))
            .WithMessage($"PhoneNo cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.FaxNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.FaxNo))
            .WithMessage($"FaxNo cannot exceed {MaxPhoneLength} characters");

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

        RuleFor(x => x.PostOrZipCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PostOrZipCode))
            .WithMessage($"PostOrZipCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmApplicantReferenceRecord.
/// </summary>
public class UpdateCrmApplicantReferenceRecordValidator : BaseRecordValidator<UpdateCrmApplicantReferenceRecord>
{
    public UpdateCrmApplicantReferenceRecordValidator()
    {
        RuleFor(x => x.ApplicantReferenceId)
            .GreaterThan(0)
            .WithMessage("ApplicantReferenceId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Designation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Designation))
            .WithMessage($"Designation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Institution)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Institution))
            .WithMessage($"Institution cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EmailId)
            .MaximumLength(MaxEmailLength)
            .When(x => !string.IsNullOrEmpty(x.EmailId))
            .WithMessage($"EmailId cannot exceed {MaxEmailLength} characters");

        RuleFor(x => x.PhoneNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.PhoneNo))
            .WithMessage($"PhoneNo cannot exceed {MaxPhoneLength} characters");

        RuleFor(x => x.FaxNo)
            .MaximumLength(MaxPhoneLength)
            .When(x => !string.IsNullOrEmpty(x.FaxNo))
            .WithMessage($"FaxNo cannot exceed {MaxPhoneLength} characters");

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

        RuleFor(x => x.PostOrZipCode)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PostOrZipCode))
            .WithMessage($"PostOrZipCode cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmApplicantReferenceRecord.
/// </summary>
public class DeleteCrmApplicantReferenceRecordValidator : BaseRecordValidator<DeleteCrmApplicantReferenceRecord>
{
    public DeleteCrmApplicantReferenceRecordValidator()
    {
        RuleFor(x => x.ApplicantReferenceId)
            .GreaterThan(0)
            .WithMessage("ApplicantReferenceId must be greater than 0");

    }
}
