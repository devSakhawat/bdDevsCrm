using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateAccessRestrictionRecord.
/// </summary>
public class CreateAccessRestrictionRecordValidator : BaseRecordValidator<CreateAccessRestrictionRecord>
{
    public CreateAccessRestrictionRecordValidator()
    {
        RuleFor(x => x.HrRecordId)
            .GreaterThan(0)
            .WithMessage("HrRecordId must be greater than 0");

        RuleFor(x => x.ReferenceId)
            .GreaterThan(0)
            .WithMessage("ReferenceId must be greater than 0");

    }
}

/// <summary>
/// Validator for UpdateAccessRestrictionRecord.
/// </summary>
public class UpdateAccessRestrictionRecordValidator : BaseRecordValidator<UpdateAccessRestrictionRecord>
{
    public UpdateAccessRestrictionRecordValidator()
    {
        RuleFor(x => x.AccessRestrictionId)
            .GreaterThan(0)
            .WithMessage("AccessRestrictionId must be greater than 0");

        RuleFor(x => x.HrRecordId)
            .GreaterThan(0)
            .WithMessage("HrRecordId must be greater than 0");

        RuleFor(x => x.ReferenceId)
            .GreaterThan(0)
            .WithMessage("ReferenceId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteAccessRestrictionRecord.
/// </summary>
public class DeleteAccessRestrictionRecordValidator : BaseRecordValidator<DeleteAccessRestrictionRecord>
{
    public DeleteAccessRestrictionRecordValidator()
    {
        RuleFor(x => x.AccessRestrictionId)
            .GreaterThan(0)
            .WithMessage("AccessRestrictionId must be greater than 0");

    }
}