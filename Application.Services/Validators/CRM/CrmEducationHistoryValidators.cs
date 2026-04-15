using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmEducationHistoryRecord.
/// </summary>
public class CreateCrmEducationHistoryRecordValidator : BaseRecordValidator<CreateCrmEducationHistoryRecord>
{
    public CreateCrmEducationHistoryRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.Institution)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Institution))
            .WithMessage($"Institution cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Qualification)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Qualification))
            .WithMessage($"Qualification cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Grade)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Grade))
            .WithMessage($"Grade cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentPath))
            .WithMessage($"DocumentPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentName)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentName))
            .WithMessage($"DocumentName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmEducationHistoryRecord.
/// </summary>
public class UpdateCrmEducationHistoryRecordValidator : BaseRecordValidator<UpdateCrmEducationHistoryRecord>
{
    public UpdateCrmEducationHistoryRecordValidator()
    {
        RuleFor(x => x.EducationHistoryId)
            .GreaterThan(0)
            .WithMessage("EducationHistoryId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.Institution)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Institution))
            .WithMessage($"Institution cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Qualification)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Qualification))
            .WithMessage($"Qualification cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Grade)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Grade))
            .WithMessage($"Grade cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentPath))
            .WithMessage($"DocumentPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentName)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentName))
            .WithMessage($"DocumentName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmEducationHistoryRecord.
/// </summary>
public class DeleteCrmEducationHistoryRecordValidator : BaseRecordValidator<DeleteCrmEducationHistoryRecord>
{
    public DeleteCrmEducationHistoryRecordValidator()
    {
        RuleFor(x => x.EducationHistoryId)
            .GreaterThan(0)
            .WithMessage("EducationHistoryId must be greater than 0");

    }
}
