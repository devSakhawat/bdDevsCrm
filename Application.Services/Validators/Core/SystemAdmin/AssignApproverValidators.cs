using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAssignApproverRecord.
/// </summary>
public class CreateAssignApproverRecordValidator : BaseRecordValidator<CreateAssignApproverRecord>
{
    public CreateAssignApproverRecordValidator()
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
/// Validator for UpdateAssignApproverRecord.
/// </summary>
public class UpdateAssignApproverRecordValidator : BaseRecordValidator<UpdateAssignApproverRecord>
{
    public UpdateAssignApproverRecordValidator()
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
/// Validator for DeleteAssignApproverRecord.
/// </summary>
public class DeleteAssignApproverRecordValidator : BaseRecordValidator<DeleteAssignApproverRecord>
{
    public DeleteAssignApproverRecordValidator()
    {
        RuleFor(x => x.AssignApproverId)
            .GreaterThan(0)
            .WithMessage("AssignApproverId must be greater than 0");

    }
}