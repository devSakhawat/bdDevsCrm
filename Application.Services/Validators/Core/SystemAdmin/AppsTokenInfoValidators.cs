using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAppsTokenInfoRecord.
/// </summary>
public class CreateAppsTokenInfoRecordValidator : BaseRecordValidator<CreateAppsTokenInfoRecord>
{
    public CreateAppsTokenInfoRecordValidator()
    {
        RuleFor(x => x.AppsUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AppsUserId))
            .WithMessage($"AppsUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EmployeeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeId))
            .WithMessage($"EmployeeId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TokenNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TokenNumber))
            .WithMessage($"TokenNumber cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateAppsTokenInfoRecord.
/// </summary>
public class UpdateAppsTokenInfoRecordValidator : BaseRecordValidator<UpdateAppsTokenInfoRecord>
{
    public UpdateAppsTokenInfoRecordValidator()
    {
        RuleFor(x => x.AppsTokenInfoId)
            .GreaterThan(0)
            .WithMessage("AppsTokenInfoId must be greater than 0");

        RuleFor(x => x.AppsUserId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.AppsUserId))
            .WithMessage($"AppsUserId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.EmployeeId)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeId))
            .WithMessage($"EmployeeId cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.TokenNumber)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.TokenNumber))
            .WithMessage($"TokenNumber cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteAppsTokenInfoRecord.
/// </summary>
public class DeleteAppsTokenInfoRecordValidator : BaseRecordValidator<DeleteAppsTokenInfoRecord>
{
    public DeleteAppsTokenInfoRecordValidator()
    {
        RuleFor(x => x.AppsTokenInfoId)
            .GreaterThan(0)
            .WithMessage("AppsTokenInfoId must be greater than 0");

    }
}