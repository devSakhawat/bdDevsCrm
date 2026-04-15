using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateDmsDocumentFolderRecord.
/// </summary>
public class CreateDmsDocumentFolderRecordValidator : BaseRecordValidator<CreateDmsDocumentFolderRecord>
{
    public CreateDmsDocumentFolderRecordValidator()
    {
        RuleFor(x => x.FolderName)
            .NotEmpty()
            .WithMessage("FolderName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"FolderName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.OwnerId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OwnerId))
            .WithMessage($"OwnerId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReferenceEntityType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityType))
            .WithMessage($"ReferenceEntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReferenceEntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityId))
            .WithMessage($"ReferenceEntityId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDmsDocumentFolderRecord.
/// </summary>
public class UpdateDmsDocumentFolderRecordValidator : BaseRecordValidator<UpdateDmsDocumentFolderRecord>
{
    public UpdateDmsDocumentFolderRecordValidator()
    {
        RuleFor(x => x.FolderId)
            .GreaterThan(0)
            .WithMessage("FolderId must be greater than 0");

        RuleFor(x => x.FolderName)
            .NotEmpty()
            .WithMessage("FolderName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"FolderName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.OwnerId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OwnerId))
            .WithMessage($"OwnerId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReferenceEntityType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityType))
            .WithMessage($"ReferenceEntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ReferenceEntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ReferenceEntityId))
            .WithMessage($"ReferenceEntityId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDmsDocumentFolderRecord.
/// </summary>
public class DeleteDmsDocumentFolderRecordValidator : BaseRecordValidator<DeleteDmsDocumentFolderRecord>
{
    public DeleteDmsDocumentFolderRecordValidator()
    {
        RuleFor(x => x.FolderId)
            .GreaterThan(0)
            .WithMessage("FolderId must be greater than 0");

    }
}
