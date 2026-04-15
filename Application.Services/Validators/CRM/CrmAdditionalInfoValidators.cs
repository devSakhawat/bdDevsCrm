using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmAdditionalInfoRecord.
/// </summary>
public class CreateCrmAdditionalInfoRecordValidator : BaseRecordValidator<CreateCrmAdditionalInfoRecord>
{
    public CreateCrmAdditionalInfoRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.HealthNmedicalNeedsRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.HealthNmedicalNeedsRemarks))
            .WithMessage($"HealthNmedicalNeedsRemarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AdditionalInformationRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInformationRemarks))
            .WithMessage($"AdditionalInformationRemarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmAdditionalInfoRecord.
/// </summary>
public class UpdateCrmAdditionalInfoRecordValidator : BaseRecordValidator<UpdateCrmAdditionalInfoRecord>
{
    public UpdateCrmAdditionalInfoRecordValidator()
    {
        RuleFor(x => x.AdditionalInfoId)
            .GreaterThan(0)
            .WithMessage("AdditionalInfoId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.HealthNmedicalNeedsRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.HealthNmedicalNeedsRemarks))
            .WithMessage($"HealthNmedicalNeedsRemarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AdditionalInformationRemarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInformationRemarks))
            .WithMessage($"AdditionalInformationRemarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmAdditionalInfoRecord.
/// </summary>
public class DeleteCrmAdditionalInfoRecordValidator : BaseRecordValidator<DeleteCrmAdditionalInfoRecord>
{
    public DeleteCrmAdditionalInfoRecordValidator()
    {
        RuleFor(x => x.AdditionalInfoId)
            .GreaterThan(0)
            .WithMessage("AdditionalInfoId must be greater than 0");

    }
}
