using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateApproverOrderRecord.
/// </summary>
public class CreateApproverOrderRecordValidator : BaseRecordValidator<CreateApproverOrderRecord>
{
    public CreateApproverOrderRecordValidator()
    {
        RuleFor(x => x.OrderTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OrderTitle))
            .WithMessage($"OrderTitle cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateApproverOrderRecord.
/// </summary>
public class UpdateApproverOrderRecordValidator : BaseRecordValidator<UpdateApproverOrderRecord>
{
    public UpdateApproverOrderRecordValidator()
    {
        RuleFor(x => x.ApproverOrderId)
            .GreaterThan(0)
            .WithMessage("ApproverOrderId must be greater than 0");

        RuleFor(x => x.OrderTitle)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OrderTitle))
            .WithMessage($"OrderTitle cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteApproverOrderRecord.
/// </summary>
public class DeleteApproverOrderRecordValidator : BaseRecordValidator<DeleteApproverOrderRecord>
{
    public DeleteApproverOrderRecordValidator()
    {
        RuleFor(x => x.ApproverOrderId)
            .GreaterThan(0)
            .WithMessage("ApproverOrderId must be greater than 0");

    }
}