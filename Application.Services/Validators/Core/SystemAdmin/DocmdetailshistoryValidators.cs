using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocmdetailshistoryRecord.
/// </summary>
public class CreateDocmdetailshistoryRecordValidator : BaseRecordValidator<CreateDocmdetailshistoryRecord>
{
    public CreateDocmdetailshistoryRecordValidator()
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
/// Validator for UpdateDocmdetailshistoryRecord.
/// </summary>
public class UpdateDocmdetailshistoryRecordValidator : BaseRecordValidator<UpdateDocmdetailshistoryRecord>
{
    public UpdateDocmdetailshistoryRecordValidator()
    {
        RuleFor(x => x.DocumentHistoryId)
            .GreaterThan(0)
            .WithMessage("DocumentHistoryId must be greater than 0");

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
/// Validator for DeleteDocmdetailshistoryRecord.
/// </summary>
public class DeleteDocmdetailshistoryRecordValidator : BaseRecordValidator<DeleteDocmdetailshistoryRecord>
{
    public DeleteDocmdetailshistoryRecordValidator()
    {
        RuleFor(x => x.DocumentHistoryId)
            .GreaterThan(0)
            .WithMessage("DocumentHistoryId must be greater than 0");

    }
}