using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocmdetailsRecord.
/// </summary>
public class CreateDocmdetailsRecordValidator : BaseRecordValidator<CreateDocmdetailsRecord>
{
    public CreateDocmdetailsRecordValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("DepartmentId must be greater than 0");

        RuleFor(x => x.Subject)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Subject))
            .WithMessage($"Subject cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Filename)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Filename))
            .WithMessage($"Filename cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Filedescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Filedescription))
            .WithMessage($"Filedescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Fullpath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Fullpath))
            .WithMessage($"Fullpath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDocmdetailsRecord.
/// </summary>
public class UpdateDocmdetailsRecordValidator : BaseRecordValidator<UpdateDocmdetailsRecord>
{
    public UpdateDocmdetailsRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("DepartmentId must be greater than 0");

        RuleFor(x => x.Subject)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Subject))
            .WithMessage($"Subject cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Filename)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Filename))
            .WithMessage($"Filename cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Filedescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Filedescription))
            .WithMessage($"Filedescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Fullpath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Fullpath))
            .WithMessage($"Fullpath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDocmdetailsRecord.
/// </summary>
public class DeleteDocmdetailsRecordValidator : BaseRecordValidator<DeleteDocmdetailsRecord>
{
    public DeleteDocmdetailsRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

    }
}