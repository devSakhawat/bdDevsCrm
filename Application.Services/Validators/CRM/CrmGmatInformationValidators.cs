using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmGmatInformationRecord.
/// </summary>
public class CreateCrmGmatInformationRecordValidator : BaseRecordValidator<CreateCrmGmatInformationRecord>
{
    public CreateCrmGmatInformationRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.GmatscannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GmatscannedCopyPath))
            .WithMessage($"GmatscannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.GmatadditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GmatadditionalInformation))
            .WithMessage($"GmatadditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmGmatInformationRecord.
/// </summary>
public class UpdateCrmGmatInformationRecordValidator : BaseRecordValidator<UpdateCrmGmatInformationRecord>
{
    public UpdateCrmGmatInformationRecordValidator()
    {
        RuleFor(x => x.GMATInformationId)
            .GreaterThan(0)
            .WithMessage("GMATInformationId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.GmatscannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GmatscannedCopyPath))
            .WithMessage($"GmatscannedCopyPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.GmatadditionalInformation)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GmatadditionalInformation))
            .WithMessage($"GmatadditionalInformation cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmGmatInformationRecord.
/// </summary>
public class DeleteCrmGmatInformationRecordValidator : BaseRecordValidator<DeleteCrmGmatInformationRecord>
{
    public DeleteCrmGmatInformationRecordValidator()
    {
        RuleFor(x => x.GMATInformationId)
            .GreaterThan(0)
            .WithMessage("GMATInformationId must be greater than 0");

    }
}
