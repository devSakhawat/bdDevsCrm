using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateDmsDocumentRecord.
/// </summary>
public class CreateDmsDocumentRecordValidator : BaseRecordValidator<CreateDmsDocumentRecord>
{
    public CreateDmsDocumentRecordValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Title cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FileName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FileExtension)
            .NotEmpty()
            .WithMessage("FileExtension is required")
            .MaximumLength(MaxCodeLength)
            .WithMessage($"FileExtension cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("FileSize must be greater than 0");

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("FilePath is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UploadedByUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UploadedByUserId))
            .WithMessage($"UploadedByUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentTypeId)
            .GreaterThan(0)
            .WithMessage("DocumentTypeId must be greater than 0");

        RuleFor(x => x.ReferenceEntityType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityType))
            .WithMessage($"ReferenceEntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReferenceEntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityId))
            .WithMessage($"ReferenceEntityId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.SystemTag)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SystemTag))
            .WithMessage($"SystemTag cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDmsDocumentRecord.
/// </summary>
public class UpdateDmsDocumentRecordValidator : BaseRecordValidator<UpdateDmsDocumentRecord>
{
    public UpdateDmsDocumentRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Title cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FileName)
            .NotEmpty()
            .WithMessage("FileName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FileName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.FileExtension)
            .NotEmpty()
            .WithMessage("FileExtension is required")
            .MaximumLength(MaxCodeLength)
            .WithMessage($"FileExtension cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.FileSize)
            .GreaterThan(0)
            .WithMessage("FileSize must be greater than 0");

        RuleFor(x => x.FilePath)
            .NotEmpty()
            .WithMessage("FilePath is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"FilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UploadedByUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UploadedByUserId))
            .WithMessage($"UploadedByUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentTypeId)
            .GreaterThan(0)
            .WithMessage("DocumentTypeId must be greater than 0");

        RuleFor(x => x.ReferenceEntityType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityType))
            .WithMessage($"ReferenceEntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReferenceEntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityId))
            .WithMessage($"ReferenceEntityId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.SystemTag)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SystemTag))
            .WithMessage($"SystemTag cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDmsDocumentRecord.
/// </summary>
public class DeleteDmsDocumentRecordValidator : BaseRecordValidator<DeleteDmsDocumentRecord>
{
    public DeleteDmsDocumentRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

    }
}
