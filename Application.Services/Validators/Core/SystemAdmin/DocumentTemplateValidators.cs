using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocumentTemplateRecord.
/// </summary>
public class CreateDocumentTemplateRecordValidator : BaseRecordValidator<CreateDocumentTemplateRecord>
{
    public CreateDocumentTemplateRecordValidator()
    {
        RuleFor(x => x.DocumentTitle)
            .NotEmpty()
            .WithMessage("DocumentTitle is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentText)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentText))
            .WithMessage($"DocumentText cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TemplateName)
            .NotEmpty()
            .WithMessage("TemplateName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"TemplateName cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDocumentTemplateRecord.
/// </summary>
public class UpdateDocumentTemplateRecordValidator : BaseRecordValidator<UpdateDocumentTemplateRecord>
{
    public UpdateDocumentTemplateRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.DocumentTitle)
            .NotEmpty()
            .WithMessage("DocumentTitle is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"DocumentTitle cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DocumentText)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DocumentText))
            .WithMessage($"DocumentText cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TemplateName)
            .NotEmpty()
            .WithMessage("TemplateName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"TemplateName cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDocumentTemplateRecord.
/// </summary>
public class DeleteDocumentTemplateRecordValidator : BaseRecordValidator<DeleteDocumentTemplateRecord>
{
    public DeleteDocumentTemplateRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

    }
}