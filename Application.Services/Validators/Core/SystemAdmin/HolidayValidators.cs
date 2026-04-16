using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateHolidayRecord.
/// </summary>
public class CreateHolidayRecordValidator : BaseRecordValidator<CreateHolidayRecord>
{
    public CreateHolidayRecordValidator()
    {
        RuleFor(x => x.HolidayDate)
            .NotNull()
            .WithMessage("Holiday date is required");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MonthName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.MonthName))
            .WithMessage($"Month name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.DayName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.DayName))
            .WithMessage($"Day name cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for UpdateHolidayRecord.
/// </summary>
public class UpdateHolidayRecordValidator : BaseRecordValidator<UpdateHolidayRecord>
{
    public UpdateHolidayRecordValidator()
    {
        RuleFor(x => x.HolidayId)
            .GreaterThan(0)
            .WithMessage("HolidayId must be greater than 0");

        RuleFor(x => x.HolidayDate)
            .NotNull()
            .WithMessage("Holiday date is required");

        RuleFor(x => x.Description)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Description))
            .WithMessage($"Description cannot exceed {MaxStringLength} characters");

        RuleFor(x => x.MonthName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.MonthName))
            .WithMessage($"Month name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.DayName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.DayName))
            .WithMessage($"Day name cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for DeleteHolidayRecord.
/// </summary>
public class DeleteHolidayRecordValidator : BaseRecordValidator<DeleteHolidayRecord>
{
    public DeleteHolidayRecordValidator()
    {
        RuleFor(x => x.HolidayId)
            .GreaterThan(0)
            .WithMessage("HolidayId must be greater than 0");
    }
}
