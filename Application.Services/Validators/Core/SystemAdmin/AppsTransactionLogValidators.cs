using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAppsTransactionLogRecord.
/// </summary>
public class CreateAppsTransactionLogRecordValidator : BaseRecordValidator<CreateAppsTransactionLogRecord>
{
    public CreateAppsTransactionLogRecordValidator()
    {
        RuleFor(x => x.TransactionType)
            .NotEmpty()
            .WithMessage("TransactionType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"TransactionType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Request)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Request))
            .WithMessage($"Request cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Response)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Response))
            .WithMessage($"Response cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AppsUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AppsUserId))
            .WithMessage($"AppsUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EmployeeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeId))
            .WithMessage($"EmployeeId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAppsTransactionLogRecord.
/// </summary>
public class UpdateAppsTransactionLogRecordValidator : BaseRecordValidator<UpdateAppsTransactionLogRecord>
{
    public UpdateAppsTransactionLogRecordValidator()
    {
        RuleFor(x => x.TransactionLogId)
            .GreaterThan(0)
            .WithMessage("TransactionLogId must be greater than 0");

        RuleFor(x => x.TransactionType)
            .NotEmpty()
            .WithMessage("TransactionType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"TransactionType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Request)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Request))
            .WithMessage($"Request cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Response)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Response))
            .WithMessage($"Response cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Remarks)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Remarks))
            .WithMessage($"Remarks cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AppsUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AppsUserId))
            .WithMessage($"AppsUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EmployeeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeId))
            .WithMessage($"EmployeeId cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAppsTransactionLogRecord.
/// </summary>
public class DeleteAppsTransactionLogRecordValidator : BaseRecordValidator<DeleteAppsTransactionLogRecord>
{
    public DeleteAppsTransactionLogRecordValidator()
    {
        RuleFor(x => x.TransactionLogId)
            .GreaterThan(0)
            .WithMessage("TransactionLogId must be greater than 0");

    }
}