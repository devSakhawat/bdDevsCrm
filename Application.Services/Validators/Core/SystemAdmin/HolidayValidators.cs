using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateHolidayRecord.
/// Validates business rules for creating new holidays.
/// </summary>
public class CreateHolidayRecordValidator : BaseRecordValidator<CreateHolidayRecord>
{
    public CreateHolidayRecordValidator()
    {
        RuleFor(x => x.HolidayDate)
            .NotNull()
            .WithMessage("Holiday date is required");

        RuleFor(x => x.HolidayType)
            .NotNull()
            .WithMessage("Holiday type is required")
            .GreaterThan(0)
            .WithMessage("Holiday type must be greater than 0");

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

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .When(x => x.Month.HasValue)
            .WithMessage("Month must be between 1 and 12");

        RuleFor(x => x.YearName)
            .InclusiveBetween(1900, 2100)
            .When(x => x.YearName.HasValue)
            .WithMessage("Year must be between 1900 and 2100");
    }
}

/// <summary>
/// Validator for UpdateHolidayRecord.
/// Validates business rules for updating existing holidays.
/// </summary>
public class UpdateHolidayRecordValidator : BaseRecordValidator<UpdateHolidayRecord>
{
    public UpdateHolidayRecordValidator()
    {
        RuleFor(x => x.HolidayId)
            .GreaterThan(0)
            .WithMessage("Holiday ID must be greater than 0");

        RuleFor(x => x.HolidayDate)
            .NotNull()
            .WithMessage("Holiday date is required");

        RuleFor(x => x.HolidayType)
            .NotNull()
            .WithMessage("Holiday type is required")
            .GreaterThan(0)
            .WithMessage("Holiday type must be greater than 0");

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

        RuleFor(x => x.Month)
            .InclusiveBetween(1, 12)
            .When(x => x.Month.HasValue)
            .WithMessage("Month must be between 1 and 12");

        RuleFor(x => x.YearName)
            .InclusiveBetween(1900, 2100)
            .When(x => x.YearName.HasValue)
            .WithMessage("Year must be between 1900 and 2100");
    }
}

/// <summary>
/// Validator for DeleteHolidayRecord.
/// Validates business rules for deleting holidays.
/// </summary>
public class DeleteHolidayRecordValidator : BaseRecordValidator<DeleteHolidayRecord>
{
    public DeleteHolidayRecordValidator()
    {
        RuleFor(x => x.HolidayId)
            .GreaterThan(0)
            .WithMessage("Holiday ID must be greater than 0");
    }
}
