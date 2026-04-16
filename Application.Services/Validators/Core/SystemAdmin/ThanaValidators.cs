using bdDevs.Shared.Records.Core.SystemAdmin;
using FluentValidation;

namespace Application.Services.Validators.Core.SystemAdmin;

public class CreateThanaRecordValidator : BaseRecordValidator<CreateThanaRecord>
{
    public CreateThanaRecordValidator()
    {
        RuleFor(x => x.DistrictId)
            .GreaterThan(0)
            .WithMessage("District ID is required");

        RuleFor(x => x.ThanaName)
            .NotEmpty()
            .WithMessage("Thana name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Thana name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.ThanaCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaCode))
            .WithMessage($"Thana code cannot exceed {MaxCodeLength} characters");
    }
}

public class UpdateThanaRecordValidator : BaseRecordValidator<UpdateThanaRecord>
{
    public UpdateThanaRecordValidator()
    {
        RuleFor(x => x.ThanaId)
            .GreaterThan(0)
            .WithMessage("Thana ID must be greater than 0");

        RuleFor(x => x.DistrictId)
            .GreaterThan(0)
            .WithMessage("District ID is required");

        RuleFor(x => x.ThanaName)
            .NotEmpty()
            .WithMessage("Thana name is required")
            .MaximumLength(MaxNameLength)
            .WithMessage($"Thana name cannot exceed {MaxNameLength} characters");

        RuleFor(x => x.ThanaCode)
            .MaximumLength(MaxCodeLength)
            .When(x => !string.IsNullOrEmpty(x.ThanaCode))
            .WithMessage($"Thana code cannot exceed {MaxCodeLength} characters");
    }
}

public class DeleteThanaRecordValidator : BaseRecordValidator<DeleteThanaRecord>
{
    public DeleteThanaRecordValidator()
    {
        RuleFor(x => x.ThanaId)
            .GreaterThan(0)
            .WithMessage("Thana ID must be greater than 0");
    }
}
