using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateFileUpdateHistoryRecord.
/// </summary>
public class CreateFileUpdateHistoryRecordValidator : BaseRecordValidator<CreateFileUpdateHistoryRecord>
{
    public CreateFileUpdateHistoryRecordValidator()
    {
        RuleFor(x => x.EntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EntityId))
            .WithMessage($"EntityId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EntityType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EntityType))
            .WithMessage($"EntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentType))
            .WithMessage($"DocumentType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OldFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OldFilePath))
            .WithMessage($"OldFilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.NewFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.NewFilePath))
            .WithMessage($"NewFilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UpdatedBy)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UpdatedBy))
            .WithMessage($"UpdatedBy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UpdateReason)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UpdateReason))
            .WithMessage($"UpdateReason cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateFileUpdateHistoryRecord.
/// </summary>
public class UpdateFileUpdateHistoryRecordValidator : BaseRecordValidator<UpdateFileUpdateHistoryRecord>
{
    public UpdateFileUpdateHistoryRecordValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");

        RuleFor(x => x.EntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EntityId))
            .WithMessage($"EntityId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EntityType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EntityType))
            .WithMessage($"EntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentType))
            .WithMessage($"DocumentType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OldFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OldFilePath))
            .WithMessage($"OldFilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.NewFilePath)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.NewFilePath))
            .WithMessage($"NewFilePath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UpdatedBy)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UpdatedBy))
            .WithMessage($"UpdatedBy cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UpdateReason)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UpdateReason))
            .WithMessage($"UpdateReason cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteFileUpdateHistoryRecord.
/// </summary>
public class DeleteFileUpdateHistoryRecordValidator : BaseRecordValidator<DeleteFileUpdateHistoryRecord>
{
    public DeleteFileUpdateHistoryRecordValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0");

    }
}
