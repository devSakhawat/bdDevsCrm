using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmToeflInformationRecord.
/// </summary>
public class CreateCrmToeflInformationRecordValidator : BaseRecordValidator<CreateCrmToeflInformationRecord>
{
    public CreateCrmToeflInformationRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.ToeflscannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ToeflscannedCopyPath))
            .WithMessage($"ToeflscannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ToefladditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ToefladditionalInformation))
            .WithMessage($"ToefladditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmToeflInformationRecord.
/// </summary>
public class UpdateCrmToeflInformationRecordValidator : BaseRecordValidator<UpdateCrmToeflInformationRecord>
{
    public UpdateCrmToeflInformationRecordValidator()
    {
        RuleFor(x => x.TOEFLInformationId)
            .GreaterThan(0)
            .WithMessage("TOEFLInformationId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.ToeflscannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ToeflscannedCopyPath))
            .WithMessage($"ToeflscannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ToefladditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ToefladditionalInformation))
            .WithMessage($"ToefladditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmToeflInformationRecord.
/// </summary>
public class DeleteCrmToeflInformationRecordValidator : BaseRecordValidator<DeleteCrmToeflInformationRecord>
{
    public DeleteCrmToeflInformationRecordValidator()
    {
        RuleFor(x => x.TOEFLInformationId)
            .GreaterThan(0)
            .WithMessage("TOEFLInformationId must be greater than 0");

    }
}
