using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAuditTrailRecord.
/// </summary>
public class CreateAuditTrailRecordValidator : BaseRecordValidator<CreateAuditTrailRecord>
{
    public CreateAuditTrailRecordValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

        RuleFor(x => x.ClientUser)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ClientUser))
            .WithMessage($"ClientUser cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ClientIp)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ClientIp))
            .WithMessage($"ClientIp cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Shortdescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Shortdescription))
            .WithMessage($"Shortdescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AuditType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditType))
            .WithMessage($"AuditType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AuditDescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditDescription))
            .WithMessage($"AuditDescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.RequestedUrl)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.RequestedUrl))
            .WithMessage($"RequestedUrl cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AuditStatus)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditStatus))
            .WithMessage($"AuditStatus cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAuditTrailRecord.
/// </summary>
public class UpdateAuditTrailRecordValidator : BaseRecordValidator<UpdateAuditTrailRecord>
{
    public UpdateAuditTrailRecordValidator()
    {
        RuleFor(x => x.AuditId)
            .GreaterThan(0)
            .WithMessage("AuditId must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

        RuleFor(x => x.ClientUser)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ClientUser))
            .WithMessage($"ClientUser cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.ClientIp)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.ClientIp))
            .WithMessage($"ClientIp cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Shortdescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Shortdescription))
            .WithMessage($"Shortdescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AuditType)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditType))
            .WithMessage($"AuditType cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AuditDescription)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditDescription))
            .WithMessage($"AuditDescription cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.RequestedUrl)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.RequestedUrl))
            .WithMessage($"RequestedUrl cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.AuditStatus)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditStatus))
            .WithMessage($"AuditStatus cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAuditTrailRecord.
/// </summary>
public class DeleteAuditTrailRecordValidator : BaseRecordValidator<DeleteAuditTrailRecord>
{
    public DeleteAuditTrailRecordValidator()
    {
        RuleFor(x => x.AuditId)
            .GreaterThan(0)
            .WithMessage("AuditId must be greater than 0");

    }
}