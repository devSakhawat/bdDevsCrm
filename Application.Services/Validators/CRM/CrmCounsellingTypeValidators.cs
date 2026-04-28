using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

public class CreateCrmCounsellingTypeRecordValidator : BaseRecordValidator<CreateCrmCounsellingTypeRecord>
{
    public CreateCrmCounsellingTypeRecordValidator()
    {
        RuleFor(x => x.CounsellingTypeName)
            .NotEmpty()
            .WithMessage("CounsellingTypeName is required")
            .MaximumLength(100)
            .WithMessage("CounsellingTypeName cannot exceed 100 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class UpdateCrmCounsellingTypeRecordValidator : BaseRecordValidator<UpdateCrmCounsellingTypeRecord>
{
    public UpdateCrmCounsellingTypeRecordValidator()
    {
        RuleFor(x => x.CounsellingTypeId)
            .GreaterThan(0)
            .WithMessage("CounsellingTypeId must be greater than 0");

        RuleFor(x => x.CounsellingTypeName)
            .NotEmpty()
            .WithMessage("CounsellingTypeName is required")
            .MaximumLength(100)
            .WithMessage("CounsellingTypeName cannot exceed 100 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class DeleteCrmCounsellingTypeRecordValidator : BaseRecordValidator<DeleteCrmCounsellingTypeRecord>
{
    public DeleteCrmCounsellingTypeRecordValidator()
    {
        RuleFor(x => x.CounsellingTypeId)
            .GreaterThan(0)
            .WithMessage("CounsellingTypeId must be greater than 0");
    }
}
