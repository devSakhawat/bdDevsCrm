using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDocumentParameterRecord.
/// </summary>
public class CreateDocumentParameterRecordValidator : BaseRecordValidator<CreateDocumentParameterRecord>
{
    public CreateDocumentParameterRecordValidator()
    {
        RuleFor(x => x.ParameterName)
            .NotEmpty()
            .WithMessage("ParameterName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ParameterName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ParameterKey)
            .NotEmpty()
            .WithMessage("ParameterKey is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ParameterKey cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ControlRole)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ControlRole))
            .WithMessage($"ControlRole cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DataSource)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DataSource))
            .WithMessage($"DataSource cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DataTextField)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DataTextField))
            .WithMessage($"DataTextField cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DataValueField)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DataValueField))
            .WithMessage($"DataValueField cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CaseCading)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CaseCading))
            .WithMessage($"CaseCading cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDocumentParameterRecord.
/// </summary>
public class UpdateDocumentParameterRecordValidator : BaseRecordValidator<UpdateDocumentParameterRecord>
{
    public UpdateDocumentParameterRecordValidator()
    {
        RuleFor(x => x.ParameterId)
            .GreaterThan(0)
            .WithMessage("ParameterId must be greater than 0");

        RuleFor(x => x.ParameterName)
            .NotEmpty()
            .WithMessage("ParameterName is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ParameterName cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ParameterKey)
            .NotEmpty()
            .WithMessage("ParameterKey is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"ParameterKey cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ControlRole)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ControlRole))
            .WithMessage($"ControlRole cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DataSource)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DataSource))
            .WithMessage($"DataSource cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DataTextField)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DataTextField))
            .WithMessage($"DataTextField cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.DataValueField)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DataValueField))
            .WithMessage($"DataValueField cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CaseCading)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CaseCading))
            .WithMessage($"CaseCading cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDocumentParameterRecord.
/// </summary>
public class DeleteDocumentParameterRecordValidator : BaseRecordValidator<DeleteDocumentParameterRecord>
{
    public DeleteDocumentParameterRecordValidator()
    {
        RuleFor(x => x.ParameterId)
            .GreaterThan(0)
            .WithMessage("ParameterId must be greater than 0");

    }
}