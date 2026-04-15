using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocumentTypeRecord.
/// </summary>
public class CreateDocumentTypeRecordValidator : BaseRecordValidator<CreateDocumentTypeRecord>
{
    public CreateDocumentTypeRecordValidator()
    {
        RuleFor(x => x.Documentname)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Documentname))
            .WithMessage($"Documentname cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDocumentTypeRecord.
/// </summary>
public class UpdateDocumentTypeRecordValidator : BaseRecordValidator<UpdateDocumentTypeRecord>
{
    public UpdateDocumentTypeRecordValidator()
    {
        RuleFor(x => x.Documentname)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Documentname))
            .WithMessage($"Documentname cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDocumentTypeRecord.
/// </summary>
public class DeleteDocumentTypeRecordValidator : BaseRecordValidator<DeleteDocumentTypeRecord>
{
    public DeleteDocumentTypeRecordValidator()
    {
        RuleFor(x => x.Documenttypeid)
            .GreaterThan(0)
            .WithMessage("Documenttypeid must be greater than 0");

    }
}