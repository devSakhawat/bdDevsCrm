using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

/// <summary>
/// Validator for CreateCrmInstituteTypeRecord.
/// </summary>
public class CreateCrmInstituteTypeRecordValidator : BaseRecordValidator<CreateCrmInstituteTypeRecord>
{
    public CreateCrmInstituteTypeRecordValidator()
    {
        RuleFor(x => x.InstituteTypeName)
            .NotEmpty()
            .WithMessage("InstituteTypeName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"InstituteTypeName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for UpdateCrmInstituteTypeRecord.
/// </summary>
public class UpdateCrmInstituteTypeRecordValidator : BaseRecordValidator<UpdateCrmInstituteTypeRecord>
{
    public UpdateCrmInstituteTypeRecordValidator()
    {
        RuleFor(x => x.InstituteTypeId)
            .GreaterThan(0)
            .WithMessage("InstituteTypeId must be greater than 0");

        RuleFor(x => x.InstituteTypeName)
            .NotEmpty()
            .WithMessage("InstituteTypeName is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"InstituteTypeName cannot exceed {MaxNameLength} characters");

    }
}

/// <summary>
/// Validator for DeleteCrmInstituteTypeRecord.
/// </summary>
public class DeleteCrmInstituteTypeRecordValidator : BaseRecordValidator<DeleteCrmInstituteTypeRecord>
{
    public DeleteCrmInstituteTypeRecordValidator()
    {
        RuleFor(x => x.InstituteTypeId)
            .GreaterThan(0)
            .WithMessage("InstituteTypeId must be greater than 0");

    }
}
