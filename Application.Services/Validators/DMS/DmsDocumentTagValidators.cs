using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateDmsDocumentTagRecord.
/// </summary>
public class CreateDmsDocumentTagRecordValidator : BaseRecordValidator<CreateDmsDocumentTagRecord>
{
    public CreateDmsDocumentTagRecordValidator()
    {
        RuleFor(x => x.DocumentTagName)
            .NotEmpty()
            .WithMessage("DocumentTagName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"DocumentTagName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDmsDocumentTagRecord.
/// </summary>
public class UpdateDmsDocumentTagRecordValidator : BaseRecordValidator<UpdateDmsDocumentTagRecord>
{
    public UpdateDmsDocumentTagRecordValidator()
    {
        RuleFor(x => x.TagId)
            .GreaterThan(0)
            .WithMessage("TagId must be greater than 0");

        RuleFor(x => x.DocumentTagName)
            .NotEmpty()
            .WithMessage("DocumentTagName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"DocumentTagName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDmsDocumentTagRecord.
/// </summary>
public class DeleteDmsDocumentTagRecordValidator : BaseRecordValidator<DeleteDmsDocumentTagRecord>
{
    public DeleteDmsDocumentTagRecordValidator()
    {
        RuleFor(x => x.TagId)
            .GreaterThan(0)
            .WithMessage("TagId must be greater than 0");

    }
}
