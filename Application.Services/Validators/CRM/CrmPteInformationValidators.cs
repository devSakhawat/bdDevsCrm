using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmPteInformationRecord.
/// </summary>
public class CreateCrmPteInformationRecordValidator : BaseRecordValidator<CreateCrmPteInformationRecord>
{
    public CreateCrmPteInformationRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.PtescannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PtescannedCopyPath))
            .WithMessage($"PtescannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PteadditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PteadditionalInformation))
            .WithMessage($"PteadditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmPteInformationRecord.
/// </summary>
public class UpdateCrmPteInformationRecordValidator : BaseRecordValidator<UpdateCrmPteInformationRecord>
{
    public UpdateCrmPteInformationRecordValidator()
    {
        RuleFor(x => x.PTEInformationId)
            .GreaterThan(0)
            .WithMessage("PTEInformationId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.PtescannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PtescannedCopyPath))
            .WithMessage($"PtescannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.PteadditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.PteadditionalInformation))
            .WithMessage($"PteadditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmPteInformationRecord.
/// </summary>
public class DeleteCrmPteInformationRecordValidator : BaseRecordValidator<DeleteCrmPteInformationRecord>
{
    public DeleteCrmPteInformationRecordValidator()
    {
        RuleFor(x => x.PTEInformationId)
            .GreaterThan(0)
            .WithMessage("PTEInformationId must be greater than 0");

    }
}
