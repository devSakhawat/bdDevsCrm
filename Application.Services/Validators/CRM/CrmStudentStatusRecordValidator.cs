using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmStudentStatusRecord.</summary>
public class CreateCrmStudentStatusRecordValidator : BaseRecordValidator<CreateCrmStudentStatusRecord>
{
    public CreateCrmStudentStatusRecordValidator()
    {
        RuleFor(x => x.StatusName)
            .NotEmpty().WithMessage("StatusName is required")
            .MaximumLength(MaxNameLength).WithMessage($"StatusName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.StatusCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.StatusCode))
            .WithMessage($"StatusCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.ColorCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ColorCode))
            .WithMessage($"ColorCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmStudentStatusRecord.</summary>
public class UpdateCrmStudentStatusRecordValidator : BaseRecordValidator<UpdateCrmStudentStatusRecord>
{
    public UpdateCrmStudentStatusRecordValidator()
    {
        RuleFor(x => x.StudentStatusId)
            .GreaterThan(0).WithMessage("StudentStatusId must be greater than 0");

        RuleFor(x => x.StatusName)
            .NotEmpty().WithMessage("StatusName is required")
            .MaximumLength(MaxNameLength).WithMessage($"StatusName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.StatusCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.StatusCode))
            .WithMessage($"StatusCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.ColorCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ColorCode))
            .WithMessage($"ColorCode cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmStudentStatusRecord.</summary>
public class DeleteCrmStudentStatusRecordValidator : BaseRecordValidator<DeleteCrmStudentStatusRecord>
{
    public DeleteCrmStudentStatusRecordValidator()
    {
        RuleFor(x => x.StudentStatusId)
            .GreaterThan(0).WithMessage("StudentStatusId must be greater than 0");
    }
}
