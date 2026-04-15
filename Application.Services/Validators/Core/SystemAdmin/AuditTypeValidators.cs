using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAuditTypeRecord.
/// </summary>
public class CreateAuditTypeRecordValidator : BaseRecordValidator<CreateAuditTypeRecord>
{
    public CreateAuditTypeRecordValidator()
    {
        RuleFor(x => x.AuditType1)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditType1))
            .WithMessage($"AuditType1 cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAuditTypeRecord.
/// </summary>
public class UpdateAuditTypeRecordValidator : BaseRecordValidator<UpdateAuditTypeRecord>
{
    public UpdateAuditTypeRecordValidator()
    {
        RuleFor(x => x.AuditTypeId)
            .GreaterThan(0)
            .WithMessage("AuditTypeId must be greater than 0");

        RuleFor(x => x.AuditType1)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AuditType1))
            .WithMessage($"AuditType1 cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAuditTypeRecord.
/// </summary>
public class DeleteAuditTypeRecordValidator : BaseRecordValidator<DeleteAuditTypeRecord>
{
    public DeleteAuditTypeRecordValidator()
    {
        RuleFor(x => x.AuditTypeId)
            .GreaterThan(0)
            .WithMessage("AuditTypeId must be greater than 0");

    }
}