using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreatePasswordHistoryRecord.
/// </summary>
public class CreatePasswordHistoryRecordValidator : BaseRecordValidator<CreatePasswordHistoryRecord>
{
    public CreatePasswordHistoryRecordValidator()
    {
        RuleFor(x => x.OldPassword)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OldPassword))
            .WithMessage($"OldPassword cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdatePasswordHistoryRecord.
/// </summary>
public class UpdatePasswordHistoryRecordValidator : BaseRecordValidator<UpdatePasswordHistoryRecord>
{
    public UpdatePasswordHistoryRecordValidator()
    {
        RuleFor(x => x.HistoryId)
            .GreaterThan(0)
            .WithMessage("HistoryId must be greater than 0");

        RuleFor(x => x.OldPassword)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.OldPassword))
            .WithMessage($"OldPassword cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeletePasswordHistoryRecord.
/// </summary>
public class DeletePasswordHistoryRecordValidator : BaseRecordValidator<DeletePasswordHistoryRecord>
{
    public DeletePasswordHistoryRecordValidator()
    {
        RuleFor(x => x.HistoryId)
            .GreaterThan(0)
            .WithMessage("HistoryId must be greater than 0");

    }
}