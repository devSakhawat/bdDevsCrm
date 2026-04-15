using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmIntakeMonthRecord.
/// </summary>
public class CreateCrmIntakeMonthRecordValidator : BaseRecordValidator<CreateCrmIntakeMonthRecord>
{
    public CreateCrmIntakeMonthRecordValidator()
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

        RuleFor(x => x.MonthNumber)
            .GreaterThan(0)
            .WithMessage("MonthNumber must be greater than 0");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmIntakeMonthRecord.
/// </summary>
public class UpdateCrmIntakeMonthRecordValidator : BaseRecordValidator<UpdateCrmIntakeMonthRecord>
{
    public UpdateCrmIntakeMonthRecordValidator()
    {
        RuleFor(x => x.IntakeMonthId)
            .GreaterThan(0)
            .WithMessage("IntakeMonthId must be greater than 0");

        RuleFor(x => x.MonthName)
            .NotEmpty()
            .WithMessage("MonthName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"MonthName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.MonthCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.MonthCode))
            .WithMessage($"MonthCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.MonthNumber)
            .GreaterThan(0)
            .WithMessage("MonthNumber must be greater than 0");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmIntakeMonthRecord.
/// </summary>
public class DeleteCrmIntakeMonthRecordValidator : BaseRecordValidator<DeleteCrmIntakeMonthRecord>
{
    public DeleteCrmIntakeMonthRecordValidator()
    {
        RuleFor(x => x.IntakeMonthId)
            .GreaterThan(0)
            .WithMessage("IntakeMonthId must be greater than 0");

    }
}
