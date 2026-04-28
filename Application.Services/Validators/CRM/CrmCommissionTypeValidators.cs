using bdDevs.Shared.Records.CRM;
using FluentValidation;

namespace Application.Services.Validators.CRM;

public class CreateCrmCommissionTypeRecordValidator : BaseRecordValidator<CreateCrmCommissionTypeRecord>
{
    public CreateCrmCommissionTypeRecordValidator()
    {
        RuleFor(x => x.CommissionTypeName)
            .NotEmpty()
            .WithMessage("CommissionTypeName is required")
            .MaximumLength(100)
            .WithMessage("CommissionTypeName cannot exceed 100 characters");

        RuleFor(x => x.CalculationMode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.CalculationMode))
            .WithMessage("CalculationMode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class UpdateCrmCommissionTypeRecordValidator : BaseRecordValidator<UpdateCrmCommissionTypeRecord>
{
    public UpdateCrmCommissionTypeRecordValidator()
    {
        RuleFor(x => x.CommissionTypeId)
            .GreaterThan(0)
            .WithMessage("CommissionTypeId must be greater than 0");

        RuleFor(x => x.CommissionTypeName)
            .NotEmpty()
            .WithMessage("CommissionTypeName is required")
            .MaximumLength(100)
            .WithMessage("CommissionTypeName cannot exceed 100 characters");

        RuleFor(x => x.CalculationMode)
            .MaximumLength(50)
            .When(x => !string.IsNullOrEmpty(x.CalculationMode))
            .WithMessage("CalculationMode cannot exceed 50 characters");

        RuleFor(x => x.CreatedBy)
            .GreaterThan(0)
            .WithMessage("CreatedBy must be greater than 0");
    }
}

public class DeleteCrmCommissionTypeRecordValidator : BaseRecordValidator<DeleteCrmCommissionTypeRecord>
{
    public DeleteCrmCommissionTypeRecordValidator()
    {
        RuleFor(x => x.CommissionTypeId)
            .GreaterThan(0)
            .WithMessage("CommissionTypeId must be greater than 0");
    }
}
