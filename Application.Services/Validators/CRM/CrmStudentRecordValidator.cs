using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmStudentRecord.</summary>
public class CreateCrmStudentRecordValidator : BaseRecordValidator<CreateCrmStudentRecord>
{
    public CreateCrmStudentRecordValidator()
    {
        RuleFor(x => x.StudentName)
            .NotEmpty().WithMessage("StudentName is required")
            .MaximumLength(MaxNameLength).WithMessage($"StudentName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.StudentCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.StudentCode))
            .WithMessage("StudentCode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmStudentRecord.</summary>
public class UpdateCrmStudentRecordValidator : BaseRecordValidator<UpdateCrmStudentRecord>
{
    public UpdateCrmStudentRecordValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("StudentId must be greater than 0");

        RuleFor(x => x.StudentName)
            .NotEmpty().WithMessage("StudentName is required")
            .MaximumLength(MaxNameLength).WithMessage($"StudentName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.StudentCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.StudentCode))
            .WithMessage("StudentCode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmStudentRecord.</summary>
public class DeleteCrmStudentRecordValidator : BaseRecordValidator<DeleteCrmStudentRecord>
{
    public DeleteCrmStudentRecordValidator()
    {
        RuleFor(x => x.StudentId)
            .GreaterThan(0).WithMessage("StudentId must be greater than 0");
    }
}
