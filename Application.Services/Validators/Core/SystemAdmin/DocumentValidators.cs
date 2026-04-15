using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocumentRecord.
/// </summary>
public class CreateDocumentRecordValidator : BaseRecordValidator<CreateDocumentRecord>
{
    public CreateDocumentRecordValidator()
    {
        RuleFor(x => x.Titleofdocument)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Titleofdocument))
            .WithMessage($"Titleofdocument cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Attacheddocument)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Attacheddocument))
            .WithMessage($"Attacheddocument cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Summary)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Summary))
            .WithMessage($"Summary cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDocumentRecord.
/// </summary>
public class UpdateDocumentRecordValidator : BaseRecordValidator<UpdateDocumentRecord>
{
    public UpdateDocumentRecordValidator()
    {
        RuleFor(x => x.Titleofdocument)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Titleofdocument))
            .WithMessage($"Titleofdocument cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Attacheddocument)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Attacheddocument))
            .WithMessage($"Attacheddocument cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Summary)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Summary))
            .WithMessage($"Summary cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDocumentRecord.
/// </summary>
public class DeleteDocumentRecordValidator : BaseRecordValidator<DeleteDocumentRecord>
{
    public DeleteDocumentRecordValidator()
    {
        RuleFor(x => x.Documentid)
            .GreaterThan(0)
            .WithMessage("Documentid must be greater than 0");

    }
}