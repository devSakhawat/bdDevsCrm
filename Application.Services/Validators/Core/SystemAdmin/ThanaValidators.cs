using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

/// <summary>
/// Validator for CreateThanaRecord.
/// </summary>
public class CreateThanaRecordValidator : BaseRecordValidator<CreateThanaRecord>
{
    public CreateThanaRecordValidator()
    {
        RuleFor(x => x.DistrictId)
            .GreaterThan(0)
            .WithMessage("District ID must be greater than 0");

        RuleFor(x => x.ThanaName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaName))
            .WithMessage($"Thana name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.ThanaCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaCode))
            .WithMessage($"Thana code cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.ThanaNameBn)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaNameBn))
            .WithMessage($"Thana name (Bangla) cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for UpdateThanaRecord.
/// </summary>
public class UpdateThanaRecordValidator : BaseRecordValidator<UpdateThanaRecord>
{
    public UpdateThanaRecordValidator()
    {
        RuleFor(x => x.ThanaId)
            .GreaterThan(0)
            .WithMessage("Thana ID must be greater than 0");

        RuleFor(x => x.DistrictId)
            .GreaterThan(0)
            .WithMessage("District ID must be greater than 0");

        RuleFor(x => x.ThanaName)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaName))
            .WithMessage($"Thana name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.ThanaCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaCode))
            .WithMessage($"Thana code cannot exceed {MaxCodeLength} characters");

        RuleFor(x => x.ThanaNameBn)
            .MaximumLength(MaxNameLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaNameBn))
            .WithMessage($"Thana name (Bangla) cannot exceed {MaxNameLength} characters");
    }
}

/// <summary>
/// Validator for DeleteThanaRecord.
/// </summary>
public class DeleteThanaRecordValidator : BaseRecordValidator<DeleteThanaRecord>
{
    public DeleteThanaRecordValidator()
    {
        RuleFor(x => x.ThanaId)
            .GreaterThan(0)
            .WithMessage("Thana ID must be greater than 0");
    }
}
