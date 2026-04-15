using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAuditLogRecord.
/// </summary>
public class CreateAuditLogRecordValidator : BaseRecordValidator<CreateAuditLogRecord>
{
    public CreateAuditLogRecordValidator()
    {
        RuleFor(x => x.Username)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Username))
            .WithMessage($"Username cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IpAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.IpAddress))
            .WithMessage($"IpAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.UserAgent)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UserAgent))
            .WithMessage($"UserAgent cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Action)
            .NotEmpty()
            .WithMessage("Action is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"Action cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EntityType)
            .NotEmpty()
            .WithMessage("EntityType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"EntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EntityId))
            .WithMessage($"EntityId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Endpoint)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Endpoint))
            .WithMessage($"Endpoint cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Module)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Module))
            .WithMessage($"Module cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OldValue)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OldValue))
            .WithMessage($"OldValue cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.NewValue)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.NewValue))
            .WithMessage($"NewValue cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Changes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Changes))
            .WithMessage($"Changes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CorrelationId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CorrelationId))
            .WithMessage($"CorrelationId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.SessionId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SessionId))
            .WithMessage($"SessionId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.RequestId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.RequestId))
            .WithMessage($"RequestId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ErrorMessage)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ErrorMessage))
            .WithMessage($"ErrorMessage cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAuditLogRecord.
/// </summary>
public class UpdateAuditLogRecordValidator : BaseRecordValidator<UpdateAuditLogRecord>
{
    public UpdateAuditLogRecordValidator()
    {
        RuleFor(x => x.AuditId)
            .GreaterThan(0)
            .WithMessage("AuditId must be greater than 0");

        RuleFor(x => x.Username)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Username))
            .WithMessage($"Username cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.IpAddress)
            .MaximumLength(MaxAddressLength)
            .When(x => !string.IsNullOrEmpty(x.IpAddress))
            .WithMessage($"IpAddress cannot exceed {MaxAddressLength} characters");

        RuleFor(x => x.UserAgent)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.UserAgent))
            .WithMessage($"UserAgent cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Action)
            .NotEmpty()
            .WithMessage("Action is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"Action cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EntityType)
            .NotEmpty()
            .WithMessage("EntityType is required")
            .MaximumLength(MaxStringLength)
            .WithMessage($"EntityType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EntityId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EntityId))
            .WithMessage($"EntityId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Endpoint)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Endpoint))
            .WithMessage($"Endpoint cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Module)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Module))
            .WithMessage($"Module cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.OldValue)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OldValue))
            .WithMessage($"OldValue cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.NewValue)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.NewValue))
            .WithMessage($"NewValue cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Changes)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Changes))
            .WithMessage($"Changes cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CorrelationId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.CorrelationId))
            .WithMessage($"CorrelationId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.SessionId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.SessionId))
            .WithMessage($"SessionId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.RequestId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.RequestId))
            .WithMessage($"RequestId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ErrorMessage)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ErrorMessage))
            .WithMessage($"ErrorMessage cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAuditLogRecord.
/// </summary>
public class DeleteAuditLogRecordValidator : BaseRecordValidator<DeleteAuditLogRecord>
{
    public DeleteAuditLogRecordValidator()
    {
        RuleFor(x => x.AuditId)
            .GreaterThan(0)
            .WithMessage("AuditId must be greater than 0");

    }
}