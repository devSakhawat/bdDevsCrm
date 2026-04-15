using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateApproverTypeRecord.
/// </summary>
public class CreateApproverTypeRecordValidator : BaseRecordValidator<CreateApproverTypeRecord>
{
    public CreateApproverTypeRecordValidator()
    {
        RuleFor(x => x.ApproverTypeName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ApproverTypeName))
            .WithMessage($"ApproverTypeName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for UpdateApproverTypeRecord.
/// </summary>
public class UpdateApproverTypeRecordValidator : BaseRecordValidator<UpdateApproverTypeRecord>
{
    public UpdateApproverTypeRecordValidator()
    {
        RuleFor(x => x.ApproverTypeId)
            .GreaterThan(0)
            .WithMessage("ApproverTypeId must be greater than 0");

        RuleFor(x => x.ApproverTypeName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ApproverTypeName))
            .WithMessage($"ApproverTypeName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for DeleteApproverTypeRecord.
/// </summary>
public class DeleteApproverTypeRecordValidator : BaseRecordValidator<DeleteApproverTypeRecord>
{
    public DeleteApproverTypeRecordValidator()
    {
        RuleFor(x => x.ApproverTypeId)
            .GreaterThan(0)
            .WithMessage("ApproverTypeId must be greater than 0");

    }
}