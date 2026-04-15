using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateGroupPermissionRecord.
/// </summary>
public class CreateGroupPermissionRecordValidator : BaseRecordValidator<CreateGroupPermissionRecord>
{
    public CreateGroupPermissionRecordValidator()
    {
        RuleFor(x => x.Permissiontablename)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Permissiontablename))
            .WithMessage($"Permissiontablename cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for UpdateGroupPermissionRecord.
/// </summary>
public class UpdateGroupPermissionRecordValidator : BaseRecordValidator<UpdateGroupPermissionRecord>
{
    public UpdateGroupPermissionRecordValidator()
    {
        RuleFor(x => x.PermissionId)
            .GreaterThan(0)
            .WithMessage("PermissionId must be greater than 0");

        RuleFor(x => x.Permissiontablename)
            .MaximumLength(MaxStringLength)
            .When(x => !string.IsNullOrEmpty(x.Permissiontablename))
            .WithMessage($"Permissiontablename cannot exceed {MaxStringLength} characters");

    }
}

/// <summary>
/// Validator for DeleteGroupPermissionRecord.
/// </summary>
public class DeleteGroupPermissionRecordValidator : BaseRecordValidator<DeleteGroupPermissionRecord>
{
    public DeleteGroupPermissionRecordValidator()
    {
        RuleFor(x => x.PermissionId)
            .GreaterThan(0)
            .WithMessage("PermissionId must be greater than 0");

    }
}