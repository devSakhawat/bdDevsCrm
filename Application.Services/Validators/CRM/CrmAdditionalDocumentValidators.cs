using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmAdditionalDocumentRecord.
/// </summary>
public class CreateCrmAdditionalDocumentRecordValidator : BaseRecordValidator<CreateCrmAdditionalDocumentRecord>
{
    public CreateCrmAdditionalDocumentRecordValidator()
    {
        RuleFor(x => x.DocumentTitle)
            .NotEmpty()
            .WithMessage("DocumentTitle is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentPath)
            .NotEmpty()
            .WithMessage("DocumentPath is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentName)
            .NotEmpty()
            .WithMessage("DocumentName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.RecordType)
            .NotEmpty()
            .WithMessage("RecordType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"RecordType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmAdditionalDocumentRecord.
/// </summary>
public class UpdateCrmAdditionalDocumentRecordValidator : BaseRecordValidator<UpdateCrmAdditionalDocumentRecord>
{
    public UpdateCrmAdditionalDocumentRecordValidator()
    {
        RuleFor(x => x.AdditionalDocumentId)
            .GreaterThan(0)
            .WithMessage("AdditionalDocumentId must be greater than 0");

        RuleFor(x => x.DocumentTitle)
            .NotEmpty()
            .WithMessage("DocumentTitle is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentPath)
            .NotEmpty()
            .WithMessage("DocumentPath is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentPath cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentName)
            .NotEmpty()
            .WithMessage("DocumentName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.RecordType)
            .NotEmpty()
            .WithMessage("RecordType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"RecordType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmAdditionalDocumentRecord.
/// </summary>
public class DeleteCrmAdditionalDocumentRecordValidator : BaseRecordValidator<DeleteCrmAdditionalDocumentRecord>
{
    public DeleteCrmAdditionalDocumentRecordValidator()
    {
        RuleFor(x => x.AdditionalDocumentId)
            .GreaterThan(0)
            .WithMessage("AdditionalDocumentId must be greater than 0");

    }
}
