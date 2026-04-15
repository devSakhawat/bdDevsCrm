using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmIntakeYearRecord.
/// </summary>
public class CreateCrmIntakeYearRecordValidator : BaseRecordValidator<CreateCrmIntakeYearRecord>
{
    public CreateCrmIntakeYearRecordValidator()
    {
        RuleFor(x => x.YearName)
            .NotEmpty()
            .WithMessage("YearName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"YearName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.YearCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.YearCode))
            .WithMessage($"YearCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.YearValue)
            .GreaterThan(0)
            .WithMessage("YearValue must be greater than 0");

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
/// Validator for UpdateCrmIntakeYearRecord.
/// </summary>
public class UpdateCrmIntakeYearRecordValidator : BaseRecordValidator<UpdateCrmIntakeYearRecord>
{
    public UpdateCrmIntakeYearRecordValidator()
    {
        RuleFor(x => x.IntakeYearId)
            .GreaterThan(0)
            .WithMessage("IntakeYearId must be greater than 0");

        RuleFor(x => x.YearName)
            .NotEmpty()
            .WithMessage("YearName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"YearName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.YearCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.YearCode))
            .WithMessage($"YearCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.YearValue)
            .GreaterThan(0)
            .WithMessage("YearValue must be greater than 0");

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
/// Validator for DeleteCrmIntakeYearRecord.
/// </summary>
public class DeleteCrmIntakeYearRecordValidator : BaseRecordValidator<DeleteCrmIntakeYearRecord>
{
    public DeleteCrmIntakeYearRecordValidator()
    {
        RuleFor(x => x.IntakeYearId)
            .GreaterThan(0)
            .WithMessage("IntakeYearId must be greater than 0");

    }
}
