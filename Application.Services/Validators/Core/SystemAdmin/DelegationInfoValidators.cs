using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateDelegationInfoRecord.
/// </summary>
public class CreateDelegationInfoRecordValidator : BaseRecordValidator<CreateDelegationInfoRecord>
{
    public CreateDelegationInfoRecordValidator()
    {
    }
}

/// <summary>
/// Validator for UpdateDelegationInfoRecord.
/// </summary>
public class UpdateDelegationInfoRecordValidator : BaseRecordValidator<UpdateDelegationInfoRecord>
{
    public UpdateDelegationInfoRecordValidator()
    {
        RuleFor(x => x.DeligationId)
            .GreaterThan(0)
            .WithMessage("DeligationId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteDelegationInfoRecord.
/// </summary>
public class DeleteDelegationInfoRecordValidator : BaseRecordValidator<DeleteDelegationInfoRecord>
{
    public DeleteDelegationInfoRecordValidator()
    {
        RuleFor(x => x.DeligationId)
            .GreaterThan(0)
            .WithMessage("DeligationId must be greater than 0");

    }
}