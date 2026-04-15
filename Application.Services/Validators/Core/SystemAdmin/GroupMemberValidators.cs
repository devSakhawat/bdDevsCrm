using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateGroupMemberRecord.
/// </summary>
public class CreateGroupMemberRecordValidator : BaseRecordValidator<CreateGroupMemberRecord>
{
    public CreateGroupMemberRecordValidator()
    {
        RuleFor(x => x.GroupId)
            .GreaterThan(0)
            .WithMessage("GroupId must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

        RuleFor(x => x.GroupOption)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GroupOption))
            .WithMessage($"GroupOption cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateGroupMemberRecord.
/// </summary>
public class UpdateGroupMemberRecordValidator : BaseRecordValidator<UpdateGroupMemberRecord>
{
    public UpdateGroupMemberRecordValidator()
    {
        RuleFor(x => x.GroupId)
            .GreaterThan(0)
            .WithMessage("GroupId must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

        RuleFor(x => x.GroupOption)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.GroupOption))
            .WithMessage($"GroupOption cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteGroupMemberRecord.
/// </summary>
public class DeleteGroupMemberRecordValidator : BaseRecordValidator<DeleteGroupMemberRecord>
{
    public DeleteGroupMemberRecordValidator()
    {
        RuleFor(x => x.GroupId)
            .GreaterThan(0)
            .WithMessage("GroupId must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

    }
}