using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateDmsDocumentTypeRecord.
/// </summary>
public class CreateDmsDocumentTypeRecordValidator : BaseRecordValidator<CreateDmsDocumentTypeRecord>
{
    public CreateDmsDocumentTypeRecordValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.DocumentType)
            .NotEmpty()
            .WithMessage("DocumentType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AcceptedExtensions)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AcceptedExtensions))
            .WithMessage($"AcceptedExtensions cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDmsDocumentTypeRecord.
/// </summary>
public class UpdateDmsDocumentTypeRecordValidator : BaseRecordValidator<UpdateDmsDocumentTypeRecord>
{
    public UpdateDmsDocumentTypeRecordValidator()
    {
        RuleFor(x => x.DocumentTypeId)
            .GreaterThan(0)
            .WithMessage("DocumentTypeId must be greater than 0");

        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.DocumentType)
            .NotEmpty()
            .WithMessage("DocumentType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AcceptedExtensions)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AcceptedExtensions))
            .WithMessage($"AcceptedExtensions cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDmsDocumentTypeRecord.
/// </summary>
public class DeleteDmsDocumentTypeRecordValidator : BaseRecordValidator<DeleteDmsDocumentTypeRecord>
{
    public DeleteDmsDocumentTypeRecordValidator()
    {
        RuleFor(x => x.DocumentTypeId)
            .GreaterThan(0)
            .WithMessage("DocumentTypeId must be greater than 0");

    }
}
