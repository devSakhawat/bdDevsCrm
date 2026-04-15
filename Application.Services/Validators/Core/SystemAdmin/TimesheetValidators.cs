using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateTimesheetRecord.
/// </summary>
public class CreateTimesheetRecordValidator : BaseRecordValidator<CreateTimesheetRecord>
{
    public CreateTimesheetRecordValidator()
    {
    }
}

/// <summary>
/// Validator for UpdateTimesheetRecord.
/// </summary>
public class UpdateTimesheetRecordValidator : BaseRecordValidator<UpdateTimesheetRecord>
{
    public UpdateTimesheetRecordValidator()
    {
    }
}

/// <summary>
/// Validator for DeleteTimesheetRecord.
/// </summary>
public class DeleteTimesheetRecordValidator : BaseRecordValidator<DeleteTimesheetRecord>
{
    public DeleteTimesheetRecordValidator()
    {
        RuleFor(x => x.Timesheetid)
            .GreaterThan(0)
            .WithMessage("Timesheetid must be greater than 0");

    }
}