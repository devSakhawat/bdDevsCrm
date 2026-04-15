using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCompanyLocationMapRecord.
/// </summary>
public class CreateCompanyLocationMapRecordValidator : BaseRecordValidator<CreateCompanyLocationMapRecord>
{
    public CreateCompanyLocationMapRecordValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .WithMessage("BranchId must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCompanyLocationMapRecord.
/// </summary>
public class UpdateCompanyLocationMapRecordValidator : BaseRecordValidator<UpdateCompanyLocationMapRecord>
{
    public UpdateCompanyLocationMapRecordValidator()
    {
        RuleFor(x => x.SbuLocationMapId)
            .GreaterThan(0)
            .WithMessage("SbuLocationMapId must be greater than 0");

        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.BranchId)
            .GreaterThan(0)
            .WithMessage("BranchId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCompanyLocationMapRecord.
/// </summary>
public class DeleteCompanyLocationMapRecordValidator : BaseRecordValidator<DeleteCompanyLocationMapRecord>
{
    public DeleteCompanyLocationMapRecordValidator()
    {
        RuleFor(x => x.SbuLocationMapId)
            .GreaterThan(0)
            .WithMessage("SbuLocationMapId must be greater than 0");

    }
}