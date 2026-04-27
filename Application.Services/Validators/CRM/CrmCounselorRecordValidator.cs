using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>Validator for CreateCrmCounselorRecord.</summary>
public class CreateCrmCounselorRecordValidator : BaseRecordValidator<CreateCrmCounselorRecord>
{
    public CreateCrmCounselorRecordValidator()
    {
        RuleFor(x => x.CounselorName)
            .NotEmpty().WithMessage("CounselorName is required")
            .MaximumLength(MaxNameLength).WithMessage($"CounselorName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.CounselorCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.CounselorCode))
            .WithMessage("CounselorCode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for UpdateCrmCounselorRecord.</summary>
public class UpdateCrmCounselorRecordValidator : BaseRecordValidator<UpdateCrmCounselorRecord>
{
    public UpdateCrmCounselorRecordValidator()
    {
        RuleFor(x => x.CounselorId)
            .GreaterThan(0).WithMessage("CounselorId must be greater than 0");

        RuleFor(x => x.CounselorName)
            .NotEmpty().WithMessage("CounselorName is required")
            .MaximumLength(MaxNameLength).WithMessage($"CounselorName cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.CounselorCode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.CounselorCode))
            .WithMessage("CounselorCode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0).WithMessage("CreatedBy must be greater than 0");
    }
}

/// <summary>Validator for DeleteCrmCounselorRecord.</summary>
public class DeleteCrmCounselorRecordValidator : BaseRecordValidator<DeleteCrmCounselorRecord>
{
    public DeleteCrmCounselorRecordValidator()
    {
        RuleFor(x => x.CounselorId)
            .GreaterThan(0).WithMessage("CounselorId must be greater than 0");
    }
}
