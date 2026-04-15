using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmPaymentMethodRecord.
/// </summary>
public class CreateCrmPaymentMethodRecordValidator : BaseRecordValidator<CreateCrmPaymentMethodRecord>
{
    public CreateCrmPaymentMethodRecordValidator()
    {
        RuleFor(x => x.PaymentMethodName)
            .NotEmpty()
            .WithMessage("PaymentMethodName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"PaymentMethodName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.PaymentMethodCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentMethodCode))
            .WithMessage($"PaymentMethodCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProcessingFeeType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ProcessingFeeType))
            .WithMessage($"ProcessingFeeType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmPaymentMethodRecord.
/// </summary>
public class UpdateCrmPaymentMethodRecordValidator : BaseRecordValidator<UpdateCrmPaymentMethodRecord>
{
    public UpdateCrmPaymentMethodRecordValidator()
    {
        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0)
            .WithMessage("PaymentMethodId must be greater than 0");

        RuleFor(x => x.PaymentMethodName)
            .NotEmpty()
            .WithMessage("PaymentMethodName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"PaymentMethodName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.PaymentMethodCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.PaymentMethodCode))
            .WithMessage($"PaymentMethodCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ProcessingFeeType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ProcessingFeeType))
            .WithMessage($"ProcessingFeeType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmPaymentMethodRecord.
/// </summary>
public class DeleteCrmPaymentMethodRecordValidator : BaseRecordValidator<DeleteCrmPaymentMethodRecord>
{
    public DeleteCrmPaymentMethodRecordValidator()
    {
        RuleFor(x => x.PaymentMethodId)
            .GreaterThan(0)
            .WithMessage("PaymentMethodId must be greater than 0");

    }
}
