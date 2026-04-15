using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateGroupRecord.
/// </summary>
public class CreateGroupRecordValidator : BaseRecordValidator<CreateGroupRecord>
{
    public CreateGroupRecordValidator()
    {
        RuleFor(x => x.GroupName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.GroupName))
            .WithMessage($"GroupName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for UpdateGroupRecord.
/// </summary>
public class UpdateGroupRecordValidator : BaseRecordValidator<UpdateGroupRecord>
{
    public UpdateGroupRecordValidator()
    {
        RuleFor(x => x.GroupId)
            .GreaterThan(0)
            .WithMessage("GroupId must be greater than 0");

        RuleFor(x => x.GroupName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.GroupName))
            .WithMessage($"GroupName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for DeleteGroupRecord.
/// </summary>
public class DeleteGroupRecordValidator : BaseRecordValidator<DeleteGroupRecord>
{
    public DeleteGroupRecordValidator()
    {
        RuleFor(x => x.GroupId)
            .GreaterThan(0)
            .WithMessage("GroupId must be greater than 0");

    }
}