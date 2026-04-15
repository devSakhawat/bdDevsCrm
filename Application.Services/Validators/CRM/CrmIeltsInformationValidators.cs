using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmIeltsInformationRecord.
/// </summary>
public class CreateCrmIeltsInformationRecordValidator : BaseRecordValidator<CreateCrmIeltsInformationRecord>
{
    public CreateCrmIeltsInformationRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.IeltsscannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IeltsscannedCopyPath))
            .WithMessage($"IeltsscannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IeltsadditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IeltsadditionalInformation))
            .WithMessage($"IeltsadditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmIeltsInformationRecord.
/// </summary>
public class UpdateCrmIeltsInformationRecordValidator : BaseRecordValidator<UpdateCrmIeltsInformationRecord>
{
    public UpdateCrmIeltsInformationRecordValidator()
    {
        RuleFor(x => x.IELTSInformationId)
            .GreaterThan(0)
            .WithMessage("IELTSInformationId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.IeltsscannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IeltsscannedCopyPath))
            .WithMessage($"IeltsscannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IeltsadditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IeltsadditionalInformation))
            .WithMessage($"IeltsadditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmIeltsInformationRecord.
/// </summary>
public class DeleteCrmIeltsInformationRecordValidator : BaseRecordValidator<DeleteCrmIeltsInformationRecord>
{
    public DeleteCrmIeltsInformationRecordValidator()
    {
        RuleFor(x => x.IELTSInformationId)
            .GreaterThan(0)
            .WithMessage("IELTSInformationId must be greater than 0");

    }
}
