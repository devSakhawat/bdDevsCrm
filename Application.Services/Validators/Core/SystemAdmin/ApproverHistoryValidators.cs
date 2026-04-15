using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateApproverHistoryRecord.
/// </summary>
public class CreateApproverHistoryRecordValidator : BaseRecordValidator<CreateApproverHistoryRecord>
{
    public CreateApproverHistoryRecordValidator()
    {
        RuleFor(x => x.ApproverId)
            .GreaterThan(0)
            .WithMessage("ApproverId must be greater than 0");

        RuleFor(x => x.HrRecordId)
            .GreaterThan(0)
            .WithMessage("HrRecordId must be greater than 0");

        RuleFor(x => x.ModuleId)
            .GreaterThan(0)
            .WithMessage("ModuleId must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateApproverHistoryRecord.
/// </summary>
public class UpdateApproverHistoryRecordValidator : BaseRecordValidator<UpdateApproverHistoryRecord>
{
    public UpdateApproverHistoryRecordValidator()
    {
        RuleFor(x => x.AssignApproverId)
            .GreaterThan(0)
            .WithMessage("AssignApproverId must be greater than 0");

        RuleFor(x => x.ApproverId)
            .GreaterThan(0)
            .WithMessage("ApproverId must be greater than 0");

        RuleFor(x => x.HrRecordId)
            .GreaterThan(0)
            .WithMessage("HrRecordId must be greater than 0");

        RuleFor(x => x.ModuleId)
            .GreaterThan(0)
            .WithMessage("ModuleId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteApproverHistoryRecord.
/// </summary>
public class DeleteApproverHistoryRecordValidator : BaseRecordValidator<DeleteApproverHistoryRecord>
{
    public DeleteApproverHistoryRecordValidator()
    {
        RuleFor(x => x.AssignApproverId)
            .GreaterThan(0)
            .WithMessage("AssignApproverId must be greater than 0");

    }
}