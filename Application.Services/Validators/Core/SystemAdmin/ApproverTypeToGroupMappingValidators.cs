using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateApproverTypeToGroupMappingRecord.
/// </summary>
public class CreateApproverTypeToGroupMappingRecordValidator : BaseRecordValidator<CreateApproverTypeToGroupMappingRecord>
{
    public CreateApproverTypeToGroupMappingRecordValidator()
    {
    }
}

/// <summary>
/// Validator for UpdateApproverTypeToGroupMappingRecord.
/// </summary>
public class UpdateApproverTypeToGroupMappingRecordValidator : BaseRecordValidator<UpdateApproverTypeToGroupMappingRecord>
{
    public UpdateApproverTypeToGroupMappingRecordValidator()
    {
        RuleFor(x => x.ApproverTypeMapId)
            .GreaterThan(0)
            .WithMessage("ApproverTypeMapId must be greater than 0");

    }
}

/// <summary>
/// Validator for DeleteApproverTypeToGroupMappingRecord.
/// </summary>
public class DeleteApproverTypeToGroupMappingRecordValidator : BaseRecordValidator<DeleteApproverTypeToGroupMappingRecord>
{
    public DeleteApproverTypeToGroupMappingRecordValidator()
    {
        RuleFor(x => x.ApproverTypeMapId)
            .GreaterThan(0)
            .WithMessage("ApproverTypeMapId must be greater than 0");

    }
}