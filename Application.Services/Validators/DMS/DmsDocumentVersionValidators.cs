using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateDmsDocumentVersionRecord.
/// </summary>
public class CreateDmsDocumentVersionRecordValidator : BaseRecordValidator<CreateDmsDocumentVersionRecord>
{
    public CreateDmsDocumentVersionRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.VersionNumber)
            .GreaterThan(0)
            .WithMessage("VersionNumber must be greater than 0");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FileName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("FilePath is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UploadedBy)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UploadedBy))
            .WithMessage($"UploadedBy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.VersionNotes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.VersionNotes))
            .WithMessage($"VersionNotes cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDmsDocumentVersionRecord.
/// </summary>
public class UpdateDmsDocumentVersionRecordValidator : BaseRecordValidator<UpdateDmsDocumentVersionRecord>
{
    public UpdateDmsDocumentVersionRecordValidator()
    {
        RuleFor(x => x.VersionId)
            .GreaterThan(0)
            .WithMessage("VersionId must be greater than 0");

        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.VersionNumber)
            .GreaterThan(0)
            .WithMessage("VersionNumber must be greater than 0");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FileName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("FilePath is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UploadedBy)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UploadedBy))
            .WithMessage($"UploadedBy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.VersionNotes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.VersionNotes))
            .WithMessage($"VersionNotes cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDmsDocumentVersionRecord.
/// </summary>
public class DeleteDmsDocumentVersionRecordValidator : BaseRecordValidator<DeleteDmsDocumentVersionRecord>
{
    public DeleteDmsDocumentVersionRecordValidator()
    {
        RuleFor(x => x.VersionId)
            .GreaterThan(0)
            .WithMessage("VersionId must be greater than 0");

    }
}
