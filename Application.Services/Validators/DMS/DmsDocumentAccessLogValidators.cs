using bdDevs.Shared.Records.DMS;
using FluentValidation;

namespace Application.Services.Validators.DMS;

/// <summary>
/// Validator for CreateDmsDocumentAccessLogRecord.
/// </summary>
public class CreateDmsDocumentAccessLogRecordValidator : BaseRecordValidator<CreateDmsDocumentAccessLogRecord>
{
    public CreateDmsDocumentAccessLogRecordValidator()
    {
        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.AccessedByUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AccessedByUserId))
            .WithMessage($"AccessedByUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Action)
            .NotEmpty()
            .WithMessage("Action is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"Action cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IpAddress)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.IpAddress))
            .WithMessage($"IpAddress cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.DeviceInfo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DeviceInfo))
            .WithMessage($"DeviceInfo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MacAddress)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.MacAddress))
            .WithMessage($"MacAddress cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateDmsDocumentAccessLogRecord.
/// </summary>
public class UpdateDmsDocumentAccessLogRecordValidator : BaseRecordValidator<UpdateDmsDocumentAccessLogRecord>
{
    public UpdateDmsDocumentAccessLogRecordValidator()
    {
        RuleFor(x => x.LogId)
            .GreaterThan(0)
            .WithMessage("LogId must be greater than 0");

        RuleFor(x => x.DocumentId)
            .GreaterThan(0)
            .WithMessage("DocumentId must be greater than 0");

        RuleFor(x => x.AccessedByUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AccessedByUserId))
            .WithMessage($"AccessedByUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Action)
            .NotEmpty()
            .WithMessage("Action is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"Action cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IpAddress)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.IpAddress))
            .WithMessage($"IpAddress cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.DeviceInfo)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.DeviceInfo))
            .WithMessage($"DeviceInfo cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MacAddress)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.MacAddress))
            .WithMessage($"MacAddress cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.Notes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Notes))
            .WithMessage($"Notes cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteDmsDocumentAccessLogRecord.
/// </summary>
public class DeleteDmsDocumentAccessLogRecordValidator : BaseRecordValidator<DeleteDmsDocumentAccessLogRecord>
{
    public DeleteDmsDocumentAccessLogRecordValidator()
    {
        RuleFor(x => x.LogId)
            .GreaterThan(0)
            .WithMessage("LogId must be greater than 0");

    }
}
