using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmWorkExperienceRecord.
/// </summary>
public class CreateCrmWorkExperienceRecordValidator : BaseRecordValidator<CreateCrmWorkExperienceRecord>
{
    public CreateCrmWorkExperienceRecordValidator()
    {
        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.NameOfEmployer)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.NameOfEmployer))
            .WithMessage($"NameOfEmployer cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Position)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Position))
            .WithMessage($"Position cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MainResponsibility)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.MainResponsibility))
            .WithMessage($"MainResponsibility cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ScannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ScannedCopyPath))
            .WithMessage($"ScannedCopyPath cannot exceed {MaxStringLength} characters");

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
/// Validator for UpdateCrmWorkExperienceRecord.
/// </summary>
public class UpdateCrmWorkExperienceRecordValidator : BaseRecordValidator<UpdateCrmWorkExperienceRecord>
{
    public UpdateCrmWorkExperienceRecordValidator()
    {
        RuleFor(x => x.WorkExperienceId)
            .GreaterThan(0)
            .WithMessage("WorkExperienceId must be greater than 0");

        RuleFor(x => x.ApplicantId)
            .GreaterThan(0)
            .WithMessage("ApplicantId must be greater than 0");

        RuleFor(x => x.NameOfEmployer)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.NameOfEmployer))
            .WithMessage($"NameOfEmployer cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Position)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Position))
            .WithMessage($"Position cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MainResponsibility)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.MainResponsibility))
            .WithMessage($"MainResponsibility cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ScannedCopyPath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ScannedCopyPath))
            .WithMessage($"ScannedCopyPath cannot exceed {MaxStringLength} characters");

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
/// Validator for DeleteCrmWorkExperienceRecord.
/// </summary>
public class DeleteCrmWorkExperienceRecordValidator : BaseRecordValidator<DeleteCrmWorkExperienceRecord>
{
    public DeleteCrmWorkExperienceRecordValidator()
    {
        RuleFor(x => x.WorkExperienceId)
            .GreaterThan(0)
            .WithMessage("WorkExperienceId must be greater than 0");

    }
}
