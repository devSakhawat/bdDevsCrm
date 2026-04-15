using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmCourseIntakeRecord.
/// </summary>
public class CreateCrmCourseIntakeRecordValidator : BaseRecordValidator<CreateCrmCourseIntakeRecord>
{
    public CreateCrmCourseIntakeRecordValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("CourseId must be greater than 0");

        RuleFor(x => x.IntakeTitile)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeTitile))
            .WithMessage($"IntakeTitile cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCrmCourseIntakeRecord.
/// </summary>
public class UpdateCrmCourseIntakeRecordValidator : BaseRecordValidator<UpdateCrmCourseIntakeRecord>
{
    public UpdateCrmCourseIntakeRecordValidator()
    {
        RuleFor(x => x.CourseIntakeId)
            .GreaterThan(0)
            .WithMessage("CourseIntakeId must be greater than 0");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("CourseId must be greater than 0");

        RuleFor(x => x.IntakeTitile)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeTitile))
            .WithMessage($"IntakeTitile cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCrmCourseIntakeRecord.
/// </summary>
public class DeleteCrmCourseIntakeRecordValidator : BaseRecordValidator<DeleteCrmCourseIntakeRecord>
{
    public DeleteCrmCourseIntakeRecordValidator()
    {
        RuleFor(x => x.CourseIntakeId)
            .GreaterThan(0)
            .WithMessage("CourseIntakeId must be greater than 0");

    }
}
