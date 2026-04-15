using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmMonthRecord.
/// </summary>
public class CreateCrmMonthRecordValidator : BaseRecordValidator<CreateCrmMonthRecord>
{
    public CreateCrmMonthRecordValidator()
    {
        RuleFor(x => x.MonthName)
            .NotEmpty()
            .WithMessage("MonthName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"MonthName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.MonthCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.MonthCode))
            .WithMessage($"MonthCode cannot exceed {MaxCodeLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCrmMonthRecord.
/// </summary>
public class UpdateCrmMonthRecordValidator : BaseRecordValidator<UpdateCrmMonthRecord>
{
    public UpdateCrmMonthRecordValidator()
    {
        RuleFor(x => x.MonthId)
            .GreaterThan(0)
            .WithMessage("MonthId must be greater than 0");

        RuleFor(x => x.MonthName)
            .NotEmpty()
            .WithMessage("MonthName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"MonthName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.MonthCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.MonthCode))
            .WithMessage($"MonthCode cannot exceed {MaxCodeLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCrmMonthRecord.
/// </summary>
public class DeleteCrmMonthRecordValidator : BaseRecordValidator<DeleteCrmMonthRecord>
{
    public DeleteCrmMonthRecordValidator()
    {
        RuleFor(x => x.MonthId)
            .GreaterThan(0)
            .WithMessage("MonthId must be greater than 0");

    }
}
