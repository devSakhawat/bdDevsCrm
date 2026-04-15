using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateEmployeetypeRecord.
/// </summary>
public class CreateEmployeetypeRecordValidator : BaseRecordValidator<CreateEmployeetypeRecord>
{
    public CreateEmployeetypeRecordValidator()
    {
        RuleFor(x => x.Employeetypename)
            .NotEmpty()
            .WithMessage("Employee type name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Employee type name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.EmployeeTypeCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeTypeCode))
            .WithMessage($"Employee type code cannot exceed {MaxCodeLength} characters");
    }
}

/// <summary>
/// Validator for UpdateEmployeetypeRecord.
/// </summary>
public class UpdateEmployeetypeRecordValidator : BaseRecordValidator<UpdateEmployeetypeRecord>
{
    public UpdateEmployeetypeRecordValidator()
    {
        RuleFor(x => x.Employeetypeid)
            .GreaterThan(0)
            .WithMessage("Employee type ID must be greater than 0");

        RuleFor(x => x.Employeetypename)
            .NotEmpty()
            .WithMessage("Employee type name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Employee type name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.EmployeeTypeCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.EmployeeTypeCode))
            .WithMessage($"Employee type code cannot exceed {MaxCodeLength} characters");
    }
}

/// <summary>
/// Validator for DeleteEmployeetypeRecord.
/// </summary>
public class DeleteEmployeetypeRecordValidator : BaseRecordValidator<DeleteEmployeetypeRecord>
{
    public DeleteEmployeetypeRecordValidator()
    {
        RuleFor(x => x.Employeetypeid)
            .GreaterThan(0)
            .WithMessage("Employee type ID must be greater than 0");
    }
}
