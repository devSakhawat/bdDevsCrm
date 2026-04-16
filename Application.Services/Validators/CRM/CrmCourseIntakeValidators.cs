using bdDevs.Shared.Records.CRM;
using FluentValidation;
using Application.Services.Validators;

namespace Application.Services.Validators.CRM;

public class CreateCrmCourseIntakeRecordValidator : BaseRecordValidator<CreateCrmCourseIntakeRecord>
{
    public CreateCrmCourseIntakeRecordValidator()
    {
        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID is required");

        RuleFor(x => x.IntakeTitile)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeTitile))
            .WithMessage($"Intake title cannot exceed {MaxNameLength} characters");
    }
}

public class UpdateCrmCourseIntakeRecordValidator : BaseRecordValidator<UpdateCrmCourseIntakeRecord>
{
    public UpdateCrmCourseIntakeRecordValidator()
    {
        RuleFor(x => x.CourseIntakeId)
            .GreaterThan(0)
            .WithMessage("Course Intake ID must be greater than 0");

        RuleFor(x => x.CourseId)
            .GreaterThan(0)
            .WithMessage("Course ID is required");

        RuleFor(x => x.IntakeTitile)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.IntakeTitile))
            .WithMessage($"Intake title cannot exceed {MaxNameLength} characters");
    }
}

public class DeleteCrmCourseIntakeRecordValidator : BaseRecordValidator<DeleteCrmCourseIntakeRecord>
{
    public DeleteCrmCourseIntakeRecordValidator()
    {
        RuleFor(x => x.CourseIntakeId)
            .GreaterThan(0)
            .WithMessage("Course Intake ID must be greater than 0");
    }
}
