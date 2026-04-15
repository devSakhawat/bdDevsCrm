using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateMaritalStatusRecord.
/// </summary>
public class CreateMaritalStatusRecordValidator : BaseRecordValidator<CreateMaritalStatusRecord>
{
    public CreateMaritalStatusRecordValidator()
    {
        RuleFor(x => x.MaritalStatusName)
            .NotEmpty()
            .WithMessage("Marital status name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Marital status name cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for UpdateMaritalStatusRecord.
/// </summary>
public class UpdateMaritalStatusRecordValidator : BaseRecordValidator<UpdateMaritalStatusRecord>
{
    public UpdateMaritalStatusRecordValidator()
    {
        RuleFor(x => x.MaritalStatusId)
            .GreaterThan(0)
            .WithMessage("Marital status ID must be greater than 0");

        RuleFor(x => x.MaritalStatusName)
            .NotEmpty()
            .WithMessage("Marital status name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Marital status name cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for DeleteMaritalStatusRecord.
/// </summary>
public class DeleteMaritalStatusRecordValidator : BaseRecordValidator<DeleteMaritalStatusRecord>
{
    public DeleteMaritalStatusRecordValidator()
    {
        RuleFor(x => x.MaritalStatusId)
            .GreaterThan(0)
            .WithMessage("Marital status ID must be greater than 0");
    }
}
