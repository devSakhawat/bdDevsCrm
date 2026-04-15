using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmApplicationRecord.
/// </summary>
public class CreateCrmApplicationRecordValidator : BaseRecordValidator<CreateCrmApplicationRecord>
{
    public CreateCrmApplicationRecordValidator()
    {
        RuleFor(x => x.StateId)
            .GreaterThan(0)
            .WithMessage("StateId must be greater than 0");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCrmApplicationRecord.
/// </summary>
public class UpdateCrmApplicationRecordValidator : BaseRecordValidator<UpdateCrmApplicationRecord>
{
    public UpdateCrmApplicationRecordValidator()
    {
        RuleFor(x => x.ApplicationId)
            .GreaterThan(0)
            .WithMessage("ApplicationId must be greater than 0");

        RuleFor(x => x.StateId)
            .GreaterThan(0)
            .WithMessage("StateId must be greater than 0");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCrmApplicationRecord.
/// </summary>
public class DeleteCrmApplicationRecordValidator : BaseRecordValidator<DeleteCrmApplicationRecord>
{
    public DeleteCrmApplicationRecordValidator()
    {
        RuleFor(x => x.ApplicationId)
            .GreaterThan(0)
            .WithMessage("ApplicationId must be greater than 0");

    }
}
