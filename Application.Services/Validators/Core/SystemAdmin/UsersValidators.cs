using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateUsersRecord.
/// </summary>
public class CreateUsersRecordValidator : BaseRecordValidator<CreateUsersRecord>
{
    public CreateUsersRecordValidator()
    {
        RuleFor(x => x.LoginId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LoginId))
            .WithMessage($"LoginId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UserName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.UserName))
            .WithMessage($"UserName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Password)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Password))
            .WithMessage($"Password cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Theme)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Theme))
            .WithMessage($"Theme cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateUsersRecord.
/// </summary>
public class UpdateUsersRecordValidator : BaseRecordValidator<UpdateUsersRecord>
{
    public UpdateUsersRecordValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

        RuleFor(x => x.LoginId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.LoginId))
            .WithMessage($"LoginId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.UserName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.UserName))
            .WithMessage($"UserName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.Password)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Password))
            .WithMessage($"Password cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.Theme)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Theme))
            .WithMessage($"Theme cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteUsersRecord.
/// </summary>
public class DeleteUsersRecordValidator : BaseRecordValidator<DeleteUsersRecord>
{
    public DeleteUsersRecordValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

    }
}