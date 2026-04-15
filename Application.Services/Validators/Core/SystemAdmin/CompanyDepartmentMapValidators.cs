using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateCompanyDepartmentMapRecord.
/// </summary>
public class CreateCompanyDepartmentMapRecordValidator : BaseRecordValidator<CreateCompanyDepartmentMapRecord>
{
    public CreateCompanyDepartmentMapRecordValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("DepartmentId must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateCompanyDepartmentMapRecord.
/// </summary>
public class UpdateCompanyDepartmentMapRecordValidator : BaseRecordValidator<UpdateCompanyDepartmentMapRecord>
{
    public UpdateCompanyDepartmentMapRecordValidator()
    {
        RuleFor(x => x.SbuDepartmentMapId)
            .GreaterThan(0)
            .WithMessage("SbuDepartmentMapId must be greater than 0");

        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.DepartmentId)
            .GreaterThan(0)
            .WithMessage("DepartmentId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteCompanyDepartmentMapRecord.
/// </summary>
public class DeleteCompanyDepartmentMapRecordValidator : BaseRecordValidator<DeleteCompanyDepartmentMapRecord>
{
    public DeleteCompanyDepartmentMapRecordValidator()
    {
        RuleFor(x => x.SbuDepartmentMapId)
            .GreaterThan(0)
            .WithMessage("SbuDepartmentMapId must be greater than 0");

    }
}