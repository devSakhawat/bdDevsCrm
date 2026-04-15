using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmOthersInformationRecord.
/// </summary>
public class CreateCrmOthersInformationRecordValidator : BaseRecordValidator<CreateCrmOthersInformationRecord>
{
    public CreateCrmOthersInformationRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.AdditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInformation))
            .WithMessage($"AdditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OthersScannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OthersScannedCopyPath))
            .WithMessage($"OthersScannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmOthersInformationRecord.
/// </summary>
public class UpdateCrmOthersInformationRecordValidator : BaseRecordValidator<UpdateCrmOthersInformationRecord>
{
    public UpdateCrmOthersInformationRecordValidator()
    {
        RuleFor(x => x.OthersInformationId)
            .GreaterThan(0)
            .WithMessage("OthersInformationId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.AdditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AdditionalInformation))
            .WithMessage($"AdditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OthersScannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OthersScannedCopyPath))
            .WithMessage($"OthersScannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmOthersInformationRecord.
/// </summary>
public class DeleteCrmOthersInformationRecordValidator : BaseRecordValidator<DeleteCrmOthersInformationRecord>
{
    public DeleteCrmOthersInformationRecordValidator()
    {
        RuleFor(x => x.OthersInformationId)
            .GreaterThan(0)
            .WithMessage("OthersInformationId must be greater than 0");

    }
}
