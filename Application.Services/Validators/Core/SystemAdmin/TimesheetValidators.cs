using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

public class CreateTimesheetRecordValidator : BaseRecordValidator<CreateTimesheetRecord>
{
    public CreateTimesheetRecordValidator()
    {
        RuleFor(x => x.WorkingLogDate)
            .NotNull()
            .WithMessage("Working log date is required");

        RuleFor(x => x.WorkedLogHour)
            .GreaterThan(0)
            .When(x => x.WorkedLogHour.HasValue)
            .WithMessage("Worked log hour must be greater than 0");
    }
}

public class UpdateTimesheetRecordValidator : BaseRecordValidator<UpdateTimesheetRecord>
{
    public UpdateTimesheetRecordValidator()
    {
        RuleFor(x => x.Timesheetid)
            .GreaterThan(0)
            .WithMessage("Timesheet ID must be greater than 0");

        RuleFor(x => x.WorkingLogDate)
            .NotNull()
            .WithMessage("Working log date is required");

        RuleFor(x => x.WorkedLogHour)
            .GreaterThan(0)
            .When(x => x.WorkedLogHour.HasValue)
            .WithMessage("Worked log hour must be greater than 0");
    }
}

public class DeleteTimesheetRecordValidator : BaseRecordValidator<DeleteTimesheetRecord>
{
    public DeleteTimesheetRecordValidator()
    {
        RuleFor(x => x.Timesheetid)
            .GreaterThan(0)
            .WithMessage("Timesheet ID must be greater than 0");
    }
}
