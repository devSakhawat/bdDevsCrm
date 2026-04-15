using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmYearRecord.
/// </summary>
public class CreateCrmYearRecordValidator : BaseRecordValidator<CreateCrmYearRecord>
{
    public CreateCrmYearRecordValidator()
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

    }
}

/// <summary>
/// Validator for UpdateCrmYearRecord.
/// </summary>
public class UpdateCrmYearRecordValidator : BaseRecordValidator<UpdateCrmYearRecord>
{
    public UpdateCrmYearRecordValidator()
    {
        RuleFor(x => x.YearId)
            .GreaterThan(0)
            .WithMessage("YearId must be greater than 0");

        RuleFor(x => x.YearName)
            .NotEmpty()
            .WithMessage("YearName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"YearName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.YearCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.YearCode))
            .WithMessage($"YearCode cannot exceed {MaxCodeLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCrmYearRecord.
/// </summary>
public class DeleteCrmYearRecordValidator : BaseRecordValidator<DeleteCrmYearRecord>
{
    public DeleteCrmYearRecordValidator()
    {
        RuleFor(x => x.YearId)
            .GreaterThan(0)
            .WithMessage("YearId must be greater than 0");

    }
}
